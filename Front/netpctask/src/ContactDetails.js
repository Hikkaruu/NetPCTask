import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';

const ContactDetails = () => {
  const { id } = useParams();
  const [contact, setContact] = useState(null);
  const [category, setCategory] = useState(null);
  const [subcategory, setSubcategory] = useState(null);

  useEffect(() => {
    // Get all information about contact
    const fetchContact = async () => {
      try {
        const response = await axios.get(`http://localhost:5288/api/Contact/${id}`);
        setContact(response.data);

        const categoryResponse = await axios.get(`http://localhost:5288/api/Category/${response.data.categoryId}`);
        setCategory(categoryResponse.data.name);

        const subcategoryResponse = await axios.get(`http://localhost:5288/api/Subcategory/${response.data.subCategoryId}`);
        if (subcategoryResponse.data.name == "")
          setSubcategory("-")
        else
          setSubcategory(subcategoryResponse.data.name);
      } catch (error) {
        console.error('Error fetching contact details:', error);
      }
    };

    fetchContact();
  }, [id]);

  if (!contact || !category || !subcategory) {
    return <div>Data missing</div>;
  }

  return (
    <div>
      <h2>Contact Details</h2>
      <p>Name: <b>{contact.name}</b></p>
      <p>Surname: <b>{contact.surname}</b></p>
      <p>Email: <b>{contact.email}</b></p>
      <p>Password: <b>{contact.password}</b></p>
      <p>Phone: <b>{contact.phone}</b></p>
      <p>Birth Date: <b>{contact.birth_date}</b></p>
      <p>Category: <b>{category}</b></p>
      {subcategory != "-" ? (
        <p>Subcategory: <b>{subcategory}</b></p>
      ) : null}
    </div>
  );
};

export default ContactDetails;
