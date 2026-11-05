using GuardianAngel.Services;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Session holds the expected captcha code between showing the form and submitting it
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<CaptchaService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

// Old static URLs (about.html, index.html, ...) redirect to the new clean routes
app.UseRewriter(new RewriteOptions()
    .AddRedirect(@"^index\.html$", "/", StatusCodes.Status301MovedPermanently)
    .AddRedirect(@"^(about|ourgoals|project|blog|gallery|faq|contact)\.html$", "$1", StatusCodes.Status301MovedPermanently));

app.UseStaticFiles();

app.UseSession();

app.MapControllers();

app.Run();
