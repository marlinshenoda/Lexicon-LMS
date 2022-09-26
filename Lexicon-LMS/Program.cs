using Lexicon_LMS.Core.Entities;
using Lexicon_LMS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<Lexicon_LMSContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("Lexicon_LMSContext") ?? throw new InvalidOperationException("Connection string 'Lexicon_LMSContext' not found.")));
builder.Services.AddAutoMapper(typeof(MapperProfile));

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Lexicon_LMSContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("Lexicon_LMSContext") ?? throw new InvalidOperationException("Connection string 'Lexicon_LMSContext' not found.")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => 
options.SignIn.RequireConfirmedAccount = true
)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<Lexicon_LMSContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();


// Configure the HTTP request pipeline.
app.SeedDataAsync().GetAwaiter().GetResult();
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
app.UseRouting();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=WelcomePage}/{id?}");
app.MapRazorPages();

app.Run();
