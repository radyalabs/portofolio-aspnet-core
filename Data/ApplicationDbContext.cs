using Microsoft.EntityFrameworkCore;
using portofolio_aspnet_core.Models;

namespace portofolio_aspnet_core.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    public DbSet<User>? Users { get; set; }
    public DbSet<Member>? Members { get; set; }
    public DbSet<Category>? Categories { get; set; }
    public DbSet<Project>? Projects { get; set; }
    public DbSet<ProjectImage>? ProjectImages { get; set; }
    public DbSet<ProjectMember>? ProjectMembers { get; set; }
    public DbSet<ProjectCategory>? ProjectCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>().HasMany(p => p.ProjectImages).WithOne(pi => pi.Project);
        modelBuilder.Entity<Project>().HasMany(p => p.Members).WithMany(m => m.Projects).UsingEntity<ProjectMember>();
        modelBuilder.Entity<Project>().HasMany(p => p.Categories).WithMany(c => c.Projects).UsingEntity<ProjectCategory>();
    }
}
