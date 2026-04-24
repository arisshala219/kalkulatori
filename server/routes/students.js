const express = require('express');
const pool = require('../db');

const router = express.Router();

router.get('/', async (req, res, next) => {
  try {
    const [rows] = await pool.query('SELECT id, name, class_name FROM students ORDER BY id DESC');
    res.json(rows);
  } catch (error) {
    next(error);
  }
});

router.post('/', async (req, res, next) => {
  try {
    const { name, class_name } = req.body;

    if (!name?.trim() || !class_name?.trim()) {
      return res.status(400).json({ message: 'Name and class are required.' });
    }

    const [result] = await pool.query('INSERT INTO students (name, class_name) VALUES (?, ?)', [
      name.trim(),
      class_name.trim(),
    ]);

    res.status(201).json({ id: result.insertId, message: 'Student created successfully.' });
  } catch (error) {
    next(error);
  }
});

router.put('/:id', async (req, res, next) => {
  try {
    const { id } = req.params;
    const { name, class_name } = req.body;

    if (!name?.trim() || !class_name?.trim()) {
      return res.status(400).json({ message: 'Name and class are required.' });
    }

    const [result] = await pool.query('UPDATE students SET name = ?, class_name = ? WHERE id = ?', [
      name.trim(),
      class_name.trim(),
      id,
    ]);

    if (result.affectedRows === 0) {
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

    const [activeBorrow] = await pool.query(
      'SELECT id FROM borrowings WHERE student_id = ? AND return_date IS NULL LIMIT 1',
      [id]
    );

    if (activeBorrow.length > 0) {
      return res.status(400).json({ message: 'Cannot delete: student has active borrowings.' });
    }

    const [result] = await pool.query('DELETE FROM students WHERE id = ?', [id]);

    if (result.affectedRows === 0) {
      return res.status(404).json({ message: 'Student not found.' });
    }

    res.json({ message: 'Student deleted successfully.' });
  } catch (error) {
    next(error);
  }
});

module.exports = router;
