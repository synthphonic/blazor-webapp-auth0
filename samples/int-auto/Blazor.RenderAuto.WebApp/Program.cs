using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Auth0.AspNetCore.Authentication.Custom;
using Blazor.RenderAuto.WebApp.Auth;
using Blazor.RenderAuto.WebApp.Client.Pages;
using Blazor.RenderAuto.WebApp.Components;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOidcPKCEAuthentication<CustomRemoteUserAccount>(options =>
{
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.ResponseType = "code";
    options.DefaultScopes = "email";
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapGet("/Account/Login", async (HttpContext httpContext, string redirectUri = "/") =>
{
    Console.WriteLine("LOGIN CALLED");

    var authProperties = new AuthenticationProperties
    {
        RedirectUri = redirectUri
    };

    await httpContext.ChallengeAsync("Auth0", authProperties);
});

app.MapGet("/Account/Logout", async (HttpContext httpContext, string redirectUri = "/") =>
{
    Console.WriteLine("LOGOUT CALLED");
    var authProperties = new AuthenticationProperties
    {
        RedirectUri = redirectUri
    };

    await httpContext.SignOutAsync("Auth0", authProperties);
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly);

app.Run();
