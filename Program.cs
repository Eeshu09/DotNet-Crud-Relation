using Crud.Context;
using Crud.DI;
using Crud.Interface;
using Crud.Model.Hangfire;
using Crud.Services;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<UserContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("UsersCS")));


//builder.Services.AddScoped<IUser,UserService>();
//builder.Services.AddScoped<IProduct,ProductService>();
builder.Services.AddScoped<IHangFireJobs, IHangFireJobsService>();


builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireDb")));
builder.Services.AddHangfireServer();

var emailConfig = builder.Configuration
       .GetSection("EmailConfiguration")
       .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.DI();
//builder.Services.AddSingleton<SieveProcessor>();
//Register to Mapper


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    //AppPath = "" //The path for the Back To Site link. Set to null in order to hide the Back To  Site link.
    DashboardTitle = "AlSadique Hangfire Jobs",
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter{
            User = builder.Configuration.GetSection("HangfireSettings:UserName").Value,
                    Pass = builder.Configuration.GetSection("HangfireSettings:Password").Value
                }
            }
});

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
