import React, { useEffect, useState } from 'react';

const AdminDashboard: React.FC<{ token: string }> = ({ token }) => {
  const [pendingUsers, setPendingUsers] = useState<any[]>([]);

  useEffect(() => {
    const fetchPending = async () => {
      const res = await fetch(`${process.env.REACT_APP_API_URL}/admin/pending-users`, {
        headers: { Authorization: `Bearer ${token}` }
      });
      const data = await res.json();
      setPendingUsers(data);
    };
    fetchPending();
  }, [token]);

  const handleAction = async (id: number, action: 'approve' | 'reject') => {
    await fetch(`${process.env.REACT_APP_API_URL}/admin/${action}/${id}`, {
      method: 'POST',
      headers: { Authorization: `Bearer ${token}` }
    });
    setPendingUsers(pendingUsers.filter(u => u.id !== id));
  };

  return (
    <div>
      <h2>Pending Users</h2>
      <ul>
        {pendingUsers.map(user => (
          <li key={user.id}>
            {user.email} - <button onClick={() => handleAction(user.id, 'approve')}>Approve</button> <button onClick={() => handleAction(user.id, 'reject')}>Reject</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default AdminDashboard;
