import React, { useState, useEffect, useRef } from 'react';
import { loginUser } from '../../services/api';
import '../style/LoginStyle.css';

const LoginPage = ({ onLoginSuccess }) => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const isMounted = useRef(true); 

    useEffect(() => {
        return () => {
            isMounted.current = false;
        };
    }, []);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setLoading(true);

        try {
            const userData = await loginUser(username, password);
            onLoginSuccess(userData); 
            // console.log('userData:', userData);
        } catch (err) {
            if (isMounted.current) { 
                setError(err); 
            }
        } finally {
            if (isMounted.current) { 
                setLoading(false);
            }
        }
    };

    return (
        <div className="login-page-container"> 
            <div className="login-box">
                <h2>👤 Login</h2>
                <form className="login-form" onSubmit={handleSubmit}>
                    <div>
                        <label htmlFor="username">Username:</label>
                        <input 
                            id="username"
                            type="text" 
                            value={username} 
                            onChange={(e) => setUsername(e.target.value)} 
                            required 
                            autoFocus
                        />
                    </div>

                    <div>
                        <label htmlFor="password">Password:</label>
                        <input 
                            id="password"
                            type="password" 
                            value={password} 
                            onChange={(e) => setPassword(e.target.value)} 
                            required 
                        />
                    </div>

                    {error && <p className="login-error">{error}</p>}

                    <button 
                        type="submit" 
                        className="login-button"
                        disabled={loading}
                    >
                        {loading ? '...' : 'Login'}
                    </button>
                </form>
            </div>
        </div>
    );
};

export default LoginPage;