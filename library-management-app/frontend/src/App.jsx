import React, { useState, useEffect } from 'react';
import BookList from './components/pages/BookList';
import LoginPage from './components/pages/LoginPage';
import { logoutUser } from './services/api'; 

const App = () => {
    const [user, setUser] = useState(null);

    useEffect(() => {
      const token = localStorage.getItem('token');
      const storedUsername = localStorage.getItem('userName');
      const storedRolesJSON = localStorage.getItem('roles');      

      if (token && storedUsername && storedRolesJSON) {
        //setUser({ isAuthenticated: true, roles: [], username: 'Loading...' }); 
        try {
          const storedRoles = JSON.parse(storedRolesJSON);
          
          setUser({ 
              isAuthenticated: true, 
              username: storedUsername, 
              roles: storedRoles
          });
        } catch (error) {
            console.error("Error:", error);
            logoutUser();
        }
      }
    }, []);

    const handleLoginSuccess = (userData ) => {
      setUser({
          isAuthenticated: true,
          username: userData.userName,
          roles: userData.roles 
      });
    };

    const handleLogout = () => {
      logoutUser();
      setUser(null);
    };

    if (!user || !user.isAuthenticated) {
      return <LoginPage onLoginSuccess={handleLoginSuccess} />;
    }

    return (
      <div className="app-container">
        <header className="app-header">
            <h1>📚 AP Library</h1>
            <div style={{ position: 'absolute', top: 20, right: 40, display: 'flex', alignItems: 'center', gap: '10px' }}>
                <span style={{ fontSize: '14px', color: '#636366' }}>Hi, {user.username}!</span>
                <button 
                    className="delete-button"
                    onClick={handleLogout}
                    style={{ padding: '4px 10px', fontSize: '12px' }}
                >
                    Logout
                </button>
            </div>
        </header>
        <main>
            <BookList roles={user.role} />
        </main>
      </div>
    );
};

export default App;