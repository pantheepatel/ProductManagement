global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using ProductManagement.Api;
global using ProductManagement.Api.DTOs;
global using ProductManagement.Api.models;
global using ProductManagement.Api.Repository;
global using ProductManagement.Api.Services.CategoryService;
global using ProductManagement.Api.Services.CustomerService;
global using ProductManagement.Api.UnitOfWork;
global using ProductManagement.Api.Services.ProductService;
using ProductManagement.Api.Services.InvoiceService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

// Add DbContext
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IUnitOfWork>(provider =>
{
    var context = provider.GetRequiredService<ProductDbContext>(); // your actual DbContext
    return new UnitOfWork<ProductDbContext>(context);
});

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularClient");

app.UseAuthorization();

app.MapControllers();

app.Run();
