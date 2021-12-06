using System.ComponentModel.DataAnnotations.Schema;

namespace portofolio_aspnet_core.Models;

[Table("user")]
public class User
{
    [Key]
    [Column("id_user")]
    public string Id { get; set; } = string.Empty;
    [Required]
    [Column("username")]
    public string Username { get; set; } = string.Empty;
    [Required]
    [Column("password")]
    public string Password { get; set; } = string.Empty;
    [Required]
    [Column("email")]
    public string Email { get; set; } = string.Empty;
    public virtual ICollection<ResetPassword>? ResetPasswords { get; set; }
}

public class UserLogin
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
