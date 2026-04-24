# Library Management Web Application

A full-stack Library Management app built using:
- **Frontend:** HTML, CSS, JavaScript
- **Backend:** Node.js + Express
- **Database:** SQLite (`library.db`)

## Features

### 1) Book Management (Libra)
- Add books (`title`, `author`, `year`)
- View books in a table
- Edit and delete books
- Search books by title or author
- Borrow button disabled automatically if book is already borrowed

### 2) Student Management (Nxënësit)
- Add students (`name`, `class`)
- View, edit, and delete students

### 3) Borrowing System (Huazime)
- Borrow a book by connecting a student and a book
- Return a book
- Stores `borrow_date` and `return_date`
- Prevents active duplicate borrowing of the same book
- Highlights active borrowings in the table

### 4) Modern Responsive UI
- Dashboard layout
- Sidebar navigation: Books, Students, Borrowings
- Mobile-friendly design
- Dynamic updates with Fetch API (no full page reload)
- Success/error feedback messages

---

## Project Structure

```bash
project-root/
  server/
    db.js
    server.js
    schema.sql
    library.db     # auto-created at runtime
    routes/
      books.js
      students.js
      borrowings.js
  public/
    index.html
    style.css
    app.js
  package.json
  .env.example
  README.md
```

---

## SQLite Schema

SQLite setup is handled automatically in `server/db.js` when the server starts.

You can also inspect the SQL in:
- `server/schema.sql`

Tables:
- `books(id, title, author, year)`
- `students(id, name, class)`
- `borrowings(id, book_id, student_id, borrow_date, return_date)`

---

## Step-by-Step Run Instructions (Visual Studio Code)

1. **Open project in Visual Studio Code**
   - `File` → `Open Folder...` → select this project.

2. **Open terminal in VS Code**
   - `Terminal` → `New Terminal`.

3. **Install dependencies**
   ```bash
   npm install
   ```

4. **(Optional) Create environment file**
   ```bash
   cp .env.example .env
   ```
   Default port is `5000`.

5. **Start the app**
   ```bash
   npm run dev
   ```
   or
   ```bash
   npm start
   ```

6. **Open in browser**
   - Visit: `http://localhost:5000`

---

## API Endpoints

### `/books`
- `GET /books` – get all books (with borrowing status)
- `POST /books` – add book
- `PUT /books/:id` – update book
- `DELETE /books/:id` – delete book

### `/students`
- `GET /students`
- `POST /students`
- `PUT /students/:id`
- `DELETE /students/:id`

### `/borrowings`
- `GET /borrowings` – all borrowings with JOIN data
- `POST /borrowings` – borrow book
- `PUT /borrowings/:id` – return book

---

## Notes
- The app uses `async/await` in all backend routes.
- Input validation and error handling are included.
- `library.db` is created automatically inside `/server`.
- The app is beginner-friendly and ready to run locally.
