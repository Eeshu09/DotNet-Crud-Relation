using Crud.Context;
using Crud.Interface;
using Crud.Services;
using Sieve.Services;
using System.Runtime.CompilerServices;

namespace Crud.DI
{
    public  static  class Dependency
    {
        public static void DI(this IServiceCollection services)
        {
            services.AddScoped<IUser, UserService>();
            services.AddScoped<IProduct, ProductService>();
            services.AddSingleton<SieveProcessor>();
            services.AddAutoMapper(typeof(Program));
            
        }
    }
}
