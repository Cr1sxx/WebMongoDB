﻿using WebMongoDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebMongoDB.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WebMongoDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebMongoDBContext") ?? throw new InvalidOperationException("Connection string 'WebMongoDBContext' not found.")));

ContextMongodb.ConnectionString = builder.Configuration.GetSection("MongoConnections:ConnectionString").Value!;
ContextMongodb.DatabaseName = builder.Configuration.GetSection("MongoConnections:Database").Value!;
ContextMongodb.IsSSL = Convert.ToBoolean(builder.Configuration.GetSection("MongoConnections:IsSSL").Value);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
   
app.Run();
