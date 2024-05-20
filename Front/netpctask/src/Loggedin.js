import React, { useEffect, useState } from 'react';
import { Navigate, useNavigate, Link } from 'react-router-dom'; // Dodano Link
import axios from 'axios';
import './UserProfile.css';
import { useParams } from 'react-router-dom';
import UpdateContactForm from './UpdateContactForm';

const UserProfile = () => {
  const [user, setUser] = useState(null);
  const [redirect, setRedirect] = useState(false);
  const navigate = useNavigate();
  const [contacts, setContacts] = useState([]);

  useEffect(() => {
    // Get account informations
    const fetchUserData = async () => {
      try {
        const response = await axios.get('http://localhost:5288/api/Account/user', {
          withCredentials: true 
        });
        setUser(response.data); 
      } catch (error) {
        console.error('Error:', error);
        setRedirect(true); 
      }
    };

    // List contacts
    const fetchContacts = async () => {
      try {
        const response = await axios.get('http://localhost:5288/api/Contact');
        setContacts(response.data);
      } catch (error) {
        console.error('Error fetching contacts:', error);
      }
    };

    fetchContacts();
    fetchUserData();
  }, []);

  if (redirect) {
    return <Navigate to="/home" />; 
  }

  if (!user) {
    return <div>Loading...</div>;
  }

  const handleContactClick = (contactId) => {
    navigate(`/contact/${contactId}`);
  };

  const handleUpdateContact = (contactId) => {
    navigate(`/update-contact/${contactId}`);
  };

  const handleDeleteContact = async (contactId) => {
    try {
      await axios.delete(`http://localhost:5288/api/Contact/${contactId}`);
      setContacts(contacts.filter(contact => contact.id !== contactId));
    } catch (error) {
      console.error('Error deleting contact:', error);
    }
  };

  return (
    <div>
      <h1>Welcome, {user.name}!</h1>
      <p><b>Email: {user.email}</b></p>
      <div className="home-container">
        <h2>Admin panel</h2>
        <Link to="/add-contact">Add Contact</Link> <br/>
        <h3>Contacts:</h3>
        <ul>
          {contacts.map(contact => (
            <li key={contact.id} className="contact-item">
              <span onClick={() => handleContactClick(contact.id)} className="contact-name">
                <b>{contact.name} {contact.surname}</b>
              </span>
              <div className="button-container">
                <button onClick={() => handleUpdateContact(contact.id)} className="update-button">Update</button>
              </div>
              <div className="button-container">
                <button onClick={() => handleDeleteContact(contact.id)} className="delete-button">Delete</button>
              </div>
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
};

export default UserProfile;
