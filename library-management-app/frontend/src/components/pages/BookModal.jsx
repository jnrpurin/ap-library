import React, { useState, useEffect } from 'react';
import '../style/GeneralBookStyle.css';

const BookModal = ({ isOpen, onClose, onSave, onUpdate, mode = "add", book }) => {
  const [formData, setFormData] = useState({
    id: "",
    title: "",
    author: "",
    genre: "",
    publishedDate: "",
    description: "",
    isAvailable: true
  });

  useEffect(() => {
    if (mode === "edit" && book) {
      setFormData(book);
    } else if (mode === "add") {
      setFormData({
        id: "",
        title: "",
        author: "",
        genre: "",
        publishedDate: "",
        description: "",
        isAvailable: true
      });
    }
  }, [mode, book]);

  if (!isOpen) return null;

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...formData,
      [name]: type === "checkbox" ? checked : value,
    });
  };

  const handleSubmit = () => {
    if (mode === "edit") {
      onUpdate(formData);
    } else {
      onSave(formData);
    }
    onClose();
  };

  return (
    <div className="modal-overlay">
      <div className="modal">
        <h2>{mode === "edit" ? "Edit Book" : "Add New Book"}</h2>
        <label>Title:</label>
        <input name="title" value={formData.title} onChange={handleChange} />

        <label>Author:</label>
        <input name="author" value={formData.author} onChange={handleChange} />

        <label>Genre:</label>
        <input name="genre" value={formData.genre} onChange={handleChange} />

        <label>Published Date:</label>
        <input
          type="date"
          name="publishedDate"
          value={formData.publishedDate?.split("T")[0] || ""}
          onChange={handleChange}
        />

        <label>Description:</label>
        <textarea name="description" value={formData.description} onChange={handleChange} />

        <label>
          <input
            type="checkbox"
            name="isAvailable"
            checked={formData.isAvailable}
            onChange={handleChange}
          />
          Available
        </label>

        <div className="modal-buttons">
          <button onClick={handleSubmit}>
            {mode === "edit" ? "Update" : "Add"}
          </button>
          <button className="cancel-button" onClick={onClose}>Cancel</button>
        </div>
      </div>
    </div>
  );
};

export default BookModal;
