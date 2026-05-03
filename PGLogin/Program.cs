using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PGLogin.Models;
using PGLogin.Models.Data;
using PGLogin.Models.Repository;
using PGLogin.Models.Repository.IRepository;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});
// Add services to the container.
builder.Services.AddControllersWithViews().AddSessionStateTempDataProvider();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(2); // set session timeout
    options.Cookie.HttpOnly = true;                 // security
    options.Cookie.IsEssential = true;              // required for GDPR compliance
});

/*builder.Services.AddIdentity<User, IdentityRole>(option =>
{
    option.Password.RequireDigit = true;
    option.Password.RequireLowercase = true;
    option.Password.RequiredLength = 6;
    option.User.RequireUniqueEmail = true;
    option.SignIn.RequireConfirmedAccount = false;
    option.SignIn.RequireConfirmedEmail = false;
    option.SignIn.RequireConfirmedPhoneNumber = false;
})
    .AddEntityFrameworkStores<MydBContext>()
    .AddDefaultTokenProviders();*/

builder.Services.AddDbContext<MydBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IRegisterRepository, RegisterRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IPGRepository, PGRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IPage_MasterRepository, Page_MasterRepository>();
builder.Services.AddScoped<IRole_MasterRepository, Role_MasterRepository>();
builder.Services.AddScoped<IRole_Page_Mapper_Repository, Role_Page_MapperRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<EmailSender>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient("MagicAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7062/");
});

//ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Use NonCommercial or Commercial as needed

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
app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.Run();