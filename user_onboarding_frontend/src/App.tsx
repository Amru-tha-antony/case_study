import React, { useState } from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import Register from './pages/Register';
import Login from './pages/Login';
import UserStatus from './pages/UserStatus';
import AdminLogin from './pages/AdminLogin';
import AdminDashboard from './pages/AdminDashboard';

function App() {
  const [userToken, setUserToken] = useState<string | null>(null);
  const [userStatus, setUserStatus] = useState<string | null>(null);
  const [adminToken, setAdminToken] = useState<string | null>(null);

  return (
    <Router>
      <nav>
        <Link to="/register">Register</Link> | <Link to="/login">Login</Link> | <Link to="/user-status">User Status</Link> | <Link to="/admin">Admin</Link>
      </nav>
      <Routes>
        <Route path="/register" element={<Register />} />
        <Route path="/login" element={<Login onLogin={(token, status) => { setUserToken(token); setUserStatus(status); }} />} />
        <Route path="/user-status" element={userToken ? <UserStatus token={userToken} /> : <p>Please login first.</p>} />
        <Route path="/admin" element={<AdminLogin onLogin={setAdminToken} />} />
        <Route path="/admin-dashboard" element={adminToken ? <AdminDashboard token={adminToken} /> : <p>Please login as admin.</p>} />
        <Route path="/" element={<p>Welcome to the User Onboarding Platform</p>} />
      </Routes>
    </Router>
  );
}

export default App;
