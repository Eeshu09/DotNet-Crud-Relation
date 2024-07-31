using AutoMapper;
using Crud.Context;
using Crud.Interface;
using Crud.Model;
using Microsoft.EntityFrameworkCore;

namespace Crud.Services
{
    public class ProductService : IProduct
    {
        private readonly UserContext _dbContext;
        private  IMapper _Map;
        public ProductService(UserContext dbContext, IMapper Map)
        {
            _Map = Map;
            _dbContext=dbContext;
        }

        public async Task<ProductDto> AddProduct(ProductDto product)
        {
            var mappdata = _Map.Map<Product>(product);
            var result= await _dbContext.Products.AddAsync(mappdata);
            await _dbContext.SaveChangesAsync();
            var return1=_Map.Map<ProductDto>(result.Entity);
            return return1;

        }
    }
}
