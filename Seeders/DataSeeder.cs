namespace portofolio_aspnet_core.Seeders;

public static class DataSeeder
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = "admin",
                Password = "admin123",
                Email = "yanuar.wanda2@gmail.com"
            }
        );
    }
}