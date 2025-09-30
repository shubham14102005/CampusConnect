using CampusConnect.Data;
using CampusConnect.Models;
using CampusConnect.Repositories;
using CampusConnect.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Configure database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2️⃣ Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 3️⃣ Add repositories
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IAnswerVoteRepo, AnswerVoteRepo>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IQuestionTagRepository, QuestionTagRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();



// 4️⃣ Add MVC
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

//changing cookie behaviour for login
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // cookie valid 30 min
    options.SlidingExpiration = true; // refresh cookie if active
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});




var app = builder.Build();

// 5️⃣ Middleware pipeline
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

// 6️⃣ Routes
app.MapRazorPages();
app.MapHub<NotificationHub>("/notificationHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await DbSeeder.SeedRolesAndAdminAsync(roleManager);

    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Ensure database is created
    await context.Database.EnsureCreatedAsync();
}

// 7️⃣ Run app
app.Run();

