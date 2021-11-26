using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace portofolio_aspnet_core.Models;

[Table("project_image")]
public class ProjectImage
{
    [Key]
    [Column("id_project_image")]
    public string Id { get; set; } = string.Empty;
    [Required]
    [Column("id_project")]
    public string ProjectId { get; set; } = string.Empty;
    [Required]
    [Column("url")]
    public string Url { get; set; } = string.Empty;
    public virtual Project? Project { get; set;}
}
