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
    }
}
