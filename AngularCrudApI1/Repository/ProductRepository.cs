using AngularCrudApI1.Model;
using Microsoft.EntityFrameworkCore;

namespace AngularCrudApI1.Repository
{
    public class ProductRepository:IProductRepository
    {
        private readonly MyAngularDataContext _appDBContext;
        public ProductRepository(MyAngularDataContext context)
        {
            _appDBContext = context ??
            throw new ArgumentNullException(nameof(context));
        }

        public async Task<Product> CreateProduct(Product objProd)
        {
            _appDBContext.Products.Add(objProd);
            await _appDBContext.SaveChangesAsync();
            return objProd;
        }

        public async Task<IEnumerable<Product>> GetProduct()
        {
            return (IEnumerable<Product>)await _appDBContext.Products.ToListAsync();

        }

        

    }
}
