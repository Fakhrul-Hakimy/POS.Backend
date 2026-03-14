using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PosSystem.Application.Interfaces;
using PosSystem.Application.Service;
using PosSystem.Infrastructure.Data;
using System.Text;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var frontendOrigin = builder.Configuration["AppSettings:FrontendOrigin"] ?? "http://127.0.0.1:5500";
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500") // No trailing slashes!
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddAuthorization();


// Configure database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

var token = builder.Configuration.GetValue<string>("AppSettings:Token")
    ?? throw new InvalidOperationException("AppSettings:Token is missing.");
var issuer = builder.Configuration.GetValue<string>("AppSettings:Issuer")
    ?? throw new InvalidOperationException("AppSettings:Issuer is missing.");
var audience = builder.Configuration.GetValue<string>("AppSettings:Audience")
    ?? throw new InvalidOperationException("AppSettings:Audience is missing.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // The backend "gets" the cookie here
                context.Token = context.Request.Cookies["jwt_token"];
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>()!);
var app = builder.Build();

// Configure the HTTP request pipeline.


 app.MapOpenApi();
    app.MapScalarApiReference();
    
    // Automatically redirect to Scalar documentation
    app.MapGet("/", () => Results.Redirect("/scalar"));
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();


