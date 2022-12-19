using CakesByRAndRCore.Models;
using CakesByRAndRCore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using System.Runtime.Intrinsics.X86;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));


builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddSqlServer<CakesByRAndRCoreContext>(builder.Configuration.GetConnectionString("DefaultConnection"));



// Add services to the container.
builder.Services.AddControllersWithViews();


//builder.Services.AddControllers().AddNewtonsoftJson();
//builder.Services.AddControllersWithViews().AddNewtonsoftJson();
//builder.Services.AddRazorPages().AddNewtonsoftJson();

//Use the default serialization that is delivered with ASP.NET Core (recommended approach).
//-----------------------------------------------------------------------------------------

//builder.Services.AddControllers()
//            .AddJsonOptions(options =>
//                options.JsonSerializerOptions.PropertyNamingPolicy = null);

//builder.Services.AddControllersWithViews()
//            .AddJsonOptions(options =>
//                options.JsonSerializerOptions.PropertyNamingPolicy = null);

//builder.Services.AddRazorPages()
//            .AddJsonOptions(options =>
//                options.JsonSerializerOptions.PropertyNamingPolicy = null);


//-------------------------------------------------------------------------------------------

//Use the Newtonsoft library.
//-------------------------------------------------------------------------------------------
builder.Services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());

//builder.Services.AddControllersWithViews()
//                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());

//builder.Services.AddRazorPages()
//                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());


//builder.Services.AddMvc().AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());


builder.Services.Configure<IdentityOptions>(options =>
{


    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/AppUserAuthentication/Login";
    options.AccessDeniedPath = "/AppUserAuthentication/AccessDenied";
    options.SlidingExpiration = true;
});




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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");




//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
//    try
//    {
//        var context = services.GetRequiredService<AppIdentityDbContext>();
//        var userManager = services.GetRequiredService<UserManager<AppUser>>();
//        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

//        await SampleData.InitializeRolesAndUser(context,userManager, roleManager);
//        //await ContextSeed.SeedSuperAdminAsync(userManager, roleManager);
//    }
//    catch (Exception ex)
//    {
//        var logger = loggerFactory.CreateLogger<Program>();
//        logger.LogError(ex, "An error occurred seeding the DB.");
//    }
//}





app.Run();
