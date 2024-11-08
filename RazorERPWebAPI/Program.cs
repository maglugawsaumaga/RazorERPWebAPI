using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RazorERPWebAPI.Repository;
using RazorERPWebAPI.Services;
using RazorERPWebAPI.Middleware;
using RazorERPWebAPI.Dapper;

var builder = WebApplication.CreateBuilder(args);

// Register DapperContext as a singleton service
builder.Services.AddSingleton<DapperContext>();

// JWT Configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Register PasswordHasherService with DI container
builder.Services.AddScoped<PasswordHasherService>();

// Add services to the container.
builder.Services.AddControllers();

// Register Dapper Database Context
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<AuthService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Register the middleware in Program.cs
app.UseMiddleware<ThrottlingMiddleware>();

app.Run();
