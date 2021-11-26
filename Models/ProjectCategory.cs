using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace portofolio_aspnet_core.Models;

[Table("project_category")]
public class ProjectCategory
{
    [Key]
    [Column("id_project_category")]
    public string Id { get; set; } = string.Empty;
    [Required]
    [Column("id_project")]
    public string ProjectId { get; set; } = string.Empty;
    [Required]
    [Column("id_category")]
    public string CategoryId { get; set; } = string.Empty;
    public virtual Project? Project { get; set; }
    public virtual Category? Category { get; set; }

}
