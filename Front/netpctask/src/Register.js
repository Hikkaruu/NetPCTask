import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from "react-router-dom";
import { checkPasswordComplexity } from './CheckPass';

// Handle registration
const Register = () => {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');

    const complexityMessage = checkPasswordComplexity(password);

    if (complexityMessage !== 'Password is complex enough.') {
      alert(complexityMessage);
      return;
    }

    try {
      const response = await axios.post('http://localhost:5288/api/Account/register', {
        name,
        email,
        password,
      },
      {
        withCredentials: true
      });

      if (response.status === 201) {
        console.log('Registered successfully');
        navigate("/login");
      }
    } catch (err) {
      setError('Registration failed. Please try again.');
    }
  };

  return (
    <div className="login-container">
      <h2>Register</h2>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="name">Name:</label>
          <input
            type="text"
            id="name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="email">Email:</label>
          <input
            type="email"
            id="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="password">Password:</label>
          <input
            type="password"
            id="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        {error && <p style={{ color: 'red' }}>{error}</p>}
        <button type="submit">Register</button>
      </form>
    </div>
  );
};

export default Register;
