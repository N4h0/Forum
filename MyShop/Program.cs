﻿using Microsoft.EntityFrameworkCore;
using Forum.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CategoryDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:CategoryDbContextConnection"]);

});

builder.Services.AddDbContext<RoomDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration.GetConnectionString("RoomDbContextConnection"));
});

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseStaticFiles();

app.MapDefaultControllerRoute();

app.Run();
