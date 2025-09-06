using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using star_events.Data;
using star_events.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
// Configure DbContext with MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
<<<<<<< HEAD
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 25))));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add Identity services with custom User and Role types
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole<int>>() // Use custom Role with int TKey
=======
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(connectionString)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Enable Roles
>>>>>>> 1a6e91a5c3e183d04a96b5123e9fbd038a46d20b
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


builder.Services.AddRazorPages(); //Enable Razor Pages


var app = builder.Build();




//// Seed initial roles and users
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
//    var roleManager = services.GetRequiredService<RoleManager<Role>>();

//    // Ensure roles exist
//    string[] roles = { "Admin", "Organizer", "Customer" };
//    foreach (var role in roles)
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new Role { Name = role });
//        }
//    }

//    // Register Admin
//    var adminEmail = "admin@starevents.com";
//    var admin = await userManager.FindByEmailAsync(adminEmail);
//    if (admin == null)
//    {
//        admin = new ApplicationUser
//        {
//            UserName = adminEmail,
//            Email = adminEmail,
//            FirstName = "Admin",
//            LastName = "User",
//            ContactNo = "123-456-7890",
//            Address = "123 Admin St",
//            LoyaltyPoints = 0
//        };
//        var result = await userManager.CreateAsync(admin, "Admin@123");
//        if (result.Succeeded)
//        {
//            await userManager.AddToRoleAsync(admin, "Admin");
//        }
//    }

//    // Register Event Organizer
//    var organizerEmail = "organizer@starevents.com";
//    var organizer = await userManager.FindByEmailAsync(organizerEmail);
//    if (organizer == null)
//    {
//        organizer = new ApplicationUser
//        {
//            UserName = organizerEmail,
//            Email = organizerEmail,
//            FirstName = "Event",
//            LastName = "Organizer",
//            ContactNo = "987-654-3210",
//            Address = "456 Organizer Ave",
//            LoyaltyPoints = 0
//        };
//        var result = await userManager.CreateAsync(organizer, "Organizer@123");
//        if (result.Succeeded)
//        {
//            await userManager.AddToRoleAsync(organizer, "Organizer");
//        }
//    }

//    // Register Customer
//    var customerEmail = "customer@starevents.com";
//    var customer = await userManager.FindByEmailAsync(customerEmail);
//    if (customer == null)
//    {
//        customer = new ApplicationUser
//        {
//            UserName = customerEmail,
//            Email = customerEmail,
//            FirstName = "John",
//            LastName = "Doe",
//            ContactNo = "555-555-5555",
//            Address = "789 Customer Rd",
//            LoyaltyPoints = 100
//        };
//        var result = await userManager.CreateAsync(customer, "Customer@123");
//        if (result.Succeeded)
//        {
//            await userManager.AddToRoleAsync(customer, "Customer");
//        }
//    }
//}






// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();

app.Run();
