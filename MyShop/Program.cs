using Microsoft.EntityFrameworkCore;
using Forum.DAL;
using Forum.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<CategoryDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:CategoryDbContextConnection"]);

});

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<ITopicRepository, TopicRepository>();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    DBInit.Seed(app);
}


app.UseStaticFiles();

app.MapDefaultControllerRoute();

app.Run();
