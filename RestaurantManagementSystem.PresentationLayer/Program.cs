    using Mapster;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Application.Contracts;
using RestaurantManagementSystem.Application.Mapping;
using RestaurantManagementSystem.Application.Services;
using RestaurantManagementSystem.Domain.Repositories;
using RestaurantManagementSystem.Infrastructure.Context;
using RestaurantManagementSystem.Infrastructure.Implementations.Base;
using RestaurantManagementSystem.PresentationLayer.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var mappingConfig = new MappingConfig();
mappingConfig.Configure();

// Add Infrastructure Layer
var connectionString = builder.Configuration.GetConnectionString("RestaurantDb");
builder.Services.AddDbContext<RestaurantDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IServiceManager, ServiceManager>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

// Use Business Hours Middleware
//app.UseMiddleware<BusinessHoursMiddleware>();

// Add Business Hours Middleware with extension method
//app.UseBusinessHours();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
