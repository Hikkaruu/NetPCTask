import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useParams } from 'react-router-dom'; 
import './UpdateContactForm.css'; 
import { checkPasswordComplexity } from './CheckPass';

const UpdateContactForm = () => {
  const { id } = useParams(); 

  const [formData, setFormData] = useState({
    name: '',
    surname: '',
    email: '',
    password: '',
    phone: '',
    birth_date: '',
    categoryId: '',
    subCategoryId: ''
  });

  const [categories, setCategories] = useState([]);
  const [subcategories, setSubcategories] = useState([]);
  const [isSubcategoryDisabled, setIsSubcategoryDisabled] = useState(false);
  const [isOtherCategory, setIsOtherCategory] = useState(false);

  // Get categories to fill the drop-down list
  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const response = await axios.get('http://localhost:5288/api/Category');
        setCategories(response.data);
      } catch (error) {
        console.error('Error fetching categories:', error);
      }
    };

    fetchCategories();
  }, []);

  // If category is "służbowy" then the associated subcategories are loaded into drop-down list
  // If category is "prywatny" then subcategory field is disabled
  // If category is "inny" then subcategory field becomes text field which we can fill with custom value
  useEffect(() => {
    if (formData.categoryId) {
      if (formData.categoryId === "2") {
        setFormData({
          ...formData,
          subCategoryId: '8'
        });
        setIsSubcategoryDisabled(true);
        setIsOtherCategory(false);
      } else if (formData.categoryId === "3") {
        setIsOtherCategory(true);
        setIsSubcategoryDisabled(true);
        setFormData({
          ...formData,
          subCategoryId: ''
        });
      } else {
        setIsOtherCategory(false);
        setIsSubcategoryDisabled(false);
        const fetchSubcategories = async () => {
        try {
          const response = await axios.get(`http://localhost:5288/api/Subcategory/category/${formData.categoryId}`);
          setSubcategories(response.data);
        } catch (error) {
          console.error('Error fetching subcategories:', error);
        }
      };

      fetchSubcategories();
      }
    } else {
      setSubcategories([]); 
      setIsSubcategoryDisabled(false);
      setIsOtherCategory(false);
    }
  }, [formData.categoryId]);

  // method changing name when user fills name field in form
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value
    });
  };

  // method adds new contact to the database 
  const handleSubmit = async (e) => {
    e.preventDefault();

    const complexityMessage = checkPasswordComplexity(formData.password);

    if (complexityMessage !== 'Password is complex enough.') {
      alert(complexityMessage);
      return;
    }

    try {
      let subCategoryId = formData.subCategoryId;
      
      if (isOtherCategory && formData.subCategoryId) {
        try {
          // get all subcategories associated with category id=3 ("inny")
          const response = await axios.get('http://localhost:5288/api/Subcategory/category/3');
          const existingSubcategory = response.data.find(subcategory => subcategory.name.toLowerCase() === formData.subCategoryId.toLowerCase());

          if (existingSubcategory) {
            subCategoryId = existingSubcategory.id;
          } else {
            // create subcategory with name filled in form field if it doesn't already exist in database
            const subcategoryResponse = await axios.post('http://localhost:5288/api/Subcategory', {
              name: formData.subCategoryId,
              categoryId: 3
            });
            subCategoryId = subcategoryResponse.data.id;
          }
        } catch (error) {
          console.error('Error adding or fetching subcategory:', error.response ? error.response.data : error.message);
          alert('Error adding or fetching subcategory: ' + (error.response ? error.response.data.message : error.message));
          return;
        }
      }

      var updatedFormData = {
        ...formData,
        subCategoryId: subCategoryId
      };

      if (isOtherCategory){
        // if category is "inny", get subcategory by it's name
        const respon = await axios.get(`http://localhost:5288/api/Subcategory/name/${formData.subCategoryId}`);

        updatedFormData = {
          ...formData,
          subCategoryId: respon.data.id
        }
      }

      // update Contact in database
      await axios.put(`http://localhost:5288/api/Contact/${id}`, updatedFormData); 

      alert('Contact updated successfully!');
      setFormData({
        name: '',
        surname: '',
        email: '',
        password: '',
        phone: '',
        birth_date: '',
        categoryId: '',
        subCategoryId: ''
      });
    } catch (error) {
      console.error('Error updating contact:', error);
    }
  };


  return (
    <div className="update-contact-form">
     <h2>Update Contact</h2>
     <form onSubmit={handleSubmit} className="update-contact-form">
      <div className="form-group">
        <label htmlFor="name">Name:</label>
        <input type="text" id="name" name="name" value={formData.name} onChange={handleChange} />
      </div>
      <div className="form-group">
        <label htmlFor="surname">Surname:</label>
        <input type="text" id="surname" name="surname" value={formData.surname} onChange={handleChange} />
      </div>
      <div className="form-group">
        <label htmlFor="email">Email:</label>
        <input type="email" id="email" name="email" value={formData.email} onChange={handleChange} />
      </div>
      <div className="form-group">
        <label htmlFor="password">Password:</label>
        <input type="password" id="password" name="password" value={formData.password} onChange={handleChange} />
      </div>
      <div className="form-group">
        <label htmlFor="phone">Phone:</label>
        <input type="text" id="phone" name="phone" value={formData.phone} onChange={handleChange} />
      </div>
      <div className="form-group">
        <label htmlFor="birth_date">Birth Date:</label>
        <input type="date" id="birth_date" name="birth_date" value={formData.birth_date} onChange={handleChange} />
      </div>
      <div className="form-group">
        <label htmlFor="categoryId">Category:</label>
        <select id="categoryId" name="categoryId" value={formData.categoryId} onChange={handleChange}>
          <option value="">Select Category</option>
          {categories.map(category => (
            <option key={category.id} value={category.id}>{category.name}</option>
          ))}
        </select>
      </div>
      <div className="form-group">
        <label htmlFor="subCategoryId">Subcategory:</label>
        {isOtherCategory ? (
          <input type="text" id="subCategoryId" name="subCategoryId" value={formData.subCategoryId} onChange={handleChange} />
        ) : (
          <select id="subCategoryId" name="subCategoryId" value={formData.subCategoryId} onChange={handleChange} disabled={isSubcategoryDisabled}>
            <option value="">Select Subcategory</option>
            {subcategories.map(subcategory => (
              <option key={subcategory.id} value={subcategory.id}>{subcategory.name}</option>
            ))}
          </select>
        )}
      </div>
      <button type="submit">Update Contact</button>
    </form>
    </div>
  );
};

export default UpdateContactForm;
