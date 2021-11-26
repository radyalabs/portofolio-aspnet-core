using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace portofolio_aspnet_core.Models;

[Table("project_member")]
public class ProjectMember
{
    [Key]
    [Column("id_project_member")]
    public string Id { get; set; } = string.Empty;
    [Required]
    [Column("id_project")]
    public string ProjectId { get; set; } = string.Empty;
    [Required]
    [Column("id_member")]
    public string MemberId { get; set; } = string.Empty;
    public virtual Project? Project { get; set; }
    public virtual Member? Member { get; set; }
}
