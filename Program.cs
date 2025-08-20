using Challenges.Data;
using Challenges.Interfaces; // New using directive for Interfaces
using Challenges.Repositories; // New using directive for Repositories
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextPool<ChallengesDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("ChallengesConnection")));

// Register the Unit of Work and Generic Repository
// Option 1: Register GenericRepository for direct use (less common with UnitOfWork)
// builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Option 2: Register UnitOfWork (preferred when using a Unit of Work pattern)
builder.Services.AddScoped<IUnitOfWork, IUnitOfWork>();

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});


builder.Services.AddControllersWithViews();
// In Program.cs, after builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(Program)); 

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/{slug?}");

app.Run();
