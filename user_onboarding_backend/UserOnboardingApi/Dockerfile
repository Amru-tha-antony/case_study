# Use official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "UserOnboardingApi.csproj"
RUN dotnet publish "UserOnboardingApi.csproj" -c Release -o /app/publish

# Use official .NET runtime image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ConnectionStrings__DefaultConnection="${DB_CONNECTION_STRING}"
ENV Jwt__Key="${JWT_SECRET}"
ENTRYPOINT ["dotnet", "UserOnboardingApi.dll"]
