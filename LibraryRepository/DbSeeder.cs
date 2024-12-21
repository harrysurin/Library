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
               new Role(){ Id = new Guid("729e2c22-3832-4ab5-8f28-5a2c99d85944"), Name = "Admin"},
               new Role(){ Id = new Guid("8f76dd13-0a57-492b-9131-8324382ccb06"), Name = "User" }
        );
    }

    // public void SeedAdminUser()
    // {
    //     var user = new User
    //     {
    //         Id = new Guid("37794e22-6ed1-4fd6-ace4-a69fb910d2a0"), // primary key
    //         UserName = "admin",
    //         NormalizedUserName = "ADMIN",
    //     };

    //     var hasher = new PasswordHasher<User>();
    //     user.PasswordHash = hasher.HashPassword(user, "Pa$$w0rd");

    //     //Seeding the User to AspNetUsers table
    //     modelBuilder.Entity<User>().HasData(user);

    //     //Seeding the relation between our user and role to AspNetUserRoles table
    //     modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
    //         new IdentityUserRole<Guid>
    //         {
    //             RoleId = new Guid("729e2c22-3832-4ab5-8f28-5a2c99d85944"), 
    //             UserId = new Guid("37794e22-6ed1-4fd6-ace4-a69fb910d2a0")
    //         }
    //     );
    // }
}