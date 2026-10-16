using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

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

app.MapControllers();

app.Run();
