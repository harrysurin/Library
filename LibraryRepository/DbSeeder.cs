using LibraryRepository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class DbSeeder
{
    private readonly ModelBuilder modelBuilder;

    public DbSeeder(ModelBuilder modelBuilder)
    {
        this.modelBuilder = modelBuilder;
    }

    public void SeedRoles()
    {
        modelBuilder.Entity<Role>().HasData(
               new Role(){ Id = new Guid("729e2c22-3832-4ab5-8f28-5a2c99d85944"), Name = "Admin", NormalizedName = "ADMIN"},
               new Role(){ Id = new Guid("8f76dd13-0a57-492b-9131-8324382ccb06"), Name = "User", NormalizedName = "USER"}
        );
    }


}