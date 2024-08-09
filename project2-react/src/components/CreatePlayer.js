import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import '../DungeonStyle.css';

const CreatePlayer = ({ userId }) => {
  const [step, setStep] = useState(1);
  const [charName, setCharName] = useState('');
  const [rollType, setRollType] = useState('');
  const [attributesPool, setAttributesPool] = useState([]);
  const [assignedAttributes, setAssignedAttributes] = useState({
    strength: 0,
    dexterity: 0,
    constitution: 0,
    intelligence: 0,
    wisdom: 0,
    charisma: 0,
  });
  const [remainingPoints, setRemainingPoints] = useState(0);
  const [characterClass, setCharacterClass] = useState('fighter');
  const { userId: routeUserId } = useParams(); // Get from URL if passed
  const navigate = useNavigate();

  useEffect(() => {
    console.log('User ID:', userId || routeUserId);
  }, [userId, routeUserId]);

  const rollAttributes = (type) => {
    let rolls = [];
    let rolledAttributes = [];

    switch (type) {
      case '4d6d1':
        for (let i = 0; i < 6; i++) {
          rolls = [];
          for (let j = 0; j < 4; j++) {
            rolls.push(Math.floor(Math.random() * 6) + 1);
          }
          rolls.sort((a, b) => a - b);
          rolledAttributes.push(rolls.slice(1).reduce((a, b) => a + b, 0));
        }
        break;

      case '3d6':
        for (let i = 0; i < 6; i++) {
          let total = 0;
          for (let j = 0; j < 3; j++) {
            total += Math.floor(Math.random() * 6) + 1;
          }
          rolledAttributes.push(total);
        }
        break;

      case '2d6+6':
        for (let i = 0; i < 6; i++) {
          let total = 6;
          for (let j = 0; j < 2; j++) {
            total += Math.floor(Math.random() * 6) + 1;
          }
          rolledAttributes.push(total);
        }
        break;

      default:
        rolledAttributes = [10, 10, 10, 10, 10, 10];
    }

    return rolledAttributes;
  };

  const handleRollAttributes = async () => {
    try {
      const rolled = rollAttributes(rollType);
      setAttributesPool(rolled);
      setRemainingPoints(rolled.reduce((a, b) => a + b, 0));
      setStep(3);
    } catch (error) {
      alert('An error occurred while rolling attributes.');
    }
  };

  const handleSliderChange = (attrName, value) => {
    const currentPointsUsed = Object.values(assignedAttributes).reduce((a, b) => a + b, 0);
    const updatedPointsUsed = currentPointsUsed + value - assignedAttributes[attrName];
    if (updatedPointsUsed <= remainingPoints) {
      setAssignedAttributes((prev) => ({ ...prev, [attrName]: value }));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
  
    const finalUserId = userId || routeUserId;
  
    try {
      const response = await fetch(`http://localhost:5201/createEmptyPlayer/${finalUserId}/${charName}`, {
        method: 'POST',
      });
  
      if (response.ok) {
        const newPlayer = await response.json();
        if (newPlayer && newPlayer.id) {
          const attributesString = Object.values(assignedAttributes).join(',');
          const attributeResponse = await fetch(`http://localhost:5201/createPlayerAttributes/${newPlayer.id}/${attributesString}`, {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
            },
          });
  
          if (attributeResponse.ok) {
            const classResponse = await fetch(`http://localhost:5201/createPlayerClass/${newPlayer.id}`, {
              method: 'POST',
              headers: {
                'Content-Type': 'application/json',
              },
              body: JSON.stringify(characterClass),
            });
  
            if (classResponse.ok) {
              await Promise.all([
                fetch(`http://localhost:5201/createPlayerLevel/${newPlayer.id}`, {
                  method: 'POST',
                  headers: {
                    'Content-Type': 'application/json',
                  },
                  body: JSON.stringify("5_3000/6000"),
                }),
                fetch(`http://localhost:5201/createPlayerSkill/${newPlayer.id}`, {
                  method: 'POST',
                  headers: {
                    'Content-Type': 'application/json',
                  },
                  body: JSON.stringify(3),
                }),
                fetch(`http://localhost:5201/createPlayerHealth/${newPlayer.id}`, {
                  method: 'POST',
                  headers: {
                    'Content-Type': 'application/json',
                  },
                  body: JSON.stringify("5d10"),
                }),
                fetch(`http://localhost:5201/createPlayerUnarmed/${newPlayer.id}`, {
                  method: 'POST',
                  headers: {
                    'Content-Type': 'application/json',
                  },
                  body: JSON.stringify("fists_punches with their_Melee_0/0_1_bludgeoning"),
                }),
                fetch(`http://localhost:5201/createPlayerAttack/${newPlayer.id}`, {
                  method: 'POST',
                  headers: {
                    'Content-Type': 'application/json',
                  },
                  body: JSON.stringify("longsword_swings with their_Melee_0/1d8_0_slashing"),
                }),
                fetch(`http://localhost:5201/createPlayerDefense/${newPlayer.id}`, {
                  method: 'POST',
                  headers: {
                    'Content-Type': 'application/json',
                  },
                  body: JSON.stringify("Breastplate_14+DEX/M2"),
                }),
              ]);

              alert('Player created successfully!');
              const response = await fetch(`http://localhost:5201/getPlayerByName/${finalUserId}/${charName}`);
              if (response.ok) {
                const playerId = await response.text();

                // Update the player's ID and navigate to the game screen
                console.log('Navigating to:', `/game/${finalUserId}/${playerId}`)
                navigate(`/game/${finalUserId}/${playerId}`);
              } else {
                console.error('Failed to fetch created player details.');
              }
            } else {
              alert('Failed to assign class.');
            }
          } else {
            alert('Failed to create player attributes.');
          }
        }
      } else {
        alert('Failed to create player.');
      }
    } catch (error) {
      alert('An error occurred while creating the player.');
    }
  };
  

  const renderStepContent = () => {
    switch (step) {
      case 1:
        return (
          <div>
            <label htmlFor="charName">Character Name:</label>
            <input
              type="text"
              id="charName"
              value={charName}
              onChange={(e) => setCharName(e.target.value)}
              required
            />
            <button type="button" onClick={() => setStep(2)}>Continue</button>
          </div>
        );
      case 2:
        return (
          <div>
            <label>Roll Type:</label>
            <select value={rollType} onChange={(e) => setRollType(e.target.value)} required>
              <option value="4d6d1">4d6 drop lowest</option>
              <option value="3d6">3d6</option>
              <option value="2d6+6">2d6+6</option>
            </select>
            <button type="button" onClick={handleRollAttributes}>Roll Attributes</button>
          </div>
        );
      case 3:
        return (
          <div>
            <p>Assign Attributes (Remaining Points: {remainingPoints - Object.values(assignedAttributes).reduce((a, b) => a + b, 0)}):</p>
            {Object.keys(assignedAttributes).map((attrName) => (
              <div key={attrName}>
                <label>{attrName.charAt(0).toUpperCase() + attrName.slice(1)}:</label>
                <input
                  type="range"
                  min="0"
                  max={Math.max(...attributesPool)}
                  value={assignedAttributes[attrName]}
                  onChange={(e) => handleSliderChange(attrName, parseInt(e.target.value))}
                />
                <span>{assignedAttributes[attrName]}</span>
              </div>
            ))}
            <button type="button" onClick={() => setStep(4)}>Continue</button>
          </div>
        );
      case 4:
        return (
          <div>
            <label>Character Class:</label>
            <div>
              <input
                type="radio"
                id="fighter"
                name="characterClass"
                value="fighter"
                onChange={(e) => setCharacterClass(e.target.value)}
                required
                checked={characterClass === 'fighter'}
              />
              <label htmlFor="fighter">Fighter</label>
            </div>
            <button className="button-secondary" type="button" onClick={handleSubmit}>Create Character</button>
          </div>
        );
      default:
        return null;
    }
  };

  return (
    <div className="grid-container">
      <div className="header">
        <h1>Game Name</h1>
      </div>
      <div className="left"></div>
      <div className="middle">
        <form>
          {renderStepContent()}
        </form>
      </div>
    </div>
  );
};

export default CreatePlayer;
