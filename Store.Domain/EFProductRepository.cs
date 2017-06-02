using System.Linq;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Abstract;
using Store.Domain.Concrete;

namespace Store.Domain
{
    public class EFProductRepository : IProductRepository
    {
        private EFDbContext _context; //= new EFDbContext();

        public EFProductRepository(EFDbContext context)
        {
            _context = context;
        }
        public IQueryable<Entities.Product> Products
        {
            get { return _context.Products; }
        }

        public void SaveProduct(Entities.Product product)
        {
            if (product.ProductID == 0)
            {
                _context.Products.Add(product);
                _context.Entry(product).State = EntityState.Added;
            }
            else
            {
                _context.Entry(product).State = EntityState.Modified;
            }

            _context.SaveChanges();
        }

        public void DeleteProduct(Entities.Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}
