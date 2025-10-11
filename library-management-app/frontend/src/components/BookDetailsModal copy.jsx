import React from 'react';
import './GeneralBookStyle.css';

const BookDetailsModal = ({ book, onClose }) => {
  if (!book) return null;

  return (
    <div className="modal-overlay">
      <div className="modal">
        <h3>📖 Books Details</h3>
        <div className="form-group">
          <label>Title:</label>
          <input type="text" value={book.title} readOnly />
        </div>
        <div className="form-group">
          <label>Author:</label>
          <input type="text" value={book.author} readOnly />
        </div>
        <div className="form-group">
          <label>Genre:</label>
          <input type="text" value={book.genre} readOnly />
        </div>
        <div className="form-group">
          <label>Published Date:</label>
          <input type="text" value={book.publishedDate} readOnly />
        </div>
        <div className="form-group">
          <label>Description:</label>
          <textarea value={book.description} readOnly></textarea>
        </div>
        <div className="form-group">
          <label>Is Available:</label>
          <input type="checkbox" checked={book.isAvailable} readOnly />
        </div>
        <button onClick={onClose}>Fechar</button>
      </div>
    </div>
  );
};

export default BookDetailsModal;
