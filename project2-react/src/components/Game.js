import React, { useState } from 'react';
import '../DungeonStyle.css';

const Game = () => {
  const [stats, setStats] = useState('');
  const [scenarios, setScenarios] = useState('');

  const handleChange = (e) => {
    const { name, value } = e.target;
    if (name === 'stats') {
      setStats(value);
    } else if (name === 'scenarios') {
      setScenarios(value);
    }
  };

  const handleAction = (action) => {
    console.log(`Action ${action} clicked`);
  };

  const handleSaveAndExit = () => {
    console.log('Save and Exit clicked');
  };

  return (
    <div className="grid-container">
      <div className="header">
        <h1>Game Name</h1>
      </div>
      <div className="left"></div>
      <div className="middle">
        <form>
          <label htmlFor="stat">Stats</label>
          <textarea
            id="stats"
            name="stats"
            rows="15"
            cols="20"
            value={stats}
            onChange={handleChange}
          />

          <label htmlFor="scenario">Scenarios</label>
          <textarea
            id="scenarios"
            name="scenarios"
            rows="5"
            cols="50"
            value={scenarios}
            onChange={handleChange}
          />
          
          <div className="button-group">
            <button type="button" onClick={() => handleAction(1)}>Action 1</button>
            <button type="button" onClick={() => handleAction(2)}>Action 2</button>
            <button type="button" onClick={() => handleAction(3)}>Action 3</button>
            <button type="button" onClick={() => handleAction(4)}>Action 4</button>
            <button type="button" onClick={handleSaveAndExit}>Save & Exit</button>
          </div>
        </form>
      </div>
      <div className="right"></div>
      <div className="footer">
        <p>&copy; 2024 Game Company</p>
      </div>
    </div>
  );
};

export default Game;
