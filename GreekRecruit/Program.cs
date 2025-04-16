using GreekRecruit.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using GreekRecruit.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("Sql");

builder.Services.AddDbContext<SqlDataContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Login/Login";
        options.AccessDeniedPath = "/Login/AccessDenied";
    });

builder.Services.AddAuthorization();

// AWS S3 Setup
var awsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESSKEY");
var awsSecretKey = Environment.GetEnvironmentVariable("AWS_SECRETKEY");
var awsRegion = RegionEndpoint.USEast2; // Change to your bucket's region if needed

builder.Services.AddSingleton<IAmazonS3>(_ =>
    new AmazonS3Client(awsAccessKey, awsSecretKey, awsRegion));

builder.Services.AddSingleton<S3Service>();


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

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
