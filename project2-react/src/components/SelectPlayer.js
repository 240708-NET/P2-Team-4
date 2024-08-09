import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import '../DungeonStyle.css';

const SelectPlayer = () => {
  const [players, setPlayers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();
  const { userId } = useParams(); // Get userId from route

  useEffect(() => {
    const fetchPlayers = async () => {
      try {
        setLoading(true);
        const response = await fetch(`http://localhost:5201/getAllPlayers`);
        if (response.ok) {
          const data = await response.json();
          console.log("Fetched players data:", data);

          // Filter players based on userId
          const userPlayers = Object.values(data).filter(player => player.userId === parseInt(userId));
          console.log("Filtered user players:", userPlayers);

          setPlayers(userPlayers);
        } else {
          console.error('Failed to fetch players.');
          setError('Failed to fetch players.');
        }
      } catch (error) {
        console.error('Error fetching players:', error);
        setError('Error fetching players.');
      } finally {
        setLoading(false);
      }
    };

    fetchPlayers();
  }, [userId]);

  const handleSelectPlayer = async (playerId) => {
    try {
      const response = await fetch(`http://localhost:5201/getPlayerById/${playerId}`);
      if (response.ok) {
        const playerData = await response.json();
        console.log('Selected player data:', playerData);
        // Update the player's ID and navigate to the game screen
        navigate(`/game/${userId}/${playerId}`);
      } else {
        console.error('Failed to fetch selected player details.');
      }
    } catch (error) {
      console.error('Error fetching selected player details:', error);
    }
  };

  const handleCreateNewPlayer = () => {
    navigate(`/create/${userId}`);
  };

  if (loading) {
    return <div>Loading players...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className="select-player-container">
      <h2>Select a Player</h2>
      {players.length > 0 ? (
        <ul>
          {players.map(player => (
            <li key={player.id}>
              {player.name}
              <button className="button-primary" onClick={() => handleSelectPlayer(player.id)}>Select</button>
            </li>
          ))}
        </ul>
      ) : (
        <p>No players found. Please create a new player.</p>
      )}
      <button className="button-secondary" onClick={handleCreateNewPlayer}>Create New Player</button>
    </div>
  );
};

export default SelectPlayer;
