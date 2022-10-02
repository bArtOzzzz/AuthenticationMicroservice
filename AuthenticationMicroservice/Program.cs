using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Text;
using Repositories.Context;
using Repositories.Abstract;
using Repositories;
using Services.Abstract;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Enable AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();

builder.Services.AddScoped<IAuthenticateRepository, AuthenticateRepository>();
builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();

builder.Services.AddScoped<IGenerateTokenRepository, GenerateTokenRepository>();
builder.Services.AddScoped<IGenerateTokenService, GenerateTokenService>();

builder.Services.AddScoped<IRegisterRepository, RegisterRepository>();
builder.Services.AddScoped<IRegisterService, RegisterService>();

builder.Services.AddScoped<IRolesRepository, RolesRepository>();
builder.Services.AddScoped<IRolesService, RolesService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});

// Add connection to the database
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("AuthenticationMicroservice"));
});

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", config =>
    {
        config.SetIsOriginAllowedToAllowWildcardSubdomains()
        // localhost - front trace
        .WithOrigins("http://localhost:5000", "https://localhost:5001")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .Build();
    });
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FridgeJWTToken v1"));
}

app.UseHttpsRedirection();


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
