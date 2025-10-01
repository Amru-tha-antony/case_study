import React, { useEffect, useState } from 'react';

const UserStatus: React.FC<{ token: string }> = ({ token }) => {
  const [status, setStatus] = useState('');

  useEffect(() => {
    const fetchStatus = async () => {
      const res = await fetch(`${process.env.REACT_APP_API_URL}/user/status`, {
        headers: { Authorization: `Bearer ${token}` }
      });
      const data = await res.json();
      setStatus(data.status);
    };
    fetchStatus();
  }, [token]);

  return (
    <div>
      <h2>Your Status</h2>
      <p>Status: {status}</p>
    </div>
  );
};

export default UserStatus;
