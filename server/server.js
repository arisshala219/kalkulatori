const path = require('path');
const express = require('express');

const { initializeDatabase } = require('./db');
const booksRoutes = require('./routes/books');
const studentsRoutes = require('./routes/students');
const borrowingsRoutes = require('./routes/borrowings');

const app = express();
const PORT = Number(process.env.PORT || 5000);

app.use(express.json());
app.use(express.static(path.join(__dirname, '..', 'public')));

app.get('/health', async (req, res) => {
  try {
    await initializeDatabase();
    res.json({ status: 'ok', database: 'connected' });
  } catch (error) {
    res.status(500).json({ status: 'error', database: 'disconnected', error: error.message });
  }
});

app.use('/books', booksRoutes);
app.use('/students', studentsRoutes);
app.use('/borrowings', borrowingsRoutes);

app.use((error, req, res, next) => {
  console.error(error);
  res.status(500).json({ message: 'Internal server error', error: error.message });
});

initializeDatabase()
  .then(() => {
    app.listen(PORT, () => {
      console.log(`Library app running at http://localhost:${PORT}`);
    });
  })
  .catch((error) => {
    console.error('Failed to initialize database:', error);
    process.exit(1);
  });
