using FranksDinerBlazor.Client;
using FranksDinerBlazor.Server.Authentication;
using FranksDinerBlazor.Server.Interfaces;
using FranksDinerBlazor.Server.Models;
using FranksDinerBlazor.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;

builder.Services.AddAuthentication()
    .AddGoogle(o =>
    {
        o.ClientId = configuration["Authentication:Google:ClientId"];
        o.ClientSecret = configuration["Authentication:Google:ClientSecret"];
    });

builder.Services.AddDbContext<DatabaseContext>
    (options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<IOrder, OrderManager>();
builder.Services.AddTransient<IEconduitService, EconduitService>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod())
);

builder.Services.AddAuthentication(
        options => options.DefaultScheme = "FranksDinerAuthScheme")
    .AddScheme<FranksDinerAuthSchemeOptions, FranksDinerAuthHandler>(
        "FranksDinerAuthScheme", options => { });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.Run();
