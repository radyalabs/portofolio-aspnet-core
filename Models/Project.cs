using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace portofolio_aspnet_core.Models;

[Table("project")]
public class Project
{
    [Key]
    [Column("id_project")]
    public string Id { get; set; } = string.Empty;
    [Required]
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    [Column("description")]
    public string? Description { get; set; }
    [Column("content")]
    public string? Content { get; set; }
    [Column("url")]
    public string? Url { get; set; }
    public virtual ICollection<ProjectImage>? ProjectImages { get; set; }
    public virtual ICollection<Member>? Members { get; set; }
    public virtual ICollection<Category>? Categories { get; set; }
}

public class ProjectInput : Project
{
    public List<IFormFile>? Images { get; set; }
    public List<string>? MemberIds { get; set; }
    public List<string>? CategoryIds { get; set; }
}