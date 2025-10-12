import axios from 'axios';

const BASE_URL = 'https://localhost:7100/api/v1';
const API_BOOKS_URL = `${BASE_URL}/Books`;
const API_AUTH_URL = `${BASE_URL}/Login/authenticate`;

axios.interceptors.request.use(config => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers.Authorization = `${token}`;
    }
    return config;
}, error => {
    return Promise.reject(error);
});

export const loginUser = async (username, password) => {
    try {
        const response = await axios.post(API_AUTH_URL, { username, password });
        
        localStorage.setItem('token', response.data.token);
        localStorage.setItem('userName', response.data.userName);
        localStorage.setItem('roles', JSON.stringify(response.data.roles));
        return response.data; 
    } catch (error) {
        console.error('Error during login:', error);
        throw error.response?.data?.Message || 'Autentication Error.';
    }
};

export const getBooks = async () => {
    try {
        const response = await axios.get(API_BOOKS_URL);
        return response.data;
    } catch (error) {
        console.error('Error fetching books:', error);
        throw error;
    }
};


export const getBookById = async (id) => {
    try {
        const response = await axios.get(`${API_BOOKS_URL}/${id}`);
        return response.data;
    } catch (error) {
        console.error(`Error fetching book with id ${id}:`, error);
        throw error;
    }
};

export const createBook = async (book) => {
    try {
        const response = await axios.post(API_BOOKS_URL, book);
        return response.data;
    } catch (error) {
        console.error('Error creating book:', error);
        throw error;
    }
};

export const updateBook = async (id, book) => {
    try {
        const response = await axios.put(`${API_BOOKS_URL}/${id}`, book);
        return response.data;
    } catch (error) {
        console.error(`Error updating book with id ${id}:`, error);
        throw error;
    }
};

export const deleteBook = async (id) => {
    try {
        await axios.delete(`${API_BOOKS_URL}/${id}`);
    } catch (error) {
        console.error(`Error deleting book with id ${id}:`, error);
        throw error;
    }
};


export const logoutUser = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('userName');
    localStorage.removeItem('roles');    
};