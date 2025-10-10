import React, { useState } from 'react';
import './AddBookModal.css';

const AddBookModal = ({ onClose, onSave }) => {
  const [book, setBook] = useState({
    title: '',
    author: '',
    genre: '',
    publishedDate: '',
    description: '',
    isAvailable: true
  });

  const handleSave = () => {
    onSave(book);
  };

  return (
    <div className="modal-overlay">
      <div className="modal">
        <h3>Adicionar Novo Livro</h3>
        <label>
          Title:
          <input
            type="text"
            value={book.title}
            onChange={e => setBook({ ...book, title: e.target.value })}
          />
        </label>
        <label>
          Author:
          <input
            type="text"
            value={book.author}
            onChange={e => setBook({ ...book, author: e.target.value })}
          />
        </label>
        <label>
          Geneder:
          <input
            type="text"
            value={book.genre}
            onChange={e => setBook({ ...book, genre: e.target.value })}
          />
        </label>
        <label>
        Published date:
          <input
            type="date"
            value={book.publishedDate}
            onChange={e => setBook({ ...book, publishedDate: e.target.value })}
          />
        </label>
        <label>
          Description:
          <textarea
            value={book.description}
            onChange={e => setBook({ ...book, description: e.target.value })}
          />
        </label>
        <div className="modal-buttons">
          <button onClick={handleSave}>Save</button>
          <button onClick={onClose}>Cancel</button>
        </div>
      </div>
    </div>
  );
};

export default AddBookModal;
