import React, { useEffect, useState } from 'react';

const Leaderboard = () => {
  const [players, setPlayers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchPlayers = async () => {
      try {
        const response = await fetch('http://localhost:5201/getAllPlayers');
        if (response.ok) {
          const data = await response.json();

          // Log the response data for debugging
          console.log('Fetched data:', data);

          // Convert dictionary to array for sorting and rendering
          const playersArray = Object.values(data);
          
          if (Array.isArray(playersArray)) {
            // Sort players by score
            const sortedPlayers = playersArray.sort((a, b) => (b.score || 0) - (a.score || 0));
            setPlayers(sortedPlayers);
          } else {
            throw new Error('Expected data to be a dictionary but received: ' + typeof data);
          }
        } else {
          throw new Error('Failed to fetch players: ' + response.statusText);
        }
      } catch (error) {
        setError(error.message);
        console.error('Error fetching players:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchPlayers();
  }, []);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
    <div className="leaderboard-container">
      <h2>Leaderboard</h2>
      <table className="leaderboard-table">
        <thead>
          <tr>
            <th>Rank</th>
            <th>Player Name</th>
            <th>Score</th>
            {}
          </tr>
        </thead>
        <tbody>
          {players.length > 0 ? (
            players.map((player, index) => (
              <tr key={player.id}>
                <td>{index + 1}</td>
                <td>{player.name}</td>
                <td>{player.score || 'N/A'}</td> {}
                {}
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="3">No players found</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
};

export default Leaderboard;
