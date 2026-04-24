const express = require('express');
const { connectDb } = require('../db');

const router = express.Router();

const isValidId = (id) => Number.isInteger(Number(id)) && Number(id) > 0;
const isValidYear = (year) => Number.isInteger(Number(year)) && Number(year) > 0;

router.get('/', async (req, res, next) => {
  try {
    const db = await connectDb();
    const books = await db.all(
      `SELECT b.id, b.title, b.author, b.year,
        CASE WHEN EXISTS (
          SELECT 1 FROM borrowings br
          WHERE br.book_id = b.id AND br.return_date IS NULL
        ) THEN 1 ELSE 0 END AS is_borrowed
      FROM books b
      ORDER BY b.id DESC`
    );

    res.json(books);
  } catch (error) {
    next(error);
  }
});

router.post('/', async (req, res, next) => {
  try {
    const { title, author, year } = req.body;

    if (!title?.trim() || !author?.trim() || !isValidYear(year)) {
      return res.status(400).json({ message: 'Title, author and a valid year are required.' });
    }

    const db = await connectDb();
    const result = await db.run('INSERT INTO books (title, author, year) VALUES (?, ?, ?)', [
      title.trim(),
      author.trim(),
      Number(year),
    ]);

    res.status(201).json({ id: result.lastID, message: 'Book added successfully.' });
  } catch (error) {
    next(error);
  }
});

router.put('/:id', async (req, res, next) => {
  try {
    const { id } = req.params;
    const { title, author, year } = req.body;

    if (!isValidId(id)) {
      return res.status(400).json({ message: 'Invalid book id.' });
    }

    if (!title?.trim() || !author?.trim() || !isValidYear(year)) {
      return res.status(400).json({ message: 'Title, author and a valid year are required.' });
    }

    const db = await connectDb();
    const result = await db.run('UPDATE books SET title = ?, author = ?, year = ? WHERE id = ?', [
      title.trim(),
      author.trim(),
      Number(year),
      Number(id),
    ]);

    if (result.changes === 0) {
      return res.status(404).json({ message: 'Book not found.' });
    }

    res.json({ message: 'Book updated successfully.' });
  } catch (error) {
    next(error);
  }
});

router.delete('/:id', async (req, res, next) => {
  try {
    const { id } = req.params;

    if (!isValidId(id)) {
      return res.status(400).json({ message: 'Invalid book id.' });
    }

    const db = await connectDb();
    const activeBorrow = await db.get(
      'SELECT id FROM borrowings WHERE book_id = ? AND return_date IS NULL',
      [Number(id)]
    );

    if (activeBorrow) {
      return res.status(400).json({ message: 'Cannot delete a currently borrowed book.' });
    }

    const borrowingHistory = await db.get('SELECT id FROM borrowings WHERE book_id = ?', [Number(id)]);
    if (borrowingHistory) {
      return res.status(400).json({ message: 'Cannot delete book with borrowing history.' });
    }

    const result = await db.run('DELETE FROM books WHERE id = ?', [Number(id)]);

    if (result.changes === 0) {
      return res.status(404).json({ message: 'Book not found.' });
    }

    res.json({ message: 'Book deleted successfully.' });
  } catch (error) {
    next(error);
  }
});

module.exports = router;
