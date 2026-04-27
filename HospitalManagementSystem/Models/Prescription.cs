namespace HospitalManagementSystem.Models;

public class Prescription
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public string Medication { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
