using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddNewtonsoftJson();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "MultiAuthSchemes";
        options.DefaultChallengeScheme = "MultiAuthSchemes";
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/unauthorized";
        options.AccessDeniedPath = "/auth/forbidden";
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7208/",
            ValidAudience = "https://localhost:7208/",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@1"))
        };
    })
    .AddJwtBearer("SecondJwtScheme", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7209/",
            ValidAudience = "https://localhost:7208/",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@2"))
        };
    })
    .AddPolicyScheme("MultiAuthSchemes", JwtBearerDefaults.AuthenticationScheme, options =>
     {
         options.ForwardDefaultSelector = context =>
         {
             string authorization = context.Request.Headers[HeaderNames.Authorization];
             if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
             {
                 var token = authorization.Substring("Bearer ".Length).Trim();
                 var jwtHandler = new JwtSecurityTokenHandler();
                 return (jwtHandler.CanReadToken(token) && jwtHandler.ReadJwtToken(token).Issuer.Equals("https://localhost:7208/"))
                     ? JwtBearerDefaults.AuthenticationScheme : "SecondJwtScheme";
             }
             return CookieAuthenticationDefaults.AuthenticationScheme;
         };
     });

builder.Services.AddAuthorization(options =>
{
    /*var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
        JwtBearerDefaults.AuthenticationScheme,
        CookieAuthenticationDefaults.AuthenticationScheme,
        "SecondJwtScheme");
    defaultAuthorizationPolicyBuilder =
        defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();*/

    var onlySecondJwtSchemePolicyBuilder = new AuthorizationPolicyBuilder("SecondJwtScheme");
    options.AddPolicy("OnlySecondJwtScheme", onlySecondJwtSchemePolicyBuilder
        .RequireAuthenticatedUser()
        .Build());
    var onlyCookieSchemePolicyBuilder = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme);
    options.AddPolicy("OnlyCookieScheme", onlyCookieSchemePolicyBuilder
        .RequireAuthenticatedUser()
        .Build());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
