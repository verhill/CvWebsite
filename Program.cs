using CV_Website.Models;
using CV_Website.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// L�gg till sessionshantering
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Timeout inst�llning f�r sessionen
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Registrera DbContext med anslutningsstr�ngen fr�n appsettings.json
builder.Services.AddDbContext<CVContext>(options =>
    options.UseLazyLoadingProxies()
           .UseSqlServer(builder.Configuration.GetConnectionString("CVContext"))
);
builder.Services.AddIdentity<User, IdentityRole<int>>()
            .AddEntityFrameworkStores<CVContext>()
            .AddDefaultTokenProviders();

var app = builder.Build();
//Skapar en ny tj�nst som bara lever n�r vi seedar datan
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        await SeedData.InitializeAsync(services, logger);
        logger.LogInformation("Data seeding completed successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Aktivera sessionshantering
app.UseSession();

app.UseRouting();

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();