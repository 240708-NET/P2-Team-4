import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import './DungeonStyle.css';
import CreatePlayer from './components/CreatePlayer';
import Game from './components/Game';
import Login from './components/Login';
import Select from './components/Select';
import Stats from './components/Stats';

function App() {
  return (
    <Router>
      <div className="grid-container">
        <header className="header">
          <nav>
            <ul>
            <li><Link to="/login">Login</Link></li>
              <li><Link to="/create">Create Player</Link></li>
              <li><Link to="/game">Game</Link></li>
           
              <li><Link to="/select">Select</Link></li>
              <li><Link to="/stats">Stats</Link></li>
            </ul>
          </nav>
        </header>
        <aside className="left">
          {/* Left sidebar content */}
        </aside>
        <main className="middle">
          <Routes>
            <Route path="/create" element={<CreatePlayer />} />
            <Route path="/game" element={<Game />} />
            <Route path="/login" element={<Login />} />
            <Route path="/select" element={<Select />} />
            <Route path="/stats" element={<Stats />} />
            <Route path="/" element={<CreatePlayer />} />
          </Routes>
        </main>
        <aside className="right">
          {/* Right sidebar content */}
        </aside>
        <footer className="footer">
          {/* Footer content */}
        </footer>
      </div>
    </Router>
  );
}

export default App;
