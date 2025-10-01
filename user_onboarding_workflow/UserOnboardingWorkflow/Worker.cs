namespace UserOnboardingWorkflow;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Production.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var serviceBusConnection = config["ServiceBus:ConnectionString"];
        var queueName = config["ServiceBus:QueueName"];
        var dbConnection = config.GetConnectionString("DefaultConnection");

        var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<Data.UserDbContext>()
            .UseSqlServer(dbConnection)
            .Options;

        var client = new Azure.Messaging.ServiceBus.ServiceBusClient(serviceBusConnection);
        var processor = client.CreateProcessor(queueName, new Azure.Messaging.ServiceBus.ServiceBusProcessorOptions());

        processor.ProcessMessageAsync += async (args) =>
        {
            var body = args.Message.Body.ToString();
            // Expecting JSON: { "UserId": 123, "Action": "Approve" }
            var data = System.Text.Json.JsonDocument.Parse(body).RootElement;
            int userId = data.GetProperty("UserId").GetInt32();
            string action = data.GetProperty("Action").GetString();

            using var db = new Data.UserDbContext(options);
            var user = await db.Users.FindAsync(userId);
            if (user != null)
            {
                user.Status = action == "Approve" ? Models.UserStatus.Approved : Models.UserStatus.Rejected;
                await db.SaveChangesAsync();
                _logger.LogInformation($"User {userId} status updated to {user.Status}");
            }
            await args.CompleteMessageAsync(args.Message);
        };

        processor.ProcessErrorAsync += async (args) =>
        {
            _logger.LogError(args.Exception, "Service Bus error");
            await Task.CompletedTask;
        };

        await processor.StartProcessingAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }

        await processor.StopProcessingAsync();
    }
}
