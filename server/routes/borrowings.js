const express = require('express');
const { connectDb } = require('../db');

const router = express.Router();

const isValidId = (id) => Number.isInteger(Number(id)) && Number(id) > 0;
const isValidDate = (value) => /^\d{4}-\d{2}-\d{2}$/.test(value);
const today = () => new Date().toISOString().slice(0, 10);

router.get('/', async (req, res, next) => {
  try {
    const db = await connectDb();
    const borrowings = await db.all(
      `SELECT br.id, br.book_id, br.student_id, br.borrow_date, br.return_date,
              b.title AS book_title, b.author AS book_author,
              s.name AS student_name, s.class AS student_class
       FROM borrowings br
       INNER JOIN books b ON b.id = br.book_id
       INNER JOIN students s ON s.id = br.student_id
       ORDER BY br.id DESC`
    );

    res.json(borrowings);
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
      return res.status(400).json({ message: 'Borrow date must be in YYYY-MM-DD format.' });
    }

    const db = await connectDb();
    const book = await db.get('SELECT id FROM books WHERE id = ?', [Number(book_id)]);
    if (!book) {
      return res.status(404).json({ message: 'Book not found.' });
    }

    const student = await db.get('SELECT id FROM students WHERE id = ?', [Number(student_id)]);
    if (!student) {
      return res.status(404).json({ message: 'Student not found.' });
    }

    const activeBorrow = await db.get(
      'SELECT id FROM borrowings WHERE book_id = ? AND return_date IS NULL',
      [Number(book_id)]
    );
    if (activeBorrow) {
      return res.status(400).json({ message: 'This book is already borrowed.' });
    }

    const result = await db.run(
      'INSERT INTO borrowings (book_id, student_id, borrow_date, return_date) VALUES (?, ?, ?, NULL)',
      [Number(book_id), Number(student_id), borrow_date || today()]
    );

    res.status(201).json({ id: result.lastID, message: 'Book borrowed successfully.' });
  } catch (error) {
    next(error);
  }
});

router.put('/:id', async (req, res, next) => {
  try {
    const { id } = req.params;
    const { return_date } = req.body;

    if (!isValidId(id)) {
      return res.status(400).json({ message: 'Invalid borrowing id.' });
    }

    if (return_date && !isValidDate(return_date)) {
      return res.status(400).json({ message: 'Return date must be in YYYY-MM-DD format.' });
    }

    const db = await connectDb();
    const existing = await db.get('SELECT id, return_date FROM borrowings WHERE id = ?', [Number(id)]);

    if (!existing) {
      return res.status(404).json({ message: 'Borrowing not found.' });
    }

    if (existing.return_date) {
      return res.status(400).json({ message: 'This borrowing has already been returned.' });
    }

    await db.run('UPDATE borrowings SET return_date = ? WHERE id = ?', [return_date || today(), Number(id)]);

    res.json({ message: 'Book returned successfully.' });
  } catch (error) {
    next(error);
  }
});

module.exports = router;
