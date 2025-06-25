using MES_F1.Data;
using MES_F1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Ensure roles exist
    if (!await roleManager.RoleExistsAsync(ApplicationRoles.RoleAdmin))
    {
        await roleManager.CreateAsync(new IdentityRole(ApplicationRoles.RoleAdmin));
    }

    if (!await roleManager.RoleExistsAsync(ApplicationRoles.RoleDirector))
    {
        await roleManager.CreateAsync(new IdentityRole(ApplicationRoles.RoleDirector));
    }

    // Create default admin user if it doesn't exist
    var adminUser = await userManager.FindByEmailAsync("admin@example.com");
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            Name = " ",
            Surname = " ",
            UserName = "admin@example.com",
            NormalizedUserName = "ADMIN@EXAMPLE.COM",
            Email = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            EmailConfirmed = true
        };
        var createAdminResult = await userManager.CreateAsync(adminUser, "Pokemon123!");
        if (createAdminResult.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, ApplicationRoles.RoleAdmin);
        }
    }

    // Create default director user if it doesn't exist
    var directorUser = await userManager.FindByEmailAsync("director@example.com");
    if (directorUser == null)
    {
        directorUser = new ApplicationUser
        {
            Name = " ",
            Surname = " ",
            UserName = "director@example.com",
            NormalizedUserName = "DIRECTOR@EXAMPLE.COM",
            Email = "director@example.com",
            NormalizedEmail = "DIRECTOR@EXAMPLE.COM",
            EmailConfirmed = true
        };
        var createDirectorResult = await userManager.CreateAsync(directorUser, "Pokemon123!");
        if (createDirectorResult.Succeeded)
        {
            await userManager.AddToRoleAsync(directorUser, ApplicationRoles.RoleDirector);
        }
    }
}

app.Run();
