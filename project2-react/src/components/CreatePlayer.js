import React, { useState } from 'react';
import '../DungeonStyle.css';

const CreatePlayer = () => {
  const [charName, setCharName] = useState('');
  const [rollType, setRollType] = useState('');
  const [attributes, setAttributes] = useState({
    stat1: '',
    stat2: '',
    stat3: '',
    stat4: '',
    stat5: '',
    stat6: ''
  });
  const [characterClass, setCharacterClass] = useState('');

  const handleChange = (e) => {
    const { name, value } = e.target;
    if (name === 'charName') {
      setCharName(value);
    } else if (name === 'rollType') {
      setRollType(value);
    } else if (name === 'class') {
      setCharacterClass(value);
    }
  };

  const handleAttributeChange = (stat, value) => {
    setAttributes({
      ...attributes,
      [stat]: value
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log({
      charName,
      rollType,
      attributes,
      characterClass
    });
  };

  const droppoint = (event) => {
    event.preventDefault();
    const data = event.dataTransfer.getData('text/plain');
    const target = event.target;
    target.appendChild(document.getElementById(data));
  };

  const allowDropOption = (event) => {
    event.preventDefault();
  };

  const dragpoint = (event) => {
    event.dataTransfer.setData('text/plain', event.target.id);
  };

  return (
    <div className="grid-container">
      <div className="header">
        <h1>Game Name</h1>
      </div>
      <div className="left"></div>
      <div className="middle">
        <form onSubmit={handleSubmit}>
          <label htmlFor="charName">Character Name:</label>
          <input
            type="text"
            id="charName"
            name="charName"
            value={charName}
            onChange={handleChange}
          />
          <label>Roll Type:</label>
          <div>
            <input
              type="radio"
              id="4D6D1"
              name="rollType"
              value="4D6D1"
              onChange={handleChange}
            />
            <label htmlFor="4D6D1">4D6D1</label>
          </div>
          <div>
            <input
              type="radio"
              id="3D6"
              name="rollType"
              value="3D6"
              onChange={handleChange}
            />
            <label htmlFor="3D6">3D6</label>
          </div>
          <div>
            <input
              type="radio"
              id="2D66"
              name="rollType"
              value="2D66"
              onChange={handleChange}
            />
            <label htmlFor="2D66">2D6 + 6</label>
          </div>
          <button type="button">Roll for Attributes</button>
          <label>Attributes:</label>
          <table className="statRoll">
            <thead>
              <tr>
                <th>Strength</th>
                <th>Dexterity</th>
                <th>Constitution</th>
                <th>Intelligence</th>
                <th>Wisdom</th>
                <th>Charisma</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                {Object.keys(attributes).map((key, index) => (
                  <td key={index}>
                    <div
                      className="dropbox"
                      onDrop={droppoint}
                      onDragOver={allowDropOption}
                    ></div>
                  </td>
                ))}
              </tr>
              <tr>
                {Object.keys(attributes).map((key, index) => (
                  <td key={index}>
                    <p
                      id={key}
                      draggable="true"
                      onDragStart={dragpoint}
                    >
                      {key}
                    </p>
                  </td>
                ))}
              </tr>
            </tbody>
          </table>
          <label>Class</label>
          <div>
            <input
              type="radio"
              id="class1"
              name="class"
              value="class1"
              onChange={handleChange}
            />
            <label htmlFor="class1">Class 1</label>
          </div>
          <button type="submit">Create Character</button>
        </form>
      </div>
      <div className="right"></div>
      <div className="footer">
        <p>&copy; 2024 Team 4</p>
      </div>
    </div>
  );
};

export default CreatePlayer;
