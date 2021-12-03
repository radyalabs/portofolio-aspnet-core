using System.ComponentModel.DataAnnotations;
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
}

public class UserLogin
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
