const express = require('express');
const pool = require('../db');

const router = express.Router();

// GET all books with live availability state.
router.get('/', async (req, res, next) => {
  try {
    const [rows] = await pool.query(
      `SELECT b.id, b.title, b.author, b.year,
              CASE
                WHEN EXISTS (
                  SELECT 1 FROM borrowings br
                  WHERE br.book_id = b.id AND br.return_date IS NULL
                ) THEN 'Borrowed'
                ELSE 'Available'
              END AS status
       FROM books b
       ORDER BY b.id DESC`
    );

    res.json(rows);
  } catch (error) {
    next(error);
  }
});

router.post('/', async (req, res, next) => {
  try {
    const { title, author, year } = req.body;

    if (!title?.trim() || !author?.trim() || !Number.isInteger(Number(year))) {
      return res.status(400).json({ message: 'Title, author and valid year are required.' });
    }

    const [result] = await pool.query(
      'INSERT INTO books (title, author, year) VALUES (?, ?, ?)',
      [title.trim(), author.trim(), Number(year)]
    );

    res.status(201).json({ id: result.insertId, message: 'Book created successfully.' });
  } catch (error) {
    next(error);
  }
});

router.put('/:id', async (req, res, next) => {
  try {
    const { id } = req.params;
    const { title, author, year } = req.body;

    if (!title?.trim() || !author?.trim() || !Number.isInteger(Number(year))) {
      return res.status(400).json({ message: 'Title, author and valid year are required.' });
    }

    const [result] = await pool.query(
      'UPDATE books SET title = ?, author = ?, year = ? WHERE id = ?',
      [title.trim(), author.trim(), Number(year), id]
    );

    if (result.affectedRows === 0) {
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

    const [activeBorrow] = await pool.query(
      'SELECT id FROM borrowings WHERE book_id = ? AND return_date IS NULL LIMIT 1',
      [id]
    );

    if (activeBorrow.length > 0) {
      return res.status(400).json({ message: 'Cannot delete: book is currently borrowed.' });
    }

    const [result] = await pool.query('DELETE FROM books WHERE id = ?', [id]);

    if (result.affectedRows === 0) {
      return res.status(404).json({ message: 'Book not found.' });
    }

    res.json({ message: 'Book deleted successfully.' });
  } catch (error) {
    next(error);
  }
});

module.exports = router;
