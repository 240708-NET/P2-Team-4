import React, { useState } from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate, Link } from 'react-router-dom';
import './DungeonStyle.css';
import CreatePlayer from './components/CreatePlayer';
import Game from './components/Game';
import Login from './components/Login';
import SelectPlayer from './components/SelectPlayer';
import Stats from './components/Stats';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [userId, setUserId] = useState(null);

  const handleLogin = (id) => {
    console.log('User ID set to:', id); 
    setIsLoggedIn(true);
    setUserId(id); 
  };

  return (
    <Router>
      <div className="app-container">
        <header className="header">
          <nav>
            <ul>
              {isLoggedIn ? (
                <>
                  <li><Link to={`/create/${userId}`}>Create Player</Link></li>
                  <li><Link to="/game">Game</Link></li>
                  <li><Link to={`/select/${userId}`}>Select</Link></li>
                  <li><Link to="/stats">Stats</Link></li>
                </>
              ) : (
                <li><Link to="/login">Login</Link></li>
              )}
            </ul>
          </nav>
        </header>
        <aside className="left"></aside>
        <main className="middle">
          <Routes>
            <Route path="/login" element={<Login onLogin={handleLogin} />} />
            {isLoggedIn ? (
              <>
                <Route path="/create/:userId" element={<CreatePlayer userId={userId} />} />
                <Route path="/create/:userId/:playerId" element={<CreatePlayer userId={userId} />} />
               <Route path="/game/:userId/:playerId" element={<Game />} />
               <Route path="/game" element={<Game />} />
                <Route path="/select/:userId" element={<SelectPlayer />} />
                <Route path="/stats" element={<Stats />} />

                <Route path="/" element={<Navigate to={`/create/${userId}`} />} />
              </>
            ) : (
              <Route path="/" element={<Navigate to="/login" />} />
            )}
          </Routes>
        </main>
        <aside className="right"></aside>
        <footer className="footer">
          <p>&copy; 2024 TEAM 4</p>
        </footer>
      </div>
    </Router>
  );
}

export default App;
