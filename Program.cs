using DotNetEnv;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Netzwerk.Controllers;
using Netzwerk.Data;
using Netzwerk.Service;
using Newtonsoft.Json;

Env.Load();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = Environment.GetEnvironmentVariable("AUTH_AUTHORITY");
        options.Audience = "dotnet-client";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = Environment.GetEnvironmentVariable("AUTH_AUTHORITY"),
            ValidAudience = "dotnet-client"
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers()
    .AddNewtonsoftJson(options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IClaimsTransformation, JwtRoleClaimTransformer>();
var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
builder.Services.AddDbContext<ApiContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddScoped<IKeywordService, KeywordService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGeoLocationService, GeoLocationservice>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(Environment.GetEnvironmentVariable("AUTH_URL") ?? ""),
                TokenUrl = new Uri(Environment.GetEnvironmentVariable("AUTH_TOKEN_URL") ?? ""),
                Scopes = new Dictionary<string, string>
                {
                    { "dotnet-client", "Access API" }
                }
            }
        }
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new[] { "dotnet-client" }
        }
    });
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy => policy.RequireRole("admin"))
    .AddPolicy("UserPolicy", policy => policy.RequireRole("user"));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId("dotnet-client");
        options.OAuthClientSecret(Environment.GetEnvironmentVariable("AUTH_CLIENT_SECRET"));
        options.OAuthUsePkce();
    });
}

app.UseAuthentication();
app.UseAuthorization();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapControllers();

app.Run();