using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;

namespace Store.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public EFDbContext(DbContextOptions<EFDbContext> obj) : base(obj)
        {
            
        }
        public DbSet<Product> Products { get; set; }

    }
}
