using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JobJump.Data;
using JobJump.Seed;
using JobJump.Models;
using JobJump.Services;

var builder = WebApplication.CreateBuilder(args);


// ===============================
// DATABASE
// ===============================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);


// ===============================
// IDENTITY + ROLES
// ===============================
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();


// ===============================
// MVC
// ===============================
builder.Services.AddControllersWithViews();


// ===============================
// AI RESUME SERVICE
// ===============================
builder.Services.AddScoped<ResumeAIService>();


// ===============================
// EMAIL SETTINGS
// ===============================
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);


// ===============================
// EMAIL SERVICE
// ===============================
builder.Services.AddScoped<EmailService>();


var app = builder.Build();


// ===============================
// ERROR HANDLING
// ===============================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}


// ===============================
// MIDDLEWARE
// ===============================
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();


// ===============================
// ROUTING
// ===============================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages();


// ===============================
// ROLE SEEDER
// ===============================
using (var scope = app.Services.CreateScope())
{
    var roleManager =
        scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

    await RoleSeeder.SeedRoles(roleManager);
}


app.Run();