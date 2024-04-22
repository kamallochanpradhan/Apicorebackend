using AngularCrudApI1.Model;

namespace AngularCrudApI1.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProduct();

        Task<Product> CreateProduct(Product objProd);
    }
}
