const api = {
  books: '/books',
  students: '/students',
  borrowings: '/borrowings',
};

const state = {
  books: [],
  students: [],
  borrowings: [],
  bookQuery: '',
};

const messageEl = document.getElementById('message');

function showMessage(text, type = 'success') {
  messageEl.textContent = text;
  messageEl.className = `message ${type}`;
}

function sanitize(value) {
  return String(value)
    .replaceAll('&', '&amp;')
    .replaceAll('<', '&lt;')
    .replaceAll('>', '&gt;')
    .replaceAll('"', '&quot;')
    .replaceAll("'", '&#39;');
}

async function request(url, options = {}) {
  const response = await fetch(url, {
    headers: { 'Content-Type': 'application/json' },
    ...options,
  });

  const data = await response.json().catch(() => ({}));
  if (!response.ok) {
    throw new Error(data.message || 'Request failed');
  }

  return data;
}

function renderBooks() {
  const table = document.getElementById('books-table');
  const query = state.bookQuery.trim().toLowerCase();

  const filteredBooks = state.books.filter((book) => {
    if (!query) return true;
    return book.title.toLowerCase().includes(query) || book.author.toLowerCase().includes(query);
  });

  table.innerHTML = filteredBooks
    .map((book) => {
      const isBorrowed = Number(book.is_borrowed) === 1;
      return `
      <tr>
        <td>${book.id}</td>
        <td>${sanitize(book.title)}</td>
        <td>${sanitize(book.author)}</td>
        <td>${book.year}</td>
        <td>
          <span class="badge ${isBorrowed ? 'borrowed' : 'available'}">
            ${isBorrowed ? 'Borrowed' : 'Available'}
          </span>
        </td>
        <td>
          <button class="borrow-btn" data-action="pick-book" data-id="${book.id}" ${isBorrowed ? 'disabled' : ''}>
            Borrow
          </button>
        </td>
        <td>
          <button class="edit-btn" data-action="edit-book" data-id="${book.id}">Edit</button>
          <button class="delete-btn" data-action="delete-book" data-id="${book.id}">Delete</button>
        </td>
      </tr>`;
    })
    .join('');

  const bookSelect = document.getElementById('borrow-book');
  const availableBooks = state.books.filter((book) => Number(book.is_borrowed) === 0);
  bookSelect.innerHTML = '<option value="">Select book</option>';
  availableBooks.forEach((book) => {
    bookSelect.innerHTML += `<option value="${book.id}">${sanitize(book.title)} - ${sanitize(book.author)}</option>`;
  });

  updateBorrowFormAvailability();
}

function renderStudents() {
  const table = document.getElementById('students-table');
  table.innerHTML = state.students
    .map(
      (student) => `
      <tr>
        <td>${student.id}</td>
        <td>${sanitize(student.name)}</td>
        <td>${sanitize(student.class)}</td>
        <td>
          <button class="edit-btn" data-action="edit-student" data-id="${student.id}">Edit</button>
          <button class="delete-btn" data-action="delete-student" data-id="${student.id}">Delete</button>
        </td>
      </tr>`
    )
    .join('');

  const studentSelect = document.getElementById('borrow-student');
  studentSelect.innerHTML = '<option value="">Select student</option>';
  state.students.forEach((student) => {
    studentSelect.innerHTML += `<option value="${student.id}">${sanitize(student.name)} (${sanitize(student.class)})</option>`;
  });
}

function renderBorrowings() {
  const table = document.getElementById('borrowings-table');
  table.innerHTML = state.borrowings
    .map((item) => {
      const isActive = !item.return_date;
      return `
      <tr class="${isActive ? 'active-borrowing' : ''}">
        <td>${item.id}</td>
        <td>${sanitize(item.book_title)}</td>
        <td>${sanitize(item.student_name)}</td>
        <td>${item.borrow_date}</td>
        <td>${item.return_date || '-'}</td>
        <td>
          ${
            isActive
              ? `<button class="return-btn" data-action="return-book" data-id="${item.id}">Return</button>`
              : '<span>Returned</span>'
          }
        </td>
      </tr>`;
    })
    .join('');
}

async function loadAll() {
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
}

function resetBookForm() {
  document.getElementById('book-id').value = '';
  document.getElementById('book-form').reset();
}

function resetStudentForm() {
  document.getElementById('student-id').value = '';
  document.getElementById('student-form').reset();
}

function updateBorrowFormAvailability() {
  const borrowSubmit = document.getElementById('borrow-submit');
  const hasAvailableBooks = state.books.some((book) => Number(book.is_borrowed) === 0);
  const hasStudents = state.students.length > 0;
  borrowSubmit.disabled = !(hasAvailableBooks && hasStudents);
}

