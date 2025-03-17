import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Input, Button, Container } from "@mui/material";
import { useAuth } from "../../Contexts/AuthContext";

const Login: React.FC = () => {
    const { login } = useAuth();
    const navigate = useNavigate();
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    const handleLogin = async () => {
        try {
            await login(username, password);
            navigate("/dashboard");
        } catch {
            setError("Invalid credentials");
        }
    };

    return (

        <Container maxWidth="sm">
            <div className="p-4 space-y-4">
                <h1 className="text-2xl font-bold">Login</h1>
                <Input placeholder="Username" value={username} onChange={(e) => setUsername(e.target.value)} />
                <Input placeholder="Password" type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
                <Button onClick={handleLogin}>Login</Button>
                {error && <p className="text-red-500">{error}</p>}

                {/* Register Button */}
                <p className="mt-4">
                    Don't have an account?{" "}
                    <Button onClick={() => navigate("/register")} variant="outlined" color="secondary">
                        Register
                    </Button>
                </p>
            </div>
            </Container>
    );
};

export default Login;
