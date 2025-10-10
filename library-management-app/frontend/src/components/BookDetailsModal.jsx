import React from 'react';
import './GeneralBookStyle.css';

const BookDetailsModal = ({ book, onClose }) => {
  if (!book) return null;

  return (
    <div className="modal-overlay">
      <div className="modal">
        <h2>📖 Book Details</h2>

        <div className="form-group">
          <label>Title:</label>
          <p>{book.title}</p>
        </div>

        <div className="form-group">
          <label>Author:</label>
          <p>{book.author}</p>
        </div>

        <div className="form-group">
          <label>Genre:</label>
          <p>{book.genre}</p>
        </div>

        <div className="form-group">
          <label>Published Date:</label>
          <p>{new Date(book.publishedDate).toLocaleDateString()}</p>
        </div>

        <div className="form-group description">
          <label>Description:</label>
          <p>{book.description}</p>
        </div>

        <div className="form-group">
          <label>Is Available:</label>
          <p>{book.isAvailable ? "✅ Available" : "❌ Not Available"}</p>
        </div>

        <div className="modal-buttons">
          <button onClick={onClose}>Close</button>
        </div>
      </div>
    </div>
  );
};

export default BookDetailsModal;
