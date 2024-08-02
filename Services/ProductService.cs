using AutoMapper;
using Crud.Context;
using Crud.Dto;
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


 

        public async Task<UserProductDto> getUserProduct(int UserId)
        {
            var user = await _dbContext.users.Include(u => u.Products).FirstOrDefaultAsync(u=>u.Id == UserId);

            if (user == null)
            {
                return null;
            }

            var userProductDto = new UserProductDto
            {
                UserId = user.Id,
                Name = user.name,
                Email = user.email,
                Address = user.address,
                Products = user.Products.Select(product => new Product1Dto
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    ProductPrice = product.ProductPrice,
                    ProductDescription = product.ProductDescription,
                }).ToList()
            };

            return userProductDto;
        }
    }


}

