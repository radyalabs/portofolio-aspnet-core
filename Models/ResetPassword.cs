using System.ComponentModel.DataAnnotations.Schema;

namespace portofolio_aspnet_core.Models;

[Table("reset_password")]
public class ResetPassword
{
    [Key]
    [Column("id_reset_password")]
    public string Id { get; set; } = string.Empty;
    [Required]
    [Column("id_user")]
    public string UserId { get; set; } = string.Empty;
    [Required]
    [Column("token")]
    public string Token { get; set; } = string.Empty;
    [Required]
    [Column("issued_at")]
    public DateTime IssuedAt { get; set; }
    public virtual User? User { get; set;}
}

public class ChangePassword
{
    public string Password { get; set; } = string.Empty;
    public string PasswordConfirmation { get; set; } = string.Empty;
}