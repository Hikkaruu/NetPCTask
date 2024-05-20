import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import './Home.css';

const Home = () => {
  const navigate = useNavigate();
  const [contacts, setContacts] = useState([]);

  useEffect(() => {
    const fetchContacts = async () => {
      try {
        const response = await axios.get('http://localhost:5288/api/Contact');
        setContacts(response.data);
      } catch (error) {
        console.error('Error fetching contacts:', error);
      }
    };

    fetchContacts();
  }, []);

  const handleContactClick = (contactId) => {
    navigate(`/contact/${contactId}`);
  };

  return (
    <div className="home-container">
      <h2>Home Page</h2>
      <p>Welcome! Please login to unlock more options</p>
      <h3>Contacts:</h3>
      <ul>
        {contacts.map(contact => (
          <li key={contact.id} className="contact-button" onClick={() => handleContactClick(contact.id)}>
            <b>{contact.name} {contact.surname}</b>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Home;
