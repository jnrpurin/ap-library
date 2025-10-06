import React, { useEffect, useState } from 'react';
import { fetchBooks } from '../services/api';
import './BookList.css'; // Import the CSS file for styling

const BookList = () => {
    const [books, setBooks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const getBooks = async () => {
            try {
                const data = await fetchBooks();
                setBooks(data);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        getBooks();
    }, []);

    if (loading) return <div className="loading">Loading...</div>;
    if (error) return <div className="error">Error: {error}</div>;

    return (
        <div className="book-list-container">
            <h2 className="book-list-title">Lista de Livros</h2>
            <ul className="book-list">
                {books.map(book => (
                    <li key={book.id} className="book-item">
                        <span className="book-title">{book.title}</span> - <span className="book-author">{book.author}</span>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default BookList;