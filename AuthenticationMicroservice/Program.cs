using AuthenticationMicroservice.HealthChecks.DatabaseCheck;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using HealthChecks.UI.Client;
using Repositories.Abstract;
using Repositories.Context;
using Services.Abstract;
using Repositories;
using System.Text;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
                .AddJsonOptions(options =>
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

// Add Versioning for swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FridgeJWTToken",
        Version = "v1"
    });
    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Users",
        Version = "v2"
    });
});

// Add Fluent Validation
#pragma warning disable CS0618
builder.Services.AddFluentValidation(x =>
{
    x.ImplicitlyValidateChildProperties = true;
    x.ImplicitlyValidateRootCollectionElements = true;
    x.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
});
#pragma warning restore CS0618

// Add JWT Bearer validation for Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

// Add connection to the database
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration
           .GetConnectionString("DefaultConnection"), b => b
           .MigrationsAssembly("AuthenticationMicroservice"));
});

// Add Healthcheck
builder.Services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>(nameof(DatabaseHealthCheck))
                .AddCheck<PingHealthCheck>(nameof(PingHealthCheck));

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", config =>
    {
        config.SetIsOriginAllowedToAllowWildcardSubdomains()
              .WithOrigins("http://localhost:5000", "https://localhost:5001")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .Build();
    });
});

// Add API Versionings
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new MediaTypeApiVersionReader("x-api-version"));
});

builder.Services.AddVersionedApiExplorer(config =>
{
    config.GroupNameFormat = "'v'VVV";
    config.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigins");

// Healthcheck 
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    AllowCachingResponses = false
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FridgeJWTToken v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Users v2");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
