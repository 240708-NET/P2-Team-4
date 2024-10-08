import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';

import '../DungeonStyle.css';

const Game = () => {
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
  const { userId, playerId } = useParams();

  const [enemyName, setEnemyName] = useState('');
  const [enemyHealth, setEnemyHealth] = useState('');
  const [enemyPAC, setEnemyPAC] = useState('');

  const [playerName, setPlayerName] = useState('');
  const [playerHealth, setPlayerHealth] = useState('');
  const [playerAC, setPlayerAC] = useState('');

  const [hitPText, setPHitText] = useState('');
  const [damagePText, setPDamageText] = useState('');
  const [impactPText, setPImpactText] = useState('');

  const [hitEText, setEHitText] = useState('');
  const [damageEText, setEDamageText] = useState('');
  const [impactEText, setEImpactText] = useState('');

  const createCombat = async () => {
    const response = await fetch("http://localhost:5201/createCombat/"+playerId, { method:"Post" });
    fetchData();
    setLoading(false);
  }

  const fetchData = async () => {
    try {
      let response = await fetch("http://localhost:5201/getCombatEnemyName/1");
      if (response.ok) {
        let eNameData = await response.text();
        eNameData = eNameData.split('_')[0];
        
        console.log("Fetched enemy name: ", eNameData);
        setEnemyName(eNameData);
      }

      response = await fetch("http://localhost:5201/getCombatEnemyHealth/1");
      if (response.ok) {
        let eHealthData = await response.text();
        
        console.log("Fetched enemy health: ", eHealthData);
        setEnemyHealth(eHealthData);
      }

      response = await fetch("http://localhost:5201/getCombatEnemyPAC/1");
      if (response.ok) {
        let ePACData = await response.text();
        
        console.log("Fetched enemy PAC: ", ePACData);
        setEnemyPAC(ePACData);
      }

      response = await fetch("http://localhost:5201/getCombatPlayerName/1");
      if (response.ok) {
        let pNameata = await response.text();
        
        console.log("Fetched player name: ", pNameata);
        setPlayerName(pNameata);
      }

      response = await fetch("http://localhost:5201/getCombatPlayerHealth/1");
      if (response.ok) {
        let pHealthData = await response.text();
        
        console.log("Fetched player health: ", pHealthData);
        setPlayerHealth(pHealthData);
      }

      response = await fetch("http://localhost:5201/getCombatPlayerAC/1");
      if (response.ok) {
        let pACData = await response.text();
        
        console.log("Fetched player AC: ", pACData);
        setPlayerAC(pACData);
      }
    }

    catch (error) {
        console.error(error);
    }
  };

  useEffect(() => { 
    if (loading == true) {
        createCombat();
    }

    else {
      fetchData(); 
    } 
  }, []);

  const PlayerAttack = async () => {
    let response = await (await fetch("http://localhost:5201/playerAttacks/1")).text();
    console.log(response);
    let responseArr = response.split("/n");
    let toHit = responseArr[0].split("_")[1];

    if (toHit != "-999") {
      setPHitText(responseArr[0].split("_")[0]);
      setPDamageText(responseArr[1]);
      setPImpactText(responseArr[2]);

      if (enemyHealth.split("/")[0] <= 0) {
        let enemyId = await (await fetch("http://localhost:5201/getCombatEnemyId/1")).text();
        fetch('http://localhost:5201/resetEnemyHealth/'+enemyId, { method: "PUT", body:null });
        console.log("dead");
      }
      else {
        response = await (await fetch("http://localhost:5201/enemyAttacks/1")).text();
        console.log(response);
        responseArr = response.split("/n");
        toHit = responseArr[0].split("_")[1];

        if (toHit != "-999") {
          setEHitText(responseArr[0].split("_")[0]);
          setEDamageText(responseArr[1]);
          setEImpactText(responseArr[2]);
        }
      }
    }

    fetchData();
  }

  const handleSaveAndExit = async () => {
    console.log('Save and Exit clicked');
    let player = await fetch(`http://localhost:5201/getPlayerById/${playerId}`);
    console.log(player);

    await fetch("http://localhost:5201/updatePlayer", {
      method: "put",
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(player)
    });
    navigate(`/login`);
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
              <p class="Player">AC: {playerAC}</p>
          </div>    
          <div className="button-group">
            <button type="button" onClick={PlayerAttack}>Attack</button>
            <button type="button" onClick={handleSaveAndExit}>Save & Exit</button>
          </div>
          <div>
            <p>{hitPText}</p>
            <p>{damagePText}</p>
            <p>{impactPText}</p>
            <p>{hitEText}</p>
            <p>{damageEText}</p>
            <p>{impactEText}</p>
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