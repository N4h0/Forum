using Microsoft.EntityFrameworkCore;
using Forum.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CategoryDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:CategoryDbContextConnection"]);

});

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseStaticFiles();

app.MapDefaultControllerRoute();

app.Run();
