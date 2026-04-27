using System.Data;
using System.Data.SQLite;

namespace HospitalManagementSystem.Data;

public static class Database
{
    private static readonly string DbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hospital.db");
    private static readonly string ConnectionString = $"Data Source={DbPath};Version=3;";

    public static SQLiteConnection GetConnection()
    {
        var connection = new SQLiteConnection(ConnectionString);
        connection.Open();

        // Enforce foreign keys so parent rows cannot be deleted when child rows exist.
        using var pragmaCommand = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection);
        pragmaCommand.ExecuteNonQuery();

        return connection;
    }

    public static void InitializeDatabase()
    {
        try
        {
            using var connection = GetConnection();

            var script = @"
            CREATE TABLE IF NOT EXISTS Patients (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FullName TEXT NOT NULL,
                DateOfBirth TEXT NOT NULL,
                Phone TEXT,
                Address TEXT
            );

            CREATE TABLE IF NOT EXISTS Doctors (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FullName TEXT NOT NULL,
                Specialization TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Appointments (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PatientId INTEGER NOT NULL,
                DoctorId INTEGER NOT NULL,
                Date TEXT NOT NULL,
                Time TEXT NOT NULL,
                FOREIGN KEY (PatientId) REFERENCES Patients(Id),
                FOREIGN KEY (DoctorId) REFERENCES Doctors(Id)
            );

            CREATE TABLE IF NOT EXISTS Prescriptions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PatientId INTEGER NOT NULL,
                DoctorId INTEGER NOT NULL,
                Medication TEXT NOT NULL,
                Dosage TEXT NOT NULL,
                Notes TEXT,
                FOREIGN KEY (PatientId) REFERENCES Patients(Id),
                FOREIGN KEY (DoctorId) REFERENCES Doctors(Id)
            );

            CREATE TABLE IF NOT EXISTS Reception (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PatientId INTEGER NOT NULL,
                VisitDate TEXT NOT NULL,
                Reason TEXT NOT NULL,
                FOREIGN KEY (PatientId) REFERENCES Patients(Id)
            );";

            using var command = new SQLiteCommand(script, connection);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Database initialization error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public static DataTable GetDataTable(string query, params SQLiteParameter[] parameters)
    {
        var table = new DataTable();

        using var connection = GetConnection();
        using var command = new SQLiteCommand(query, connection);
        using var adapter = new SQLiteDataAdapter(command);

        if (parameters.Length > 0)
        {
            command.Parameters.AddRange(parameters);
        }

        adapter.Fill(table);

        return table;
    }

    public static int ExecuteNonQuery(string query, params SQLiteParameter[] parameters)
    {
        using var connection = GetConnection();
        using var command = new SQLiteCommand(query, connection);

        if (parameters.Length > 0)
        {
            command.Parameters.AddRange(parameters);
        }

        return command.ExecuteNonQuery();
    }
}
