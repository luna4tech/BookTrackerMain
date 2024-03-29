using BookTrackerMain.repository;
using BookTrackerMain.Repository;
using Cassandra;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;

builder.Services.AddSingleton<Cassandra.ISession>(
	Cluster.Builder()
	.WithCloudSecureConnectionBundle(@"Resources\secure-connect-betterreads.zip")
	.WithCredentials(configuration.GetValue<string>("clientId"), configuration.GetValue<string>("clientSecret"))
	.Build()
	.Connect("main"));
builder.Services.AddSingleton<BookRepository>();
builder.Services.AddSingleton<AuthorRepository>();
builder.Services.AddSingleton<UserBookRepository>();
builder.Services.AddSingleton<UserBookDetailRepository>();

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
})
	.AddCookie()
	.AddFacebook(facebookOptions =>
		{
			facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
			facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
			facebookOptions.AccessDeniedPath = "/access-denied";
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
