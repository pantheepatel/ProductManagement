global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using ProductManagement.Api;
global using ProductManagement.Api.models;
global using ProductManagement.Api.Repository;
global using ProductManagement.Api.UnitOfWork;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IUnitOfWork>(provider =>
{
    var context = provider.GetRequiredService<ProductDbContext>(); // your actual DbContext
    return new UnitOfWork<ProductDbContext>(context);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
