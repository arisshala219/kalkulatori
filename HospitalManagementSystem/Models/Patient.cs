namespace HospitalManagementSystem.Models;

public class Patient
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}
