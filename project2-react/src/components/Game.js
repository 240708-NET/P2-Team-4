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
    <div className="game-container">
      <h2>Game</h2>
      <form>
        <div className="form-group">
          <label htmlFor="stats">Stats</label>
          <textarea
            id="stats"
            name="stats"
            rows="15"
            cols="20"
            value={stats}
            onChange={handleChange}
            className="form-control"
          />
        </div>

        <div className="form-group">
          <label htmlFor="scenarios">Scenarios</label>
          <textarea
            id="scenarios"
            name="scenarios"
            rows="5"
            cols="50"
            value={scenarios}
            onChange={handleChange}
            className="form-control"
          />
        </div>
        
        <div className="button-group">
          <button type="button" onClick={() => handleAction(1)} className="btn-action">Action 1</button>
          <button type="button" onClick={() => handleAction(2)} className="btn-action">Action 2</button>
          <button type="button" onClick={() => handleAction(3)} className="btn-action">Action 3</button>
          <button type="button" onClick={() => handleAction(4)} className="btn-action">Action 4</button>
          <button type="button" onClick={handleSaveAndExit} className="btn-save">Save & Exit</button>
        </div>
      </form>
    </div>
  );
};

export default Game;
