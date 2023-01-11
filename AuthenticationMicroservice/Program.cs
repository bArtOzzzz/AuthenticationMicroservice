using AuthenticationMicroservice.HealthChecks.DatabaseCheck;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using AuthenticationMicroservice.Models.Request;
using AuthenticationMicroservice.Validation;
using Microsoft.AspNetCore.Mvc.Versioning;
using Azure.Security.KeyVault.Secrets;
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
using FluentValidation;
using Azure.Identity;
using Repositories;
using System.Text;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Key Vault URL
Environment.SetEnvironmentVariable("KVUrl", "https://applicationkv.vault.azure.net/");

// Database secret connection variables
Environment.SetEnvironmentVariable("TenantId", "b15d7df0-4a92-49e8-b851-b857d77abe6d");
Environment.SetEnvironmentVariable("ClientId", "5ac2fa80-c5b0-4959-8411-ad70a93694d6");
Environment.SetEnvironmentVariable("ClientSecretIdDbConnection", "zXc8Q~rIHIvH.85jBFL5LbtrimP3maTjZPAlOagY");

// JWT secret connection variables
Environment.SetEnvironmentVariable("ClientSecretIdJwt", "rnt8Q~_2L3fIqUenGHJ-tJrtHehzgHPXRcE3Ia_A");

// Connection to Azure Key Vaulte Database connection
var clientDatabase = new SecretClient(new Uri(Environment.GetEnvironmentVariable("KVUrl")!),
                                      new ClientSecretCredential(Environment.GetEnvironmentVariable("TenantId"),
                                                                 Environment.GetEnvironmentVariable("ClientId"),
                                                                 Environment.GetEnvironmentVariable("ClientSecretIdDbConnection")));

// Connection to Azure Key Vaulte Jwt
var clientJwt = new SecretClient(new Uri(Environment.GetEnvironmentVariable("KVUrl")!),
                                 new ClientSecretCredential(Environment.GetEnvironmentVariable("TenantId"),
                                                            Environment.GetEnvironmentVariable("ClientId"),
                                                            Environment.GetEnvironmentVariable("ClientSecretIdJwt")));

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

builder.Services.AddScoped<IValidator<RoleModel>, RoleModelValidator>();
builder.Services.AddScoped<IValidator<UserEmailModel>, UserEmailModelValidator>();
builder.Services.AddScoped<IValidator<UserLoginModel>, UserLoginModelValidator>();
builder.Services.AddScoped<IValidator<UserNameModel>, UserNameModelValidator>();
builder.Services.AddScoped<IValidator<UserPasswordModel>, UserPasswordModelValidator>();

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
builder.Services.AddFluentValidationClientsideAdapters();

// Add JWT Bearer validation for Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(clientJwt.GetSecret("Jwt-Secret").Value.Value)),
        ClockSkew = TimeSpan.Zero
    };
});

// Add connection to databases
builder.Services.AddDbContext<DataContext>(options =>
{
    // Azure connection
    //options.UseSqlServer(clientDatabase.GetSecret("ConnectionString-AuthenticationConnection").Value.Value, b => b.MigrationsAssembly("AuthenticationMicroservice"));

    // Local connection
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("AuthenticationMicroservice"));
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
              .WithOrigins("http://localhost:4200", "https://applicationclient.azurewebsites.net")
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

public partial class Program { }