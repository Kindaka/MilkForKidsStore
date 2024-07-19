using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddCors();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/AuthenticatePage/Login";
        });
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var token = context.Request.Cookies["jsonToken"];
    if (!string.IsNullOrEmpty(token))
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
        if (jwtToken != null)
        {
            var claims = jwtToken.Claims.Select(c => new Claim(c.Type, c.Value)).ToList();
            var identity = new ClaimsIdentity(claims, "jwt");
            var principal = new ClaimsPrincipal(identity);
            context.User = principal;
        }
    }

    await next();
});
app.MapRazorPages();

app.Run();
