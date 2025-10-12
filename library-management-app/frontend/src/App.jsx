import React, { useState, useEffect } from 'react';
import BookList from './components/pages/BookList';
import LoginPage from './components/pages/LoginPage';
import { logoutUser } from './services/api'; 

const App = () => {
    const [user, setUser] = useState(null);

    useEffect(() => {
      const storedUserToken = localStorage.getItem('userToken');
      const storedUserName = localStorage.getItem('userName');
      const storedRolesJSON = localStorage.getItem('userRole');

      if (storedUserToken && storedUserName && storedRolesJSON) {
        try {
          let storedRoles = JSON.parse(storedRolesJSON);
          if (typeof storedRoles === 'string') {
            storedRoles = [storedRoles];
          }
          
          setUser({ 
              isAuthenticated: true, 
              userName: storedUserName, 
              userRole: storedRoles
          });
        } catch (error) {
            console.error("Error:", error);
            logoutUser();
        }
      }
    }, []);

    const handleLoginSuccess = (userData ) => {
      let finalRoles = [];
      if (typeof userData.roles === 'string') {
          finalRoles = [userData.roles];
      } else if (Array.isArray(userData.roles)) {
          finalRoles = userData.roles;
      }      
      setUser({
          isAuthenticated: true,
          userName: userData.userName,
          userRole: finalRoles
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
            <BookList roles={user.userRole} />
        </main>
      </div>
    );
};

export default App;