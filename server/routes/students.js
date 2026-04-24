const express = require('express');
const { connectDb } = require('../db');

const router = express.Router();

const isValidId = (id) => Number.isInteger(Number(id)) && Number(id) > 0;

router.get('/', async (req, res, next) => {
  try {
    const db = await connectDb();
    const students = await db.all('SELECT id, name, class FROM students ORDER BY id DESC');
    res.json(students);
  } catch (error) {
    next(error);
  }
});

router.post('/', async (req, res, next) => {
  try {
    const { name, class: className } = req.body;

    if (!name?.trim() || !className?.trim()) {
      return res.status(400).json({ message: 'Name and class are required.' });
    }

    const db = await connectDb();
    const result = await db.run('INSERT INTO students (name, class) VALUES (?, ?)', [
      name.trim(),
      className.trim(),
    ]);

    res.status(201).json({ id: result.lastID, message: 'Student added successfully.' });
  } catch (error) {
    next(error);
  }
});

router.put('/:id', async (req, res, next) => {
  try {
    const { id } = req.params;
    const { name, class: className } = req.body;

    if (!isValidId(id)) {
      return res.status(400).json({ message: 'Invalid student id.' });
    }

    if (!name?.trim() || !className?.trim()) {
      return res.status(400).json({ message: 'Name and class are required.' });
    }

    const db = await connectDb();
    const result = await db.run('UPDATE students SET name = ?, class = ? WHERE id = ?', [
      name.trim(),
      className.trim(),
      Number(id),
    ]);

    if (result.changes === 0) {
      return res.status(404).json({ message: 'Student not found.' });
    }

    res.json({ message: 'Student updated successfully.' });
  } catch (error) {
    next(error);
  }
});

router.delete('/:id', async (req, res, next) => {
  try {
    const { id } = req.params;

    if (!isValidId(id)) {
      return res.status(400).json({ message: 'Invalid student id.' });
    }

    const db = await connectDb();
    const activeBorrow = await db.get(
      'SELECT id FROM borrowings WHERE student_id = ? AND return_date IS NULL',
      [Number(id)]
    );

    if (activeBorrow) {
      return res.status(400).json({ message: 'Cannot delete student with active borrowing.' });
    }

    const borrowingHistory = await db.get('SELECT id FROM borrowings WHERE student_id = ?', [Number(id)]);
    if (borrowingHistory) {
      return res.status(400).json({ message: 'Cannot delete student with borrowing history.' });
    }

    const result = await db.run('DELETE FROM students WHERE id = ?', [Number(id)]);

    if (result.changes === 0) {
      return res.status(404).json({ message: 'Student not found.' });
    }

    res.json({ message: 'Student deleted successfully.' });
  } catch (error) {
    next(error);
  }
});

module.exports = router;
