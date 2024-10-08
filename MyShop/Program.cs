using Microsoft.EntityFrameworkCore;
using Forum.DAL;
using Forum.Models;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Identity;

//N� fungerer programmet

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("CategoryDbContextConnection") ?? throw new 
    InvalidOperationException("Connection string 'CategoryDbContextConnection' not found.");

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

//This was removed from <identityUser>(...) for simple testing reason (in Baifan tutorial): options => options.SignIn.RequireConfirmedAccount = true 

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    //Password settings:
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 6;

    //Lockout settings:
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    //User settings:
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole>() //Adding roles to the program
    .AddDefaultUI()
    .AddEntityFrameworkStores<CategoryDbContext>()
    .AddDefaultTokenProviders();
//Getting an error when adding more security here and changing to AddIdentity instead of AddDefualtIdentity. Dont know the reason.

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(); //A category has zero or multiple 
builder.Services.AddScoped<IRoomRepository, RoomRepository>(); //Rooms which have zero or many
builder.Services.AddScoped<ITopicRepository, TopicRepository>(); //Topics which have zero or several
builder.Services.AddScoped<IPostRepository, PostRepository>(); //posts which have one or a bunch of
builder.Services.AddScoped<ICommentRepository, CommentRepository>(); //Comments

builder.Services.AddRazorPages(); //Order of adding services does not matter

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(1800); //30 minutes
    options.Cookie.IsEssential = true;
});


// Configure logger service
var logDirectory = "Logs";

//creates logger
if (!Directory.Exists(logDirectory))
{
    Directory.CreateDirectory(logDirectory);
}


//Configure logger service
var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File($"Logs/app_{DateTime.Now:yyyy.MMdd_HHmmss}.log");
loggerConfiguration.Filter.ByExcluding(e => e.Properties.TryGetValue("SourceContext", out var value) &&
                                       e.Level == LogEventLevel.Information &&
                                       e.MessageTemplate.Text.Contains("Executed DbCommand"));
;
//creates logger
var logger = loggerConfiguration.CreateLogger();
//add logger to builder
builder.Logging.AddSerilog(logger);

var app = builder.Build();
if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    DBInit.Seed(app);
}

app.UseStaticFiles();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.MapRazorPages();

using (var scope = app.Services.CreateScope()) //Want to access the services that we have defined above, and set roles to our project:
{
    //We are now seeding the roles into the project:
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(); 
    //The Rolemanager takes in the Identity role, which is the default role generated by Identty scaffolding, to set roles hiearchy which we are free to now define

    var roles = new[] { "SuperAdmin", "Admin", "Member" };

    foreach(var role in roles)
    {
        //Checking if the roles exist in the database, if not. They are created, but only once, and then they exist.
        if(!await roleManager.RoleExistsAsync(role)){
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

//We want to have a base SuperAdmin from the start of the enviroment in order to create Admins, who render the page
//and filter the stuff that members do if they step out of line in any way:
using (var scope = app.Services.CreateScope()) //Want to access the services that we have defined above, and set roles to our project:
{
    //We are now seeding the SuperAdmin into the project:
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    //We want to create a link between the Identtity beeing created, and set a role to it, therfore the userManager takes in the IdentityUser

    //Creating the email and password to the account:
    String email = "superAdmin@superAdmin.com";
    String username = "SuperAdmin";
    String password = "SuperAdmin123456!";

    //Checking if the user exist, only need to generate one:
    if(await userManager.FindByEmailAsync(email) == null)
    {
        //Creating the user:
        var user = new IdentityUser();
        user.UserName = username;
        user.Email = email;
        user.EmailConfirmed = true; //This makes the user confirmed in the system

        //Setting the user in the database
        await userManager.CreateAsync(user, password);

        //Setting the role in the database:
        await userManager.AddToRoleAsync(user, "SuperAdmin");

    }
}
app.Run();