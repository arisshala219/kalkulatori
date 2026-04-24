const api = {
  books: '/books',
  students: '/students',
  borrowings: '/borrowings',
};

const state = {
  books: [],
  students: [],
  borrowings: [],
};

const feedbackEl = document.getElementById('feedback');

const showFeedback = (message, isError = false) => {
  feedbackEl.textContent = message;
  feedbackEl.style.color = isError ? '#dc2626' : '#6b7280';
};

const request = async (url, options = {}) => {
  const response = await fetch(url, {
    headers: { 'Content-Type': 'application/json' },
    ...options,
  });

  const data = await response.json().catch(() => ({}));

  if (!response.ok) {
    throw new Error(data.message || 'Unexpected error');
  }

  return data;
};

const escapeHtml = (value) =>
  String(value)
    .replaceAll('&', '&amp;')
    .replaceAll('<', '&lt;')
    .replaceAll('>', '&gt;')
    .replaceAll('"', '&quot;')
    .replaceAll("'", '&#39;');

const switchSection = () => {
  document.querySelectorAll('.nav-btn').forEach((button) => {
    button.addEventListener('click', () => {
      document.querySelectorAll('.nav-btn').forEach((b) => b.classList.remove('active'));
      document.querySelectorAll('.panel').forEach((panel) => panel.classList.remove('active'));

      button.classList.add('active');
      const sectionId = button.dataset.section;
      document.getElementById(sectionId).classList.add('active');

      const titleMap = {
        'books-section': 'Books Management',
        'students-section': 'Students Management',
        'borrowings-section': 'Borrowings Management',
      };
      document.getElementById('section-title').textContent = titleMap[sectionId];
    });
  });
};

const renderBooks = () => {
  const tbody = document.getElementById('books-table');
  tbody.innerHTML = state.books
    .map(
      (book) => `
      <tr>
        <td>${book.id}</td>
        <td>${escapeHtml(book.title)}</td>
        <td>${escapeHtml(book.author)}</td>
        <td>${book.year}</td>
        <td>${book.status}</td>
        <td>
          <button class="action-btn edit-btn" data-action="edit-book" data-id="${book.id}">Edit</button>
          <button class="action-btn delete-btn" data-action="delete-book" data-id="${book.id}">Delete</button>
        </td>
      </tr>
    `
    )
    .join('');

  const bookSelect = document.getElementById('borrow-book');
  const availableBooks = state.books.filter((book) => book.status === 'Available');
  bookSelect.innerHTML = '<option value="">Select book</option>' +
    availableBooks.map((book) => `<option value="${book.id}">${book.title}</option>`).join('');
};

const renderStudents = () => {
  const tbody = document.getElementById('students-table');
  tbody.innerHTML = state.students
    .map(
      (student) => `
      <tr>
        <td>${student.id}</td>
        <td>${escapeHtml(student.name)}</td>
        <td>${escapeHtml(student.class_name)}</td>
        <td>
          <button class="action-btn edit-btn" data-action="edit-student" data-id="${student.id}">Edit</button>
          <button class="action-btn delete-btn" data-action="delete-student" data-id="${student.id}">Delete</button>
        </td>
      </tr>
    `
    )
    .join('');

  const studentSelect = document.getElementById('borrow-student');
  studentSelect.innerHTML = '<option value="">Select student</option>' +
    state.students.map((student) => `<option value="${student.id}">${student.name}</option>`).join('');
};

const renderBorrowings = () => {
  const tbody = document.getElementById('borrowings-table');
  tbody.innerHTML = state.borrowings
    .map(
      (borrowing) => `
      <tr>
        <td>${borrowing.id}</td>
        <td>${escapeHtml(borrowing.book_title)}</td>
        <td>${escapeHtml(borrowing.student_name)}</td>
        <td>${borrowing.borrow_date}</td>
        <td>${borrowing.return_date || '-'}</td>
        <td>
          ${
            borrowing.return_date
              ? '<span>Returned</span>'
              : `<button class="action-btn return-btn" data-action="return-book" data-id="${borrowing.id}">Return</button>`
          }
        </td>
      </tr>
    `
    )
    .join('');
};

const loadData = async () => {
  try {
    const [books, students, borrowings] = await Promise.all([
      request(api.books),
      request(api.students),
      request(api.borrowings),
    ]);

    state.books = books;
    state.students = students;
    state.borrowings = borrowings;

    renderBooks();
    renderStudents();
    renderBorrowings();
  } catch (error) {
    showFeedback(error.message, true);
  }
};

