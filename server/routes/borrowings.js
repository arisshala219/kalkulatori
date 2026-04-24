const express = require('express');
const pool = require('../db');

const router = express.Router();
const isValidId = (value) => Number.isInteger(Number(value)) && Number(value) > 0;
const isValidDate = (value) => /^\d{4}-\d{2}-\d{2}$/.test(value);

router.get('/', async (req, res, next) => {
  try {
    const [rows] = await pool.query(
      `SELECT br.id, br.book_id, b.title AS book_title, br.student_id, s.name AS student_name,
              br.borrow_date, br.return_date
       FROM borrowings br
       INNER JOIN books b ON b.id = br.book_id
       INNER JOIN students s ON s.id = br.student_id
       ORDER BY br.id DESC`
    );

    res.json(rows);
  } catch (error) {
    next(error);
  }
});

router.post('/', async (req, res, next) => {
  try {
    const { book_id, student_id, borrow_date } = req.body;

    if (!isValidId(book_id) || !isValidId(student_id)) {
      return res.status(400).json({ message: 'Valid book and student are required.' });
    }

    if (borrow_date && !isValidDate(borrow_date)) {
      return res.status(400).json({ message: 'Borrow date must use YYYY-MM-DD format.' });
    }

    const [[book]] = await pool.query('SELECT id FROM books WHERE id = ?', [Number(book_id)]);
    if (!book) return res.status(404).json({ message: 'Book not found.' });

    const [[student]] = await pool.query('SELECT id FROM students WHERE id = ?', [Number(student_id)]);
    if (!student) return res.status(404).json({ message: 'Student not found.' });

    const [activeBorrow] = await pool.query(
      'SELECT id FROM borrowings WHERE book_id = ? AND return_date IS NULL LIMIT 1',
      [Number(book_id)]
    );

    if (activeBorrow.length > 0) {
      return res.status(400).json({ message: 'Book is already borrowed.' });
    }

    const safeBorrowDate = borrow_date || new Date().toISOString().slice(0, 10);

    const [result] = await pool.query(
      'INSERT INTO borrowings (book_id, student_id, borrow_date) VALUES (?, ?, ?)',
      [Number(book_id), Number(student_id), safeBorrowDate]
    );

    res.status(201).json({ id: result.insertId, message: 'Borrowing registered successfully.' });
  } catch (error) {
    next(error);
  }
});

// Return a book by setting the return date.
router.put('/:id', async (req, res, next) => {
  try {
    const { id } = req.params;
    const { return_date } = req.body;

    if (!isValidId(id)) {
      return res.status(400).json({ message: 'Invalid borrowing id.' });
    }

    if (return_date && !isValidDate(return_date)) {
      return res.status(400).json({ message: 'Return date must use YYYY-MM-DD format.' });
    }

    const safeReturnDate = return_date || new Date().toISOString().slice(0, 10);

    const [[borrowing]] = await pool.query('SELECT id, return_date FROM borrowings WHERE id = ?', [id]);
    if (!borrowing) return res.status(404).json({ message: 'Borrowing record not found.' });

    if (borrowing.return_date) {
      return res.status(400).json({ message: 'Book has already been returned.' });
    }

    await pool.query('UPDATE borrowings SET return_date = ? WHERE id = ?', [safeReturnDate, id]);

    res.json({ message: 'Book returned successfully.' });
  } catch (error) {
    next(error);
  }
});

module.exports = router;
