using Microsoft.EntityFrameworkCore;
using Forum.DAL;
using Forum.Models;
using Serilog;
using Serilog.Events;

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

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(); //A category has zero or multiple 
builder.Services.AddScoped<IRoomRepository, RoomRepository>(); //Rooms which have zero or many
builder.Services.AddScoped<ITopicRepository, TopicRepository>(); //Topics which have zero or several
builder.Services.AddScoped<IPostRepository, PostRepository>(); //posts which have one or a bunch of
builder.Services.AddScoped<ICommentRepository, CommentRepository>(); //Comments

var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File($"Logs/app_{DateTime.Now}:yyy.MMdd_HHmmss).Log");
loggerConfiguration.Filter.ByExcluding(e => e.Properties.TryGetValue("SourceContext", out var value) &&
                                       e.Level == LogEventLevel.Information &&
                                        e.MessageTemplate.Text.Contains("Executed DbCommand"));

var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();
if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    DBInit.Seed(app);
}

app.UseStaticFiles();
app.MapDefaultControllerRoute();
app.Run();