require('dotenv').config();

const express = require('express');
const path = require('path');
const pool = require('./db');

const booksRoutes = require('./routes/books');
const studentsRoutes = require('./routes/students');
const borrowingsRoutes = require('./routes/borrowings');

const app = express();
const PORT = Number(process.env.PORT || 5000);

app.use(express.json());
app.use(express.static(path.join(__dirname, '..', 'public')));

app.get('/health', async (req, res) => {
  try {
    await pool.query('SELECT 1');
    res.json({ status: 'ok', database: 'connected' });
  } catch (error) {
    res.status(500).json({ status: 'error', database: 'disconnected', error: error.message });
  }
});

app.use('/books', booksRoutes);
app.use('/students', studentsRoutes);
app.use('/borrowings', borrowingsRoutes);

app.use((err, req, res, next) => {
  // Centralized error handler for uncaught route errors.
  console.error(err);
  res.status(500).json({ message: 'Internal server error', error: err.message });
});

app.listen(PORT, () => {
  console.log(`Library app running at http://localhost:${PORT}`);
});
