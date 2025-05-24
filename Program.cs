using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Netzwerk.Data;
using Netzwerk.Service;
using Newtonsoft.Json;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddNewtonsoftJson(options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApiContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IKeywordService, KeywordService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGeoLocationService, GeoLocationservice>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapControllers();

app.Run();