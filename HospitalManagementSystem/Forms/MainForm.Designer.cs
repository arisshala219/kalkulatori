namespace HospitalManagementSystem.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        tabControlMain = new TabControl();
        tabReception = new TabPage();
        tabPatients = new TabPage();
        tabDoctors = new TabPage();
        tabAppointments = new TabPage();
        tabPrescriptions = new TabPage();

        dgvPatients = new DataGridView();
        dgvPatientLookup = new DataGridView();
        dgvDoctors = new DataGridView();
        dgvAppointments = new DataGridView();
        dgvUpcoming = new DataGridView();
        dgvPrescriptions = new DataGridView();
        dgvPrescriptionHistory = new DataGridView();
        dgvDailyVisits = new DataGridView();
        dgvTodayVisits = new DataGridView();

        txtPatientId = new TextBox();
        txtPatientName = new TextBox();
        txtPatientPhone = new TextBox();
        txtPatientAddress = new TextBox();
        txtSearchPatient = new TextBox();

        txtDoctorId = new TextBox();
        txtDoctorName = new TextBox();
        txtDoctorSpecialization = new TextBox();

        txtAppointmentTime = new TextBox();
        txtMedication = new TextBox();
        txtDosage = new TextBox();
        txtPrescriptionNotes = new TextBox();
        txtVisitReason = new TextBox();

        dtpPatientDob = new DateTimePicker();
        dtpAppointmentDate = new DateTimePicker();
        dtpVisitDate = new DateTimePicker();

        cmbAppointmentPatient = new ComboBox();
        cmbAppointmentDoctor = new ComboBox();
        cmbPrescriptionPatient = new ComboBox();
        cmbPrescriptionDoctor = new ComboBox();
        cmbReceptionPatient = new ComboBox();
        cmbHistoryPatient = new ComboBox();

        btnPatientAdd = new Button();
        btnPatientEdit = new Button();
        btnPatientDelete = new Button();
        btnPatientSearch = new Button();

        btnDoctorAdd = new Button();
        btnDoctorEdit = new Button();
        btnDoctorDelete = new Button();

        btnAppointmentSave = new Button();
        btnPrescriptionSave = new Button();
        btnLoadHistory = new Button();
        btnReceptionSave = new Button();

        tabControlMain.SuspendLayout();
        tabReception.SuspendLayout();
        tabPatients.SuspendLayout();
        tabDoctors.SuspendLayout();
        tabAppointments.SuspendLayout();
        tabPrescriptions.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPatients).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvPatientLookup).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvDoctors).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvAppointments).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvUpcoming).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvPrescriptions).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvPrescriptionHistory).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvDailyVisits).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvTodayVisits).BeginInit();
        SuspendLayout();

        tabControlMain.Controls.Add(tabReception);
        tabControlMain.Controls.Add(tabPatients);
        tabControlMain.Controls.Add(tabDoctors);
        tabControlMain.Controls.Add(tabAppointments);
        tabControlMain.Controls.Add(tabPrescriptions);
        tabControlMain.Dock = DockStyle.Fill;

        tabReception.Text = "Reception";
        tabReception.Controls.Add(new Label { Text = "Patient", Left = 20, Top = 20, Width = 100 });
        tabReception.Controls.Add(cmbReceptionPatient);
        tabReception.Controls.Add(new Label { Text = "Visit Date", Left = 20, Top = 55, Width = 100 });
        tabReception.Controls.Add(dtpVisitDate);
        tabReception.Controls.Add(new Label { Text = "Reason", Left = 20, Top = 90, Width = 100 });
        tabReception.Controls.Add(txtVisitReason);
        tabReception.Controls.Add(btnReceptionSave);
        tabReception.Controls.Add(new Label { Text = "Daily Visits", Left = 20, Top = 160 });
        tabReception.Controls.Add(dgvDailyVisits);
        tabReception.Controls.Add(new Label { Text = "Today's Visits", Left = 20, Top = 390 });
        tabReception.Controls.Add(dgvTodayVisits);
        tabReception.Controls.Add(new Label { Text = "Patient Search", Left = 520, Top = 20 });
        tabReception.Controls.Add(txtSearchPatient);
        tabReception.Controls.Add(btnPatientSearch);
        tabReception.Controls.Add(dgvPatientLookup);

        cmbReceptionPatient.SetBounds(120, 18, 250, 23);
        dtpVisitDate.SetBounds(120, 53, 250, 23);
        txtVisitReason.SetBounds(120, 88, 250, 23);
        btnReceptionSave.SetBounds(120, 120, 110, 30);
        btnReceptionSave.Text = "Save";
        btnReceptionSave.Click += btnReceptionSave_Click;

        txtSearchPatient.SetBounds(620, 18, 170, 23);
        btnPatientSearch.SetBounds(800, 17, 75, 25);
        btnPatientSearch.Text = "Search";
        btnPatientSearch.Click += btnPatientSearch_Click;

        dgvPatientLookup.SetBounds(520, 55, 520, 300);
        dgvPatientLookup.ReadOnly = true;
        dgvDailyVisits.SetBounds(20, 180, 480, 190);
        dgvTodayVisits.SetBounds(20, 410, 480, 190);

        tabPatients.Text = "Patients";
        tabPatients.Controls.Add(new Label { Text = "ID", Left = 20, Top = 20 });
        tabPatients.Controls.Add(txtPatientId);
        tabPatients.Controls.Add(new Label { Text = "Full Name", Left = 20, Top = 55 });
        tabPatients.Controls.Add(txtPatientName);
        tabPatients.Controls.Add(new Label { Text = "DOB", Left = 20, Top = 90 });
        tabPatients.Controls.Add(dtpPatientDob);
        tabPatients.Controls.Add(new Label { Text = "Phone", Left = 20, Top = 125 });
        tabPatients.Controls.Add(txtPatientPhone);
        tabPatients.Controls.Add(new Label { Text = "Address", Left = 20, Top = 160 });
        tabPatients.Controls.Add(txtPatientAddress);
        tabPatients.Controls.Add(btnPatientAdd);
        tabPatients.Controls.Add(btnPatientEdit);
        tabPatients.Controls.Add(btnPatientDelete);
        tabPatients.Controls.Add(dgvPatients);

        txtPatientId.SetBounds(120, 18, 80, 23);
        txtPatientId.ReadOnly = true;
        txtPatientName.SetBounds(120, 53, 300, 23);
        dtpPatientDob.SetBounds(120, 88, 300, 23);
        txtPatientPhone.SetBounds(120, 123, 300, 23);
        txtPatientAddress.SetBounds(120, 158, 300, 23);

        btnPatientAdd.SetBounds(120, 195, 85, 30);
        btnPatientAdd.Text = "Add";
        btnPatientAdd.Click += btnPatientAdd_Click;
        btnPatientEdit.SetBounds(215, 195, 85, 30);
        btnPatientEdit.Text = "Edit";
        btnPatientEdit.Click += btnPatientEdit_Click;
        btnPatientDelete.SetBounds(310, 195, 85, 30);
        btnPatientDelete.Text = "Delete";
        btnPatientDelete.Click += btnPatientDelete_Click;

        dgvPatients.SetBounds(20, 240, 1020, 360);
        dgvPatients.ReadOnly = true;
        dgvPatients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPatients.CellClick += dgvPatients_CellClick;

        tabDoctors.Text = "Doctors";
        tabDoctors.Controls.Add(new Label { Text = "ID", Left = 20, Top = 20 });
        tabDoctors.Controls.Add(txtDoctorId);
        tabDoctors.Controls.Add(new Label { Text = "Doctor Name", Left = 20, Top = 55 });
        tabDoctors.Controls.Add(txtDoctorName);
        tabDoctors.Controls.Add(new Label { Text = "Specialization", Left = 20, Top = 90 });
        tabDoctors.Controls.Add(txtDoctorSpecialization);
        tabDoctors.Controls.Add(btnDoctorAdd);
        tabDoctors.Controls.Add(btnDoctorEdit);
        tabDoctors.Controls.Add(btnDoctorDelete);
        tabDoctors.Controls.Add(dgvDoctors);

        txtDoctorId.SetBounds(140, 18, 90, 23);
        txtDoctorId.ReadOnly = true;
        txtDoctorName.SetBounds(140, 53, 350, 23);
        txtDoctorSpecialization.SetBounds(140, 88, 350, 23);

        btnDoctorAdd.SetBounds(140, 125, 85, 30);
        btnDoctorAdd.Text = "Add";
        btnDoctorAdd.Click += btnDoctorAdd_Click;
        btnDoctorEdit.SetBounds(235, 125, 85, 30);
        btnDoctorEdit.Text = "Edit";
        btnDoctorEdit.Click += btnDoctorEdit_Click;
        btnDoctorDelete.SetBounds(330, 125, 85, 30);
        btnDoctorDelete.Text = "Delete";
        btnDoctorDelete.Click += btnDoctorDelete_Click;

        dgvDoctors.SetBounds(20, 170, 1020, 430);
        dgvDoctors.ReadOnly = true;
        dgvDoctors.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvDoctors.CellClick += dgvDoctors_CellClick;

        tabAppointments.Text = "Appointments";
        tabAppointments.Controls.Add(new Label { Text = "Patient", Left = 20, Top = 20 });
        tabAppointments.Controls.Add(cmbAppointmentPatient);
        tabAppointments.Controls.Add(new Label { Text = "Doctor", Left = 20, Top = 55 });
        tabAppointments.Controls.Add(cmbAppointmentDoctor);
        tabAppointments.Controls.Add(new Label { Text = "Date", Left = 20, Top = 90 });
        tabAppointments.Controls.Add(dtpAppointmentDate);
        tabAppointments.Controls.Add(new Label { Text = "Time", Left = 20, Top = 125 });
        tabAppointments.Controls.Add(txtAppointmentTime);
        tabAppointments.Controls.Add(btnAppointmentSave);
        tabAppointments.Controls.Add(new Label { Text = "All Appointments", Left = 20, Top = 170 });
        tabAppointments.Controls.Add(dgvAppointments);
        tabAppointments.Controls.Add(new Label { Text = "Upcoming Appointments", Left = 20, Top = 395 });
        tabAppointments.Controls.Add(dgvUpcoming);

        cmbAppointmentPatient.SetBounds(100, 18, 300, 23);
        cmbAppointmentDoctor.SetBounds(100, 53, 300, 23);
        dtpAppointmentDate.SetBounds(100, 88, 300, 23);
        txtAppointmentTime.SetBounds(100, 123, 300, 23);
        btnAppointmentSave.SetBounds(100, 150, 100, 28);
        btnAppointmentSave.Text = "Save";
        btnAppointmentSave.Click += btnAppointmentSave_Click;

        dgvAppointments.SetBounds(20, 190, 1020, 190);
        dgvUpcoming.SetBounds(20, 415, 1020, 185);

        tabPrescriptions.Text = "Prescriptions";
        tabPrescriptions.Controls.Add(new Label { Text = "Patient", Left = 20, Top = 20 });
        tabPrescriptions.Controls.Add(cmbPrescriptionPatient);
        tabPrescriptions.Controls.Add(new Label { Text = "Doctor", Left = 20, Top = 55 });
        tabPrescriptions.Controls.Add(cmbPrescriptionDoctor);
        tabPrescriptions.Controls.Add(new Label { Text = "Medication", Left = 20, Top = 90 });
        tabPrescriptions.Controls.Add(txtMedication);
        tabPrescriptions.Controls.Add(new Label { Text = "Dosage", Left = 20, Top = 125 });
        tabPrescriptions.Controls.Add(txtDosage);
        tabPrescriptions.Controls.Add(new Label { Text = "Notes", Left = 20, Top = 160 });
        tabPrescriptions.Controls.Add(txtPrescriptionNotes);
        tabPrescriptions.Controls.Add(btnPrescriptionSave);
        tabPrescriptions.Controls.Add(new Label { Text = "All Prescriptions", Left = 20, Top = 200 });
        tabPrescriptions.Controls.Add(dgvPrescriptions);
        tabPrescriptions.Controls.Add(new Label { Text = "History by Patient", Left = 20, Top = 420 });
        tabPrescriptions.Controls.Add(cmbHistoryPatient);
        tabPrescriptions.Controls.Add(btnLoadHistory);
        tabPrescriptions.Controls.Add(dgvPrescriptionHistory);

        cmbPrescriptionPatient.SetBounds(110, 18, 300, 23);
        cmbPrescriptionDoctor.SetBounds(110, 53, 300, 23);
        txtMedication.SetBounds(110, 88, 300, 23);
        txtDosage.SetBounds(110, 123, 300, 23);
        txtPrescriptionNotes.SetBounds(110, 158, 300, 23);
        btnPrescriptionSave.SetBounds(110, 188, 100, 28);
        btnPrescriptionSave.Text = "Save";
        btnPrescriptionSave.Click += btnPrescriptionSave_Click;

        dgvPrescriptions.SetBounds(20, 225, 1020, 180);
        cmbHistoryPatient.SetBounds(140, 418, 250, 23);
        btnLoadHistory.SetBounds(400, 416, 110, 27);
        btnLoadHistory.Text = "Load History";
        btnLoadHistory.Click += btnLoadHistory_Click;
        dgvPrescriptionHistory.SetBounds(20, 450, 1020, 150);

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 680);
        Controls.Add(tabControlMain);
        Text = "Hospital Management System";

        tabControlMain.ResumeLayout(false);
        tabReception.ResumeLayout(false);
        tabReception.PerformLayout();
        tabPatients.ResumeLayout(false);
        tabPatients.PerformLayout();
        tabDoctors.ResumeLayout(false);
        tabDoctors.PerformLayout();
        tabAppointments.ResumeLayout(false);
        tabAppointments.PerformLayout();
        tabPrescriptions.ResumeLayout(false);
        tabPrescriptions.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPatients).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvPatientLookup).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvDoctors).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvAppointments).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvUpcoming).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvPrescriptions).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvPrescriptionHistory).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvDailyVisits).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvTodayVisits).EndInit();
        ResumeLayout(false);
    }

    private TabControl tabControlMain;
    private TabPage tabReception;
    private TabPage tabPatients;
    private TabPage tabDoctors;
    private TabPage tabAppointments;
    private TabPage tabPrescriptions;

    private DataGridView dgvPatients;
    private DataGridView dgvPatientLookup;
    private DataGridView dgvDoctors;
    private DataGridView dgvAppointments;
    private DataGridView dgvUpcoming;
    private DataGridView dgvPrescriptions;
    private DataGridView dgvPrescriptionHistory;
    private DataGridView dgvDailyVisits;
    private DataGridView dgvTodayVisits;

    private TextBox txtPatientId;
    private TextBox txtPatientName;
    private TextBox txtPatientPhone;
    private TextBox txtPatientAddress;
    private TextBox txtSearchPatient;

    private TextBox txtDoctorId;
    private TextBox txtDoctorName;
    private TextBox txtDoctorSpecialization;

    private TextBox txtAppointmentTime;
    private TextBox txtMedication;
    private TextBox txtDosage;
    private TextBox txtPrescriptionNotes;
    private TextBox txtVisitReason;

    private DateTimePicker dtpPatientDob;
    private DateTimePicker dtpAppointmentDate;
    private DateTimePicker dtpVisitDate;

    private ComboBox cmbAppointmentPatient;
    private ComboBox cmbAppointmentDoctor;
    private ComboBox cmbPrescriptionPatient;
    private ComboBox cmbPrescriptionDoctor;
    private ComboBox cmbReceptionPatient;
    private ComboBox cmbHistoryPatient;

    private Button btnPatientAdd;
    private Button btnPatientEdit;
    private Button btnPatientDelete;
    private Button btnPatientSearch;

    private Button btnDoctorAdd;
    private Button btnDoctorEdit;
    private Button btnDoctorDelete;

    private Button btnAppointmentSave;
    private Button btnPrescriptionSave;
    private Button btnLoadHistory;
    private Button btnReceptionSave;
}
