using Microsoft.EntityFrameworkCore;
using TechWizWebApp.Domain;

namespace TechWizWebApp.Data
{
    public class TechwizDbContext : DbContext
    {
        public TechwizDbContext(DbContextOptions<TechwizDbContext> options):base(options)
        {            
        }
        public DbSet<Product> Products { get; set; }
    }
}
