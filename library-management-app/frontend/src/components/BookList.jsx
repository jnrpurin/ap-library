import React, { useEffect, useState } from 'react';
import AddBookModal from './AddBookModal';
import { getBooks, deleteBook, createBook } from '../services/api';
import './BookList.css';

const BookList = () => {
  const [books, setBooks] = useState([]);
  const [filteredBooks, setFilteredBooks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');

  const [showModal, setShowModal] = useState(false);
  const [newBook, setNewBook] = useState({
    title: '',
    author: '',
    genre: '',
    publishedDate: '',
    description: '',
    isAvailable: true
  });

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

  const handleDelete = async (id) => {
    try {
      await deleteBook(id);
      fetchData();
    } catch (err) {
      console.error(err);
    }
  };

  const handleAddBook = async () => {
    try {
      const bookToCreate = {
        ...newBook,
        id: crypto.randomUUID()
      };
      await createBook(bookToCreate);
      setShowModal(false);
      setNewBook({
        title: '',
        author: '',
        genre: '',
        publishedDate: '',
        description: '',
        isAvailable: true
      });
      fetchData();
    } catch (err) {
      console.error('Error saving the new book:', err);
    }
  };

  useEffect(() => {
    const filtered = books.filter(book =>
      book.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
      book.author.toLowerCase().includes(searchTerm.toLowerCase())
    );
    setFilteredBooks(filtered);
  }, [searchTerm, books]);

  useEffect(() => {
    fetchData();
  }, []);

  if (loading) return <div className="loading">Loading...</div>;
  if (error) return <div className="error">Error: {error}</div>;

  return (     
    <div className="book-list-container">
      <div className="book-list-header">
        <h2 className="book-list-title">Lista de Livros</h2>
        <button className="add-button" onClick={() => setShowModal(true)}>➕ Adicionar Livro</button>
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
        {books.map(book => (
          <li key={book.id} className="book-item">
            <div className="book-info">
              <span className="book-title">{book.title}</span> - <span className="book-author">{book.author}</span>
            </div>
            <div className="book-actions">
              <button className="edit-button">✏️ Edit</button>
              <button className="delete-button" onClick={() => handleDelete(book.id)}>🗑️ Delete</button>
            </div>
          </li>
        ))}
      </ul>

      {showModal && (
        <AddBookModal
          onClose={() => setShowModal(false)}
          onSave={async (bookData) => {
            await createBook({ ...bookData, id: crypto.randomUUID() });
            setShowModal(false);
            fetchData();
          }}
        />
      )}
    </div>
  );
};

export default BookList;