function setupNavigation() {
  const titleMap = {
    books: 'Book Management',
    students: 'Student Management',
    borrowings: 'Borrowing Management',
  };

  document.querySelectorAll('.nav-btn').forEach((button) => {
    button.addEventListener('click', () => {
      document.querySelectorAll('.nav-btn').forEach((btn) => btn.classList.remove('active'));
      document.querySelectorAll('.panel').forEach((panel) => panel.classList.remove('active'));

      button.classList.add('active');
      const target = button.dataset.target;
      document.getElementById(target).classList.add('active');
      document.getElementById('section-title').textContent = titleMap[target];
    });
  });
}

document.getElementById('book-search').addEventListener('input', (event) => {
  state.bookQuery = event.target.value;
  renderBooks();
});

document.addEventListener('click', async (event) => {
  const actionEl = event.target.closest('[data-action]');
  if (!actionEl) return;

  const action = actionEl.dataset.action;
  const id = Number(actionEl.dataset.id);

  try {
    if (action === 'pick-book') {
      document.getElementById('borrow-book').value = String(id);
      document.querySelector('[data-target="borrowings"]').click();
      showMessage('Book selected for borrowing. Choose student and date.', 'success');
      return;
    }

    if (action === 'edit-book') {
      const book = state.books.find((entry) => entry.id === id);
      if (!book) return;
      document.getElementById('book-id').value = book.id;
      document.getElementById('book-title').value = book.title;
      document.getElementById('book-author').value = book.author;
      document.getElementById('book-year').value = book.year;
      return;
    }

    if (action === 'delete-book') {
      if (!confirm('Delete this book?')) return;
      await request(`${api.books}/${id}`, { method: 'DELETE' });
      showMessage('Book deleted successfully.');
      await loadAll();
      return;
    }

    if (action === 'edit-student') {
      const student = state.students.find((entry) => entry.id === id);
      if (!student) return;
      document.getElementById('student-id').value = student.id;
      document.getElementById('student-name').value = student.name;
      document.getElementById('student-class').value = student.class;
      return;
    }

    if (action === 'delete-student') {
      if (!confirm('Delete this student?')) return;
      await request(`${api.students}/${id}`, { method: 'DELETE' });
      showMessage('Student deleted successfully.');
      await loadAll();
      return;
    }

    if (action === 'return-book') {
      await request(`${api.borrowings}/${id}`, {
        method: 'PUT',
        body: JSON.stringify({ return_date: new Date().toISOString().slice(0, 10) }),
      });
      showMessage('Book returned successfully.');
      await loadAll();
    }
  } catch (error) {
    showMessage(error.message, 'error');
  }
});

document.getElementById('book-form').addEventListener('submit', async (event) => {
  event.preventDefault();

  const bookId = document.getElementById('book-id').value;
  const payload = {
    title: document.getElementById('book-title').value,
    author: document.getElementById('book-author').value,
    year: Number(document.getElementById('book-year').value),
  };

  try {
    if (bookId) {
      await request(`${api.books}/${bookId}`, {
        method: 'PUT',
        body: JSON.stringify(payload),
      });
      showMessage('Book updated successfully.');
    } else {
      await request(api.books, {
        method: 'POST',
        body: JSON.stringify(payload),
      });
      showMessage('Book added successfully.');
    }

    resetBookForm();
    await loadAll();
  } catch (error) {
    showMessage(error.message, 'error');
  }
});

document.getElementById('student-form').addEventListener('submit', async (event) => {
  event.preventDefault();

  const studentId = document.getElementById('student-id').value;
  const payload = {
    name: document.getElementById('student-name').value,
    class: document.getElementById('student-class').value,
  };

  try {
    if (studentId) {
      await request(`${api.students}/${studentId}`, {
        method: 'PUT',
        body: JSON.stringify(payload),
      });
      showMessage('Student updated successfully.');
    } else {
      await request(api.students, {
        method: 'POST',
        body: JSON.stringify(payload),
      });
      showMessage('Student added successfully.');
    }

    resetStudentForm();
    await loadAll();
  } catch (error) {
    showMessage(error.message, 'error');
  }
});

document.getElementById('borrowing-form').addEventListener('submit', async (event) => {
  event.preventDefault();

  const payload = {
    book_id: Number(document.getElementById('borrow-book').value),
    student_id: Number(document.getElementById('borrow-student').value),
    borrow_date: document.getElementById('borrow-date').value,
  };

  if (!payload.book_id || !payload.student_id || !payload.borrow_date) {
    showMessage('Please select a book, student, and date.', 'error');
    return;
  }

  try {
    await request(api.borrowings, {
      method: 'POST',
      body: JSON.stringify(payload),
    });

    showMessage('Borrowing saved successfully.');
    document.getElementById('borrowing-form').reset();
    document.getElementById('borrow-date').value = new Date().toISOString().slice(0, 10);
    await loadAll();
  } catch (error) {
    showMessage(error.message, 'error');
  }
});

async function initializeApp() {
  setupNavigation();
  document.getElementById('borrow-date').value = new Date().toISOString().slice(0, 10);

  try {
    await loadAll();
    showMessage('Library dashboard loaded.');
  } catch (error) {
    showMessage(error.message, 'error');
  }
}

initializeApp();
