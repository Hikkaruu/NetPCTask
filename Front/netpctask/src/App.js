import React, { useEffect, useState } from 'react';
import { NavLink, Routes, Route, Navigate, useNavigate } from 'react-router-dom';
import './App.css';
import './Login.css';
import Loggedin from './Loggedin';
import Login from './Login';
import Register from './Register';
import Home from './Home';
import axios from 'axios';
import ContactDetails from './ContactDetails';
import UpdateContactForm from './UpdateContactForm';
import AddContact from './AddContact';

function App() {
  const [user, setUser] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    // Checking if user is logged in
    const checkUser = async () => {
      try {
        const response = await axios.get('http://localhost:5288/api/Account/user', {
          withCredentials: true
        });
        if (response.status === 200) {
          setUser(response.data); 
        }
      } catch (error) {
        if (error.response && error.response.status === 401) {
          setUser(null); 
        } else {
          console.error('Error checking user status:', error);
        }
      }
    };

    checkUser();
  }, []);

  const userLogout = async () => {
    try {
      // Logout user
      await axios.post('http://localhost:5288/api/Account/logout', {}, {
        withCredentials: true
      });
      setUser(null); 
      navigate('/home'); 
    } catch (error) {
      console.error('Error logging out:', error);
    }
    window.location.reload(); 
  };



  return (
    <div className="App">
      <nav className="navbar">
        <ul>
          {user ? (
            <li>
            <NavLink to="/panel">Home</NavLink>
            </li>
            ) : (
            <li>
            <NavLink to="/home">Home</NavLink>
            </li>
            )}
          {user ? (
            <li>
              <NavLink to="/home" onClick={userLogout}>Logout</NavLink>
            </li>
          ) : (
            <>
              <li>
                <NavLink to="/login">Login</NavLink>
              </li>
              <li>
                <NavLink to="/register">Register</NavLink>
              </li>
            </>
          )}
        </ul>
      </nav>
      <Routes>
        <Route path="/home" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/panel" element={<Loggedin />} />
        <Route path="/" element={<Navigate to="/home" />} />
        <Route path="/contact/:id" element={<ContactDetails />} />
        <Route path="/update-contact/:id" element={user ? <UpdateContactForm /> : <Home />} />
        <Route path="add-contact" element={user ? <AddContact /> : <Home />} /> 
      </Routes>
    </div>
  );
}

export default App;
