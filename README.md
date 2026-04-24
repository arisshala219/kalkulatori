# Library Management Web Application

A complete full-stack Library Management app built with:
- **Frontend:** HTML, CSS, JavaScript
- **Backend:** Node.js + Express
- **Database:** MySQL

## Features

### 1) Books (Libra)
- Add books (title, author, year)
- View all books
- Edit books
- Delete books

### 2) Students (Nxënësit)
- Add students (name, class)
- View all students
- Edit students
- Delete students

### 3) Borrowings (Huazime)
- Borrow a book by linking student + book
- Return a book by updating `return_date`
- Stores `borrow_date` and `return_date`
- Prevents borrowing when a book is already borrowed

### 4) Modern UI
- Responsive dashboard layout
- Sidebar navigation (Books, Students, Borrowings)
- Forms + data tables
- Clean modern styling

---

## Project Structure

```bash
library-management-app/
├── public/
│   ├── index.html
│   ├── styles.css
│   └── app.js
├── server/
│   ├── db.js
│   ├── index.js
│   ├── schema.sql
│   └── routes/
│       ├── books.js
│       ├── students.js
│       └── borrowings.js
├── .env.example
├── package.json
└── README.md
```

---

## Database Script (MySQL)

Run the SQL file:

```sql
SOURCE server/schema.sql;
```

Or copy and execute the script manually from `server/schema.sql`.

---

## Setup Instructions (Step by Step)

### 1. Clone/Open in VS Code
1. Open VS Code.
2. Open the project folder.
3. Open integrated terminal: **Terminal → New Terminal**.

### 2. Install dependencies
```bash
npm install
```

### 3. Configure environment variables
1. Copy `.env.example` to `.env`:
   ```bash
   cp .env.example .env
   ```
2. Edit `.env` and set your MySQL credentials:
   - `DB_HOST`
   - `DB_PORT`
   - `DB_USER`
   - `DB_PASSWORD`
   - `DB_NAME`

### 4. Create database tables
Run MySQL and execute:
```bash
mysql -u root -p < server/schema.sql
```
(Replace `root` with your MySQL username.)

### 5. Start the app
```bash
npm run dev
```
or
```bash
npm start
```

### 6. Open in browser
Go to:
```text
http://localhost:5000
```

---

## API Endpoints

### Books
- `GET /books`
- `POST /books`
- `PUT /books/:id`
- `DELETE /books/:id`

### Students
- `GET /students`
- `POST /students`
- `PUT /students/:id`
- `DELETE /students/:id`

### Borrowings
- `GET /borrowings`
- `POST /borrowings`
- `PUT /borrowings/:id` (return book)

---

## Validation & Error Handling
- Input validation on all create/update routes
- Borrowing validation prevents double-borrowing the same book
- Guard checks for missing resources (404)
- Centralized error middleware for 500 errors

---

## Notes
- This app is ready to run locally.
- Uses async/await throughout backend logic.
- Code is structured and commented in important areas.
- The schema uses a generated-column unique index to enforce only one active borrowing per book (MySQL 8+ recommended).
