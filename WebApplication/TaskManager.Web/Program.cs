using Microsoft.AspNetCore.Authentication.Cookies;
using TaskManager.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<AuthService>(client =>
{
    var dutyApiBaseUrl = builder.Configuration["ApiSettings:AuthApiBaseUrl"];
    client.BaseAddress = new Uri(dutyApiBaseUrl!);
});
/*.ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
      ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    }; 
});
+50 +s*/

builder.Services.AddHttpClient<DutyService>(client =>
{
    var dutyApiBaseUrl = builder.Configuration["ApiSettings:DutyApiBaseUrl"];
    client.BaseAddress = new Uri(dutyApiBaseUrl!);
});

builder.Services.AddHttpClient<ClientsService>(client =>
{
    var dutyApiBaseUrl = builder.Configuration["ApiSettings:DutyApiBaseUrl"];
    client.BaseAddress = new Uri(dutyApiBaseUrl!);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login"; 
        options.AccessDeniedPath = "/access-denied"; // buraya bi view ekleyelim
    });

//builder.Services.AddScoped<AuthService>();
//builder.Services.AddScoped<DutyService>();


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}");

app.Run();
