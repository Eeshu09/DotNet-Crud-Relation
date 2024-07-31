using Crud.Model;

namespace Crud.Interface
{
    public interface IProduct 
    {
        Task<ProductDto> AddProduct(ProductDto product);
    }
}
