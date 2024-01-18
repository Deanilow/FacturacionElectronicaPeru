namespace InvoicePeru.Domain.Entities;
public class User : BaseEntity
{
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!; 
    public string PasswordSalt { get; set; } = null!;
    public string Role { get; set; } = null!;
    public bool ChangePassword { get; set; }
    public bool Locked { get; set; }
}