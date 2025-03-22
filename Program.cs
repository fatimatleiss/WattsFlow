using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WattsFlowProject.Models;
using WattsFlowProject.Data;
using WattsFlowProject.BusinessLogic.Repositories;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WattsFlowDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 23))));

builder.Services.AddScoped<ExpenseRepository>();
builder.Services.AddScoped<ExpensetypeRepository>();
builder.Services.AddScoped<PostRepository>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<CustomerTrafficRepository>();
builder.Services.AddScoped<SystemSettingsRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

 using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WattsFlowDbContext>();

     await dbContext.Database.MigrateAsync();

     if (!dbContext.Roles.Any())
    {
        dbContext.Roles.AddRange(
            new Role { RoleName = "Admin" },
            new Role { RoleName = "User" }
        );
        await dbContext.SaveChangesAsync();
    }

     if (!dbContext.Users.Any())
    {
        var passwordHasher = new PasswordHasher<User>();

        var adminRole = dbContext.Roles.FirstOrDefault(r => r.RoleName == "Admin");
        var userRole = dbContext.Roles.FirstOrDefault(r => r.RoleName == "User");

         var admin = new User
        {
            FirstName = "Admin",
            LastName = "User",
            Email = "admin123@wattsflow.com",
            PasswordHash = passwordHasher.HashPassword(null, "Admin123"),
            RoleId = adminRole.RoleId
        };

        
        var normalUser = new User
        {
            FirstName = "Normal",
            LastName = "User",
            Email = "user123@wattsflow.com",
            PasswordHash = passwordHasher.HashPassword(null, "User123"),
            RoleId = userRole.RoleId
        };

        dbContext.Users.AddRange(admin, normalUser);
        await dbContext.SaveChangesAsync();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    await next();
});

app.UseAuthentication();
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
