using crackhub.Models.Data;
using crackhub.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Thêm DbContext và cấu hình kết nối cơ sở dữ liệu
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IGameRepository, EFGameRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();
builder.Services.AddScoped<IUserRepository, EFUserRepository>();
builder.Services.AddScoped<IDownloadHistoryRepository, EFDownloadHistoryRepository>();
builder.Services.AddScoped<IFavoriteGameRepository, EFFavoriteGameRepository>();
builder.Services.AddScoped<IReviewRepository, EFReviewRepository>();
builder.Services.AddScoped<IScreenshotRepository, EFScreenshotRepository>();
builder.Services.AddScoped<ISystemRequirementRepository, EFSystemRequirementRepository>();
builder.Services.AddScoped<IFeatureRepository, EFFeatureRepository>();
builder.Services.AddScoped<IGameTagRepository, EFGameTagRepository>();
builder.Services.AddScoped<ITagRepository, EFTagRepository>();
builder.Services.AddScoped<IGameLinkRepository, EFGameLinkRepository>();
builder.Services.AddScoped<ICrackInfoRepository, EFCrackInfoRepository>();
builder.Services.AddScoped<IRelatedGameRepository, EFRelatedGameRepository>();
builder.Services.AddScoped<ILocalizationInfoRepository, EFLocalizationInfoRepository>();
builder.Services.AddScoped<ISearchHistoryRepository, EFSearchHistoryRepository>();
builder.Services.AddScoped<IRoleRepository, EFRoleRepository>();
builder.Services.AddScoped<IAvatarFrameRepository, EFAvatarFrameRepository>();
builder.Services.AddScoped<IUserAvatarFrameRepository, EFUserAvatarFrameRepository>();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Thêm dịch vụ Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

// Thêm middleware Session
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
