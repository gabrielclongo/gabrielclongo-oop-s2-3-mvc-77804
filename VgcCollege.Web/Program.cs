using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Web.Data;
using VgcCollege.Web.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.Filters.Add(new AuthorizeFilter(policy)); 
});

builder.Services.AddRazorPages();

var app = builder.Build();


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


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var context = services.GetRequiredService<ApplicationDbContext>();

   
    string[] roles = { "Admin", "Faculty", "Student" };

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
            await userManager.AddToRoleAsync(admin, "Admin");
    }

   
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
            await userManager.AddToRoleAsync(student, "Student");
    }

   
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
            await userManager.AddToRoleAsync(faculty, "Faculty");
    }

    
    if (!context.Branches.Any())
    {
        context.Branches.AddRange(
            new Branch { Name = "Main Campus", Address = "Dublin" },
            new Branch { Name = "North Campus", Address = "Cork" }
        );

        context.SaveChanges();
    }
}

app.Run();