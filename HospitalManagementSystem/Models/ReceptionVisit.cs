namespace HospitalManagementSystem.Models;

public class ReceptionVisit
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public DateTime VisitDate { get; set; }
    public string Reason { get; set; } = string.Empty;
}