const resetBookForm = () => {
  document.getElementById('book-id').value = '';
  document.getElementById('book-form').reset();
};

const resetStudentForm = () => {
  document.getElementById('student-id').value = '';
  document.getElementById('student-form').reset();
};

const editBook = (book) => {
  document.getElementById('book-id').value = book.id;
  document.getElementById('book-title').value = book.title;
  document.getElementById('book-author').value = book.author;
  document.getElementById('book-year').value = book.year;
};

const editStudent = (student) => {
  document.getElementById('student-id').value = student.id;
  document.getElementById('student-name').value = student.name;
  document.getElementById('student-class').value = student.class_name;
};

const deleteBook = async (id) => {
  if (!confirm('Delete this book?')) return;
  try {
    await request(`${api.books}/${id}`, { method: 'DELETE' });
    showFeedback('Book deleted successfully.');
    await loadData();
  } catch (error) {
    showFeedback(error.message, true);
  }
};

const deleteStudent = async (id) => {
  if (!confirm('Delete this student?')) return;
  try {
    await request(`${api.students}/${id}`, { method: 'DELETE' });
    showFeedback('Student deleted successfully.');
    await loadData();
  } catch (error) {
    showFeedback(error.message, true);
  }
};

const returnBook = async (id) => {
  try {
    await request(`${api.borrowings}/${id}`, {
      method: 'PUT',
      body: JSON.stringify({ return_date: new Date().toISOString().slice(0, 10) }),
    });
    showFeedback('Book returned successfully.');
    await loadData();
  } catch (error) {
    showFeedback(error.message, true);
  }
};

document.addEventListener('click', async (event) => {
  const actionElement = event.target.closest('[data-action]');
  if (!actionElement) return;

  const { action, id } = actionElement.dataset;
  const numericId = Number(id);

  if (action === 'edit-book') {
    const book = state.books.find((item) => item.id === numericId);
    if (book) editBook(book);
    return;
  }

  if (action === 'delete-book') {
    await deleteBook(numericId);
    return;
  }

  if (action === 'edit-student') {
    const student = state.students.find((item) => item.id === numericId);
    if (student) editStudent(student);
    return;
  }

  if (action === 'delete-student') {
    await deleteStudent(numericId);
    return;
  }

  if (action === 'return-book') {
    await returnBook(numericId);
  }
});

document.getElementById('book-form').addEventListener('submit', async (event) => {
  event.preventDefault();

  const id = document.getElementById('book-id').value;
  const payload = {
    title: document.getElementById('book-title').value,
    author: document.getElementById('book-author').value,
    year: Number(document.getElementById('book-year').value),
  };

  try {
    if (id) {
      await request(`${api.books}/${id}`, { method: 'PUT', body: JSON.stringify(payload) });
      showFeedback('Book updated successfully.');
    } else {
      await request(api.books, { method: 'POST', body: JSON.stringify(payload) });
      showFeedback('Book added successfully.');
    }

    resetBookForm();
    await loadData();
  } catch (error) {
    showFeedback(error.message, true);
  }
});

document.getElementById('student-form').addEventListener('submit', async (event) => {
  event.preventDefault();

  const id = document.getElementById('student-id').value;
  const payload = {
    name: document.getElementById('student-name').value,
    class_name: document.getElementById('student-class').value,
  };

  try {
    if (id) {
      await request(`${api.students}/${id}`, { method: 'PUT', body: JSON.stringify(payload) });
      showFeedback('Student updated successfully.');
    } else {
      await request(api.students, { method: 'POST', body: JSON.stringify(payload) });
      showFeedback('Student added successfully.');
    }

    resetStudentForm();
    await loadData();
  } catch (error) {
    showFeedback(error.message, true);
  }
});

document.getElementById('borrowing-form').addEventListener('submit', async (event) => {
  event.preventDefault();

  const payload = {
    book_id: Number(document.getElementById('borrow-book').value),
    student_id: Number(document.getElementById('borrow-student').value),
    borrow_date: document.getElementById('borrow-date').value,
  };

  if (!payload.book_id || !payload.student_id) {
    showFeedback('Please select both a book and a student.', true);
    return;
  }

  try {
    await request(api.borrowings, { method: 'POST', body: JSON.stringify(payload) });
    showFeedback('Borrowing created successfully.');
    document.getElementById('borrowing-form').reset();
    document.getElementById('borrow-date').value = new Date().toISOString().slice(0, 10);
    await loadData();
  } catch (error) {
    showFeedback(error.message, true);
  }
});

document.getElementById('borrow-date').value = new Date().toISOString().slice(0, 10);
switchSection();
loadData();
