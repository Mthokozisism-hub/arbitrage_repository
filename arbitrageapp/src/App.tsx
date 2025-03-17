import React, { JSX, ReactNode } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';

import './App.css';
import Login from './pages/login';
import Register from './pages/register';
import DashboardPage from './pages/dashboard';
import { AuthProvider, useAuth } from './Contexts/AuthContext';


interface PrivateRouteProps {
  children: ReactNode;
}

const PrivateRoute = ({ children }:PrivateRouteProps): JSX.Element => {
    const { token } = useAuth();
  return token ? <div className="App">{children}</div> : <Navigate to="/login" />;
};

const App = () => {
    return (
        <AuthProvider>
            <Router>
                <Routes>
                    <Route path="/login" element={<Login />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/dashboard" element={<PrivateRoute><DashboardPage /></PrivateRoute>} />
                    <Route path="*" element={<Navigate to="/login" />} />
                </Routes>
            </Router>
        </AuthProvider>
    );
};

export default App;
