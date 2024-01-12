using SiteServices = AlphaPlus.Settings.Site.SubAccount.Services;
using InfraServices = AlphaPlus.Settings.Infrastruture;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddSingleton<SiteServices.ISubAccountService, SiteServices.SubAccountService>();
builder.Services.AddSingleton<InfraServices.ISubAccountService, InfraServices.SubAccountService>();

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
    pattern: "{controller=SubAccount}/{action=Index}/{id?}");

app.Run();
