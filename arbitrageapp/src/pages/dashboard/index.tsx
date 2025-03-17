import React, { useEffect, useState } from "react";
import axios from "axios";
import ArbitrageCard from "../../components/arbitradecard";
import { useAuth } from "../../Contexts/AuthContext";
import { ArbitrageDto } from "../../apis/arbitrage";
import { Container } from "@mui/material";

const API_BASE_URL = "https://localhost:7238";

const Dashboard: React.FC = () => {
    const { token, logout } = useAuth();
    const [bitcoinData, setBitcoinData] = useState(null);
    const [xrpData, setXrpData] = useState(null);
    const [bitcoinHistory, setBitcoinHistory] = useState<ArbitrageDto[]>([]);
    const [xrpHistory, setXrpHistory] = useState<ArbitrageDto[]>([]);
    const [error, setError] = useState("");

    const fetchData = async () => {
        try {
            const [bitcoinResponse, xrpResponse] = await Promise.all([
                axios.get(`${API_BASE_URL}/api/arbitrage/bitcoin`, { headers: { Authorization: `Bearer ${token}` } }),
                axios.get(`${API_BASE_URL}/api/arbitrage/xrp`, { headers: { Authorization: `Bearer ${token}` } }),
            ]);

            setBitcoinData(bitcoinResponse.data);
            setXrpData(xrpResponse.data);
            setBitcoinHistory((prev) => [...prev.slice(-9), bitcoinResponse.data]);
            setXrpHistory((prev) => [...prev.slice(-9), xrpResponse.data]);
        } catch {
            setError("Failed to fetch data");
        }
    };

    useEffect(() => {
        if (token) {
            fetchData();
            const interval = setInterval(fetchData, 30000);
            return () => clearInterval(interval);
        }
    }, [token]);

    if (error) return <div>{error}</div>;
    if (!bitcoinData || !xrpData) return <div>Loading...</div>;

    return (

        <Container >
            <div className="p-4 space-y-4">
                <h1 className="text-2xl font-bold">Arbitrage Dashboard</h1>
                <button onClick={logout}>Logout</button>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <ArbitrageCard currentData={bitcoinData} history={bitcoinHistory} title={"Bitcoin"} />
                    <ArbitrageCard currentData={xrpData} history={xrpHistory} title={"XRP"} />
                </div>
            </div>
        </Container>
    );
};

export default Dashboard;
