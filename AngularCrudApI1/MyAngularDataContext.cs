using AngularCrudApI1.Entities;
using AngularCrudApI1.Model;
using Microsoft.EntityFrameworkCore;

namespace AngularCrudApI1
{
    public class MyAngularDataContext:DbContext
    {

        public MyAngularDataContext(DbContextOptions<MyAngularDataContext> options) : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Agency> Agencies { get; set; }
    }
}
