import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { Input, Button, Container } from "@mui/material";

const API_BASE_URL = "https://localhost:7238";

const Register: React.FC = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const navigate = useNavigate();

    const handleRegister = async () => {
        try {
            await axios.post(`${API_BASE_URL}/Api/Authentication/register`, { email: username, password });
            navigate("/login");
        } catch {
            setError("Registration failed");
        }
    };

    return (
        <Container maxWidth="sm">
            <div className="p-4 space-y-4">
                <h1 className="text-2xl font-bold">Register</h1>
                <Input placeholder="Username" value={username} onChange={(e) => setUsername(e.target.value)} />
                <Input placeholder="Password" type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
                <Button onClick={handleRegister}>Register</Button>
                {error && <p className="text-red-500">{error}</p>}

                {/* Login Button */}
                <p className="mt-4">
                    Already have an account?{" "}
                    <Button onClick={() => navigate("/login")} variant="outlined" color="secondary">
                        Login
                    </Button>
                </p>
            </div>
        </Container>
    );
};

export default Register;
