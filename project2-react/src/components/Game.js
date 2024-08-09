import React, { useEffect, useState } from 'react';

import '../DungeonStyle.css';

const Game = () => {
  const [enemyName, setEnemyName] = useState('');
  const [enemyHealth, setEnemyHealth] = useState('');
  const [enemyPAC, setEnemyPAC] = useState('');

  const [playerName, setPlayerName] = useState('');
  const [playerHealth, setPlayerHealth] = useState('');
  const [playerPAC, setPlayerPAC] = useState('');

  const [hitText, setHitText] = useState('');
  const [damageText, setDamageText] = useState('');
  const [impactText, setImpactText] = useState('');

  const fetchData = async () => {
    try {
      setEnemyName((await (await fetch("http://localhost:5201/getCombatEnemyName/1")).text()).split('_')[0]);
      setEnemyHealth(await (await fetch("http://localhost:5201/getCombatEnemyHealth/1")).text());
      setEnemyPAC(await (await fetch("http://localhost:5201/getCombatEnemyPAC/1")).text());

      setPlayerName((await (await fetch("http://localhost:5201/getCombatPlayerName/1")).text()).split('_')[0]);
      setPlayerHealth(await (await fetch("http://localhost:5201/getCombatPlayerHealth/1")).text());
      setPlayerPAC(await (await fetch("http://localhost:5201/getCombatPlayerAC/1")).text());
    }
    catch (error) {
        console.error(error);
    }
  };

  useEffect(() => { fetchData(); }, []);

  const PlayerAttack = async () => {
    let response = await (await fetch("http://localhost:5201/playerAttacks/1")).text();
    console.log(response);
    let responseArr = response.split("/n");
    let toHit = responseArr[0].split("_")[1];

    if (toHit != "-999") {
      setHitText(responseArr[0].split("_")[0]);
      setDamageText(responseArr[1]);
      setImpactText(responseArr[2]);

      if (enemyHealth.split("/")[0] <= 0) {
        let enemyId = await (await fetch("http://localhost:5201/getCombatEnemyId/1")).text();
        fetch('http://localhost:5201/resetEnemyHealth/'+enemyId, { method: "PUT", body:null });
        console.log("dead");
      }
    }

    fetchData();
  }

  const handleSaveAndExit = () => {
    console.log('Save and Exit clicked');
  };

  return (
    <div className="grid-container">
      <div className="header">
        <h1>Game Name</h1>
      </div>
      <div className="left">
      </div>
      <div className="middle">
        <form>
          <div id="Container">
              <p class="Enemy">{enemyName}</p>
              <p class="Enemy">HP: {enemyHealth}</p>
              <p class="Enemy">AC: {enemyPAC}</p>
              
              <p class="Player">{playerName}</p>
              <p class="Player">HP: {playerHealth}</p>
              <p class="Player">AC: {playerPAC}</p>
          </div>    
          <div className="button-group">
            <button type="button" onClick={PlayerAttack}>Attack</button>
            <button type="button" onClick={handleSaveAndExit}>Save & Exit</button>
          </div>
          <div>
            <p>{hitText}</p>
            <p>{damageText}</p>
            <p>{impactText}</p>
          </div>
        </form>
      </div>
      <div className="right"></div>
      <div className="footer">
        <p>&copy; 2024 Revature Team 4</p>
      </div>
    </div>
  );
};

export default Game;
