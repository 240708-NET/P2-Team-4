import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../DungeonStyle.css';

const Login = ({ onLogin }) => {
  const [userDetails, setUserDetails] = useState({ name: '' });
  const [showCreateUser, setShowCreateUser] = useState(true);
  const navigate = useNavigate();

  const handleChange = (e) => {
    setUserDetails({ ...userDetails, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      let response = await fetch(`http://localhost:5201/getUserByName/${userDetails.name}`);
      if (response.ok) {
        const text = await response.text();
        const data = text ? JSON.parse(text) : {}; 
        
        if (data && data.name === userDetails.name) {
          alert('Welcome back!');
          onLogin(data.id);
          navigate(`/select/${data.id}`); 
        } else {
          response = await fetch(`http://localhost:5201/createUser/${userDetails.name}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
          });
  
          if (response.ok) {
            const text = await response.text();
            const createUserData = text ? JSON.parse(text) : {}; 
            
            if (createUserData && createUserData.id) {
              alert('User created successfully!');
              onLogin(createUserData.id);
              navigate(`/select/${createUserData.id}`); // Navigate to SelectPlayer
            } else {
              console.error('User creation response is not in expected format:', createUserData);
              alert('Failed to create user.');
            }
          } else {
            console.error('Failed to create user:', response.statusText);
            alert('Failed to create user.');
          }
        }
      } else {
        console.error('Failed to fetch user:', response.statusText);
        alert('Failed to fetch user.');
      }
    } catch (error) {
      console.error('Error handling user login/creation:', error);
      alert('An error occurred while handling user login/creation.');
    }
  };
  
  return (
    <div className="login-container">
      <h2>{showCreateUser ? 'Create User' : 'Login'}</h2>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label>Username:</label>
          <input
            type="text"
            name="name"
            value={userDetails.name}
            onChange={handleChange}
            required
            className="form-control"
          />
        </div>
        <button type="submit" className="btn-login">
          {showCreateUser ? 'Create User' : 'Login'}
        </button>
        {!showCreateUser && (
          <button
            type="button"
            className="btn-switch"
            onClick={() => setShowCreateUser(true)}
          >
            Create New User
          </button>
        )}
        {showCreateUser && (
          <button
            type="button"
            className="btn-switch"
            onClick={() => setShowCreateUser(false)}
          >
            Login with Existing User
          </button>
        )}
      </form>
    </div>
  );
};

export default Login;
