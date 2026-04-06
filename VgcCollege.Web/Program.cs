using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// -------------------- DATABASE (SQLite) --------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Login");
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Register");
});

// -------------------- IDENTITY --------------------
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// -------------------- MVC + 🔐 GLOBAL AUTH --------------------
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

   
});

var app = builder.Build();

// -------------------- MIDDLEWARE --------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// -------------------- ROUTES --------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// -------------------- ROLES + USERS AUTO --------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "Faculty", "Student" };

    // 🔹 Criar roles
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // ================= ADMIN =================
    var adminEmail = "admin@test.com";
    var admin = await userManager.FindByEmailAsync(adminEmail);

    if (admin == null)
    {
        admin = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, "Admin123!");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
    else
    {
        if (!await userManager.IsInRoleAsync(admin, "Admin"))
            await userManager.AddToRoleAsync(admin, "Admin");
    }

    // ================= STUDENT =================
    var studentEmail = "student@test.com";
    var student = await userManager.FindByEmailAsync(studentEmail);

    if (student == null)
    {
        student = new IdentityUser
        {
            UserName = studentEmail,
            Email = studentEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(student, "Student123!");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(student, "Student");
        }
    }
    else
    {
        if (!await userManager.IsInRoleAsync(student, "Student"))
            await userManager.AddToRoleAsync(student, "Student");
    }

    // ================= FACULTY =================
    var facultyEmail = "faculty@test.com";
    var faculty = await userManager.FindByEmailAsync(facultyEmail);

    if (faculty == null)
    {
        faculty = new IdentityUser
        {
            UserName = facultyEmail,
            Email = facultyEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(faculty, "Faculty123!");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(faculty, "Faculty");
        }
    }
    else
    {
        if (!await userManager.IsInRoleAsync(faculty, "Faculty"))
            await userManager.AddToRoleAsync(faculty, "Faculty");
    }
}

app.Run();