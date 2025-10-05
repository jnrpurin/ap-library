import axios from 'axios';

const API_URL = 'http://localhost:5000/api/books';

export const getBooks = async () => {
    try {
        const response = await axios.get(API_URL);
        return response.data;
    } catch (error) {
        console.error('Error fetching books:', error);
        throw error;
    }
};

export const getBookById = async (id) => {
    try {
        const response = await axios.get(`${API_URL}/${id}`);
        return response.data;
    } catch (error) {
        console.error(`Error fetching book with id ${id}:`, error);
        throw error;
    }
};

export const createBook = async (book) => {
    try {
        const response = await axios.post(API_URL, book);
        return response.data;
    } catch (error) {
        console.error('Error creating book:', error);
        throw error;
    }
};

export const updateBook = async (id, book) => {
    try {
        const response = await axios.put(`${API_URL}/${id}`, book);
        return response.data;
    } catch (error) {
        console.error(`Error updating book with id ${id}:`, error);
        throw error;
    }
};

export const deleteBook = async (id) => {
    try {
        await axios.delete(`${API_URL}/${id}`);
    } catch (error) {
        console.error(`Error deleting book with id ${id}:`, error);
        throw error;
    }
};