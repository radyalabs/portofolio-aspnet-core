using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace portofolio_aspnet_core.Models;

[Table("category")]
public class Category
{
    [Key]
    [Column("id_category")]
    public string Id { get; set; } = string.Empty;
    [Required]
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    public virtual ICollection<Project>? Projects { get; set; }
}
