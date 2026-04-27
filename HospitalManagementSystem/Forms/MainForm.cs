using System.Data;
using System.Data.SQLite;
using HospitalManagementSystem.Data;

namespace HospitalManagementSystem.Forms;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        Database.InitializeDatabase();
        LoadAllData();
    }

    private void LoadAllData()
    {
        LoadPatients();
        LoadDoctors();
        LoadAppointments();
        LoadPrescriptions();
        LoadReceptionVisits();
        LoadPatientDoctorDropdowns();
    }

    private void LoadPatients(string searchText = "")
    {
        try
        {
            const string sql = @"
                SELECT Id, FullName, DateOfBirth, Phone, Address
                FROM Patients
                WHERE FullName LIKE @Search
                ORDER BY FullName";

            dgvPatients.DataSource = Database.GetDataTable(sql, new SQLiteParameter("@Search", $"%{searchText}%"));
            dgvPatientLookup.DataSource = Database.GetDataTable(sql, new SQLiteParameter("@Search", $"%{searchText}%"));
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void LoadDoctors()
    {
        try
        {
            const string sql = "SELECT Id, FullName, Specialization FROM Doctors ORDER BY FullName";
            dgvDoctors.DataSource = Database.GetDataTable(sql);
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void LoadAppointments()
    {
        try
        {
            const string sql = @"
                SELECT a.Id, p.FullName AS Patient, d.FullName AS Doctor, a.Date, a.Time
                FROM Appointments a
                JOIN Patients p ON p.Id = a.PatientId
                JOIN Doctors d ON d.Id = a.DoctorId
                ORDER BY a.Date, a.Time";

            dgvAppointments.DataSource = Database.GetDataTable(sql);

            const string upcomingSql = @"
                SELECT a.Id, p.FullName AS Patient, d.FullName AS Doctor, a.Date, a.Time
                FROM Appointments a
                JOIN Patients p ON p.Id = a.PatientId
                JOIN Doctors d ON d.Id = a.DoctorId
                WHERE DATE(a.Date) >= DATE('now')
                ORDER BY a.Date, a.Time";

            dgvUpcoming.DataSource = Database.GetDataTable(upcomingSql);
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void LoadPrescriptions()
    {
        try
        {
            const string sql = @"
                SELECT pr.Id, p.FullName AS Patient, d.FullName AS Doctor, pr.Medication, pr.Dosage, pr.Notes
                FROM Prescriptions pr
                JOIN Patients p ON p.Id = pr.PatientId
                JOIN Doctors d ON d.Id = pr.DoctorId
                ORDER BY pr.Id DESC";

            dgvPrescriptions.DataSource = Database.GetDataTable(sql);
            dgvPrescriptionHistory.DataSource = Database.GetDataTable(sql);
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void LoadReceptionVisits()
    {
        try
        {
            const string allSql = @"
                SELECT r.Id, p.FullName AS Patient, r.VisitDate, r.Reason
                FROM Reception r
                JOIN Patients p ON p.Id = r.PatientId
                ORDER BY r.VisitDate DESC";

            dgvDailyVisits.DataSource = Database.GetDataTable(allSql);

            const string todaySql = @"
                SELECT r.Id, p.FullName AS Patient, r.VisitDate, r.Reason
                FROM Reception r
                JOIN Patients p ON p.Id = r.PatientId
                WHERE DATE(r.VisitDate) = DATE('now')
                ORDER BY r.VisitDate DESC";

            dgvTodayVisits.DataSource = Database.GetDataTable(todaySql);
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void LoadPatientDoctorDropdowns()
    {
        try
        {
            var patientSource = Database.GetDataTable("SELECT Id, FullName FROM Patients ORDER BY FullName");
            var doctorSource = Database.GetDataTable("SELECT Id, FullName FROM Doctors ORDER BY FullName");

            BindCombo(cmbAppointmentPatient, patientSource.Copy(), "FullName", "Id");
            BindCombo(cmbAppointmentDoctor, doctorSource.Copy(), "FullName", "Id");
            BindCombo(cmbPrescriptionPatient, patientSource.Copy(), "FullName", "Id");
            BindCombo(cmbPrescriptionDoctor, doctorSource.Copy(), "FullName", "Id");
            BindCombo(cmbReceptionPatient, patientSource.Copy(), "FullName", "Id");
            BindCombo(cmbHistoryPatient, patientSource.Copy(), "FullName", "Id");
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private static void BindCombo(ComboBox combo, DataTable source, string display, string value)
    {
        combo.DataSource = source;
        combo.DisplayMember = display;
        combo.ValueMember = value;
    }

    private void ShowError(Exception ex)
    {
        MessageBox.Show($"Operation failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private bool IsValidText(params string[] values)
    {
        return values.All(v => !string.IsNullOrWhiteSpace(v));
    }

    private void btnPatientAdd_Click(object sender, EventArgs e)
    {
        if (!IsValidText(txtPatientName.Text, txtPatientPhone.Text, txtPatientAddress.Text))
        {
            MessageBox.Show("Please fill in patient name, phone, and address.");
            return;
        }

        try
        {
            const string sql = @"
                INSERT INTO Patients (FullName, DateOfBirth, Phone, Address)
                VALUES (@FullName, @DateOfBirth, @Phone, @Address)";

            Database.ExecuteNonQuery(sql,
                new SQLiteParameter("@FullName", txtPatientName.Text.Trim()),
                new SQLiteParameter("@DateOfBirth", dtpPatientDob.Value.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@Phone", txtPatientPhone.Text.Trim()),
                new SQLiteParameter("@Address", txtPatientAddress.Text.Trim()));

            LoadAllData();
            ClearPatientInputs();
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void btnPatientEdit_Click(object sender, EventArgs e)
    {
        if (!int.TryParse(txtPatientId.Text, out var patientId))
        {
            MessageBox.Show("Select a patient to edit from the grid.");
            return;
        }

        try
        {
            const string sql = @"
                UPDATE Patients
                SET FullName = @FullName,
                    DateOfBirth = @DateOfBirth,
                    Phone = @Phone,
                    Address = @Address
                WHERE Id = @Id";

            Database.ExecuteNonQuery(sql,
                new SQLiteParameter("@FullName", txtPatientName.Text.Trim()),
                new SQLiteParameter("@DateOfBirth", dtpPatientDob.Value.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@Phone", txtPatientPhone.Text.Trim()),
                new SQLiteParameter("@Address", txtPatientAddress.Text.Trim()),
                new SQLiteParameter("@Id", patientId));

            LoadAllData();
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void btnPatientDelete_Click(object sender, EventArgs e)
    {
        if (!int.TryParse(txtPatientId.Text, out var patientId))
        {
            MessageBox.Show("Select a patient to delete from the grid.");
            return;
        }

        try
        {
            var hasReferences = Convert.ToInt32(Database.GetDataTable(
                "SELECT (SELECT COUNT(1) FROM Appointments WHERE PatientId = @Id) + " +
                "(SELECT COUNT(1) FROM Prescriptions WHERE PatientId = @Id) + " +
                "(SELECT COUNT(1) FROM Reception WHERE PatientId = @Id) AS RefCount",
                new SQLiteParameter("@Id", patientId)).Rows[0]["RefCount"]) > 0;

            if (hasReferences)
            {
                MessageBox.Show("Cannot delete patient with linked appointments/prescriptions/reception visits.");
                return;
            }

            Database.ExecuteNonQuery("DELETE FROM Patients WHERE Id = @Id", new SQLiteParameter("@Id", patientId));
            LoadAllData();
            ClearPatientInputs();
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void btnPatientSearch_Click(object sender, EventArgs e)
    {
        LoadPatients(txtSearchPatient.Text.Trim());
    }

    private void dgvPatients_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var row = dgvPatients.Rows[e.RowIndex];
        txtPatientId.Text = row.Cells["Id"].Value?.ToString();
        txtPatientName.Text = row.Cells["FullName"].Value?.ToString();
        txtPatientPhone.Text = row.Cells["Phone"].Value?.ToString();
        txtPatientAddress.Text = row.Cells["Address"].Value?.ToString();

        if (DateTime.TryParse(row.Cells["DateOfBirth"].Value?.ToString(), out var dob))
        {
            dtpPatientDob.Value = dob;
        }
    }

    private void ClearPatientInputs()
    {
        txtPatientId.Text = "";
        txtPatientName.Text = "";
        txtPatientPhone.Text = "";
        txtPatientAddress.Text = "";
        dtpPatientDob.Value = DateTime.Today;
    }

    private void btnDoctorAdd_Click(object sender, EventArgs e)
    {
        if (!IsValidText(txtDoctorName.Text, txtDoctorSpecialization.Text))
        {
            MessageBox.Show("Doctor name and specialization are required.");
            return;
        }

        try
        {
            Database.ExecuteNonQuery(
                "INSERT INTO Doctors (FullName, Specialization) VALUES (@Name, @Spec)",
                new SQLiteParameter("@Name", txtDoctorName.Text.Trim()),
                new SQLiteParameter("@Spec", txtDoctorSpecialization.Text.Trim()));

            LoadAllData();
            txtDoctorId.Text = txtDoctorName.Text = txtDoctorSpecialization.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void btnDoctorDelete_Click(object sender, EventArgs e)
    {
        if (!int.TryParse(txtDoctorId.Text, out var doctorId))
        {
            MessageBox.Show("Select a doctor from the grid.");
            return;
        }

        try
        {
            var hasReferences = Convert.ToInt32(Database.GetDataTable(
                "SELECT (SELECT COUNT(1) FROM Appointments WHERE DoctorId = @Id) + " +
                "(SELECT COUNT(1) FROM Prescriptions WHERE DoctorId = @Id) AS RefCount",
                new SQLiteParameter("@Id", doctorId)).Rows[0]["RefCount"]) > 0;

            if (hasReferences)
            {
                MessageBox.Show("Cannot delete doctor with linked appointments/prescriptions.");
                return;
            }

            Database.ExecuteNonQuery("DELETE FROM Doctors WHERE Id = @Id", new SQLiteParameter("@Id", doctorId));
            LoadAllData();
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void btnDoctorEdit_Click(object sender, EventArgs e)
    {
        if (!int.TryParse(txtDoctorId.Text, out var doctorId))
        {
            MessageBox.Show("Select a doctor from the grid.");
            return;
        }

        if (!IsValidText(txtDoctorName.Text, txtDoctorSpecialization.Text))
        {
            MessageBox.Show("Doctor name and specialization are required.");
            return;
        }

        try
        {
            Database.ExecuteNonQuery(
                "UPDATE Doctors SET FullName = @Name, Specialization = @Spec WHERE Id = @Id",
                new SQLiteParameter("@Name", txtDoctorName.Text.Trim()),
                new SQLiteParameter("@Spec", txtDoctorSpecialization.Text.Trim()),
                new SQLiteParameter("@Id", doctorId));

            LoadAllData();
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void dgvDoctors_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var row = dgvDoctors.Rows[e.RowIndex];
        txtDoctorId.Text = row.Cells["Id"].Value?.ToString();
        txtDoctorName.Text = row.Cells["FullName"].Value?.ToString();
        txtDoctorSpecialization.Text = row.Cells["Specialization"].Value?.ToString();
    }

    private void btnAppointmentSave_Click(object sender, EventArgs e)
    {
        if (cmbAppointmentPatient.SelectedValue is null || cmbAppointmentDoctor.SelectedValue is null)
        {
            MessageBox.Show("Please select both patient and doctor.");
            return;
        }

        if (!TimeOnly.TryParse(txtAppointmentTime.Text.Trim(), out _))
        {
            MessageBox.Show("Enter valid appointment time (example: 14:30).");
            return;
        }

        try
        {
            const string sql = @"
                INSERT INTO Appointments (PatientId, DoctorId, Date, Time)
                VALUES (@PatientId, @DoctorId, @Date, @Time)";

            Database.ExecuteNonQuery(sql,
                new SQLiteParameter("@PatientId", cmbAppointmentPatient.SelectedValue),
                new SQLiteParameter("@DoctorId", cmbAppointmentDoctor.SelectedValue),
                new SQLiteParameter("@Date", dtpAppointmentDate.Value.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@Time", txtAppointmentTime.Text.Trim()));

            LoadAllData();
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void btnPrescriptionSave_Click(object sender, EventArgs e)
    {
        if (!IsValidText(txtMedication.Text, txtDosage.Text) || cmbPrescriptionPatient.SelectedValue is null || cmbPrescriptionDoctor.SelectedValue is null)
        {
            MessageBox.Show("Prescription requires patient, doctor, medication, and dosage.");
            return;
        }

        try
        {
            const string sql = @"
                INSERT INTO Prescriptions (PatientId, DoctorId, Medication, Dosage, Notes)
                VALUES (@PatientId, @DoctorId, @Medication, @Dosage, @Notes)";

            Database.ExecuteNonQuery(sql,
                new SQLiteParameter("@PatientId", cmbPrescriptionPatient.SelectedValue),
                new SQLiteParameter("@DoctorId", cmbPrescriptionDoctor.SelectedValue),
                new SQLiteParameter("@Medication", txtMedication.Text.Trim()),
                new SQLiteParameter("@Dosage", txtDosage.Text.Trim()),
                new SQLiteParameter("@Notes", txtPrescriptionNotes.Text.Trim()));

            LoadAllData();
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void btnLoadHistory_Click(object sender, EventArgs e)
    {
        if (cmbHistoryPatient.SelectedValue is null) return;

        try
        {
            const string sql = @"
                SELECT pr.Id, p.FullName AS Patient, d.FullName AS Doctor, pr.Medication, pr.Dosage, pr.Notes
                FROM Prescriptions pr
                JOIN Patients p ON p.Id = pr.PatientId
                JOIN Doctors d ON d.Id = pr.DoctorId
                WHERE pr.PatientId = @PatientId
                ORDER BY pr.Id DESC";

            dgvPrescriptionHistory.DataSource = Database.GetDataTable(sql,
                new SQLiteParameter("@PatientId", cmbHistoryPatient.SelectedValue));
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void btnReceptionSave_Click(object sender, EventArgs e)
    {
        if (cmbReceptionPatient.SelectedValue is null || !IsValidText(txtVisitReason.Text))
        {
            MessageBox.Show("Please select patient and enter visit reason.");
            return;
        }

        try
        {
            Database.ExecuteNonQuery(
                "INSERT INTO Reception (PatientId, VisitDate, Reason) VALUES (@PatientId, @VisitDate, @Reason)",
                new SQLiteParameter("@PatientId", cmbReceptionPatient.SelectedValue),
                new SQLiteParameter("@VisitDate", dtpVisitDate.Value.ToString("yyyy-MM-dd")),
                new SQLiteParameter("@Reason", txtVisitReason.Text.Trim()));

            LoadAllData();
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }
}
