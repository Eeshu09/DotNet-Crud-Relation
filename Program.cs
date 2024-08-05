using Crud.Context;
using Crud.DI;
using Crud.Interface;
using Crud.Services;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<UserContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("UsersCS")));


//builder.Services.AddScoped<IUser,UserService>();
//builder.Services.AddScoped<IProduct,ProductService>();

builder.Services.DI();
//builder.Services.AddSingleton<SieveProcessor>();
//Register to Mapper


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();


app.MapControllers();

app.Run();
