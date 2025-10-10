import React from 'react';
import BookList from './components/BookList';

const App = () => {
    return (
        <div className="app-container">
          <header className="app-header">
            <h1>📚 AP Library</h1>
          </header>
          <main>
            <BookList />
          </main>
        </div>
      );
};

export default App;