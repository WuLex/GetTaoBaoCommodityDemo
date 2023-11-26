using OfficeOpenXml;
using ScheduledCrawlerProject.Common;
using ScheduledCrawlerProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


ExcelPackage.LicenseContext = LicenseContext.Commercial;
// 注册定时任务服务和逻辑服务
builder.Services.AddSingleton<IHostedService, CoinGeckoCrawlerService>();
builder.Services.AddSingleton<IHostedService, InvestingCrawlerService>();
builder.Services.AddSingleton<CoinGeckoCrawlerLogic>();
builder.Services.AddSingleton<InvestingCrawlerLogic>();

// 注册HttpClient，如果需要的话
builder.Services.AddHttpClient();


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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
