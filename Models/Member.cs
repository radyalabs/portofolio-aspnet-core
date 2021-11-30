using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace portofolio_aspnet_core.Models;

[Table("member")]
public class Member
{
    [Key]
    [Column("id_member")]
    public string Id { get; set; } = string.Empty;
    [Required]
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    [Column("profile_picture")]
    public string? ProfilePicture { get; set; }
    [Column("description")]
    public string? Description { get; set; }
    public virtual ICollection<Project>? Projects { get; set; }
}

public class MemberInput : Member
{
    public IFormFile? Image { get; set; }
}
