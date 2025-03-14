import React, { FC, useEffect, useState } from 'react';
//import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import axios from 'axios';
import ArbitrageCard from '../../components/arbitradecard/index';
import { ArbitrageDto } from '../../apis/arbitrage';
import { Input, Button } from '@mui/material';
const API_BASE_URL = 'https://localhost:7238'
const ArbitrageDashboard: FC = () => {
    const [bitcoinData, setBitcoinData] = useState<ArbitrageDto|null>(null);
    const [xrpData, setXrpData] = useState<ArbitrageDto|null>(null);
    const [bitcoinHistory, setBitcoinHistory] = useState<ArbitrageDto[]>([]);
    const [xrpHistory, setXrpHistory] = useState<ArbitrageDto[]>([]);
    const [error, setError] = useState('');
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [token, setToken] = useState(localStorage.getItem('token'));

    const login = async () => {
        try {
            const response = await axios.post(`${API_BASE_URL}/Api/Authentication/login`, { email:username, password },
                {
                    headers: {
                        'Content-Type': 'application/json', // Add Content-Type header
                    },
                }
            );
            setToken(response.data.token);
            localStorage.setItem('token', response.data.token);
            setIsAuthenticated(true);
            setError('');
        } catch {
            setError('Invalid credentials');
        }
    };

    const logout = () => {
        setToken(null);
        localStorage.removeItem('token');
        setIsAuthenticated(false);
    };

    const fetchData = async () => {
        try {
            const bitcoinResponse = await axios.get(`${API_BASE_URL}/api/arbitrage/bitcoin`, {
                headers: { Authorization: `Bearer ${token}`, }
            });
            const xrpResponse = await axios.get(`${API_BASE_URL}/api/arbitrage/xrp`, {
                headers: { Authorization: `Bearer ${token}`,}
            });
            debugger;
            if (bitcoinResponse.status === 302 || xrpResponse.status === 302) {
                setError('Authentication required. Please log in again.');
                logout();
                return;
            }

            setBitcoinData(bitcoinResponse.data);
            setXrpData(xrpResponse.data);
            setBitcoinHistory((prev) => [...prev.slice(-9), bitcoinResponse.data]);
            setXrpHistory((prev) => [...prev.slice(-9), xrpResponse.data]);
        } catch (err) {
            setError('Failed to fetch data');
        }
    };

    useEffect(() => {
        if (token) {
            setIsAuthenticated(true);
            fetchData();
            const interval = setInterval(fetchData, 30000);
            return () => clearInterval(interval);
        }
    }, [token]);

    if (!isAuthenticated) {
        return (
            <div className="p-4 space-y-4">
                <h1 className="text-2xl font-bold">Login</h1>
                <Input placeholder="Username" value={username} onChange={(e) => setUsername(e.target.value)} />
                <Input placeholder="Password" type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
                <Button onClick={login}>Login</Button>
                {error && <p className="text-red-500">{error}</p>}
            </div>
        );
    }
    if (error) return <div>{error}</div>;
    if (!bitcoinData || !xrpData) return <div>Loading...</div>;

    return (
        <div className="p-4 space-y-4">
            <h1 className="text-2xl font-bold"> Arbitrage Dashboard</h1>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <ArbitrageCard currentData={bitcoinData} history={bitcoinHistory}  title={"Bitcoin"}/>
                
                <ArbitrageCard currentData={xrpData} history={xrpHistory} title={"XRP"}/>
            </div>
        </div>
    );
};

export default ArbitrageDashboard;
