import React, { useState, useEffect } from 'react';
import BookModal from './BookModal';
import BookDetailsModal from './BookDetailsModal';
import { getBooks, deleteBook, createBook, updateBook } from '../services/api';
import './BookList.css';

const BookList = () => {
  const [books, setBooks] = useState([]);
  const [filteredBooks, setFilteredBooks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [mode, setMode] = useState('add');
  const [selectedBook, setSelectedBook] = useState(null);
  const [isDetailsOpen, setIsDetailsOpen] = useState(false);

  const fetchData = async () => {
    try {
      const data = await getBooks();
      setBooks(data);
      setFilteredBooks(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  useEffect(() => {
    const filtered = books.filter(book =>
      book.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
      book.author.toLowerCase().includes(searchTerm.toLowerCase())
    );
    setFilteredBooks(filtered);
  }, [searchTerm, books]);

  const handleViewDetails = (book) => {
    setSelectedBook(book);
    setIsDetailsOpen(true);
  };

  const handleCloseDetails = () => {
    setIsDetailsOpen(false);
    setSelectedBook(null);
  };

  const handleDelete = async (id) => {
    try {
      await deleteBook(id);
      fetchData();
    } catch (err) {
      console.error(err);
    }
  };

  const handleAddClick = () => {
    setSelectedBook(null);
    setMode('add');
    setIsModalOpen(true);
  };

  const handleEditClick = (book) => {
    setSelectedBook(book);
    setMode('edit');
    setIsModalOpen(true);
  };

  const handleSaveBook = async (bookData) => {
    try {
      if (mode === 'add') {
        const newBook = { ...bookData, id: crypto.randomUUID() };
        await createBook(newBook);
      } else if (mode === 'edit' && selectedBook) {
        await updateBook(selectedBook.id, bookData);
      }
      setIsModalOpen(false);
      fetchData();
    } catch (err) {
      console.error('Error saving book:', err);
    }
  };

  if (loading) return <div className="loading">Loading...</div>;
  if (error) return <div className="error">Error: {error}</div>;

  return (
    <div className="book-list-container">
      <div className="book-list-header">
        <h2 className="book-list-title">📚 Library Manager</h2>
        <button className="add-button" onClick={handleAddClick}>➕ Add Book</button>
      </div>

      <div className="search-bar">
        <input
          type="text"
          placeholder="🔍 Search by title or author..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
      </div>

      <ul className="book-list">
        {filteredBooks.map(book => (
          <li key={book.id} className="book-item">
            <div className="book-info">
              <span className="book-title">{book.title}</span> — <span className="book-author">{book.author}</span>
            </div>
            <div className="book-actions">
              <button className="view-button" onClick={() => handleViewDetails(book)}>👁️ View</button>
              <button className="edit-button" onClick={() => handleEditClick(book)}>✏️ Edit</button>
              <button className="delete-button" onClick={() => handleDelete(book.id)}>🗑️ Delete</button>
            </div>
          </li>
        ))}
      </ul>

      {isDetailsOpen && (
        <BookDetailsModal book={selectedBook} onClose={handleCloseDetails} />
      )}

      {isModalOpen && (
        <BookModal
          isOpen={isModalOpen}
          onClose={() => setIsModalOpen(false)}
          onSave={handleSaveBook}
          mode={mode}
          book={selectedBook}
        />
      )}
    </div>
  );
};

export default BookList;
