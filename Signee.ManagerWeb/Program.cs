using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Signee.Domain.Identity;
using Signee.Domain.RepositoryContracts.Areas.Display;
using Signee.Domain.RepositoryContracts.Areas.User;
using Signee.Infrastructure.PostgreSql;
using Signee.Infrastructure.PostgreSql.Areas.Display;
using Signee.Infrastructure.PostgreSql.Areas.User;
using Signee.Services.Areas.Auth.Contracts;
using Signee.Services.Areas.Auth.Services;
using Signee.Services.Areas.Display.Contracts;
using Signee.Services.Areas.Display.Services;
using Signee.Services.Areas.User.Contracts;
using Signee.Services.Areas.User.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add swagger for documenting API on dev environment
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Configure api and routing
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add DB Context (connection)
var dbConnectionString = builder.Configuration.GetValue<string>("ConnectionStrings:PostgreSql");
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(dbConnectionString));

// Register services for Dependency Injection
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository>(s => new UserRepository(s.GetRequiredService<UserManager<ApplicationUser>>()));
builder.Services.AddScoped<IUserService>(s => new UserService(s.GetRequiredService<IUserRepository>(), s.GetRequiredService<ILogger<UserService>>()));
builder.Services.AddScoped<IAuthService>(s => new AuthService(s.GetRequiredService<IUserService>(), s.GetRequiredService<ITokenService>(), s.GetRequiredService<ILogger<AuthService>>()));
builder.Services.AddScoped<IDisplayRepository>(s => new DisplayRepository(s.GetRequiredService<ApplicationDbContext>()));
builder.Services.AddScoped<IDisplayService>(s => new DisplayService(s.GetRequiredService<IDisplayRepository>()));

// Support string to enum conversions in the API
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Specify identity requirements
// Must be added before .AddAuthentication otherwise a 404 is thrown on authorized endpoints
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Move to user secrets/env variables for release
var validIssuer = builder.Configuration.GetValue<string>("JwtTokenSettings:ValidIssuer");
var validAudience = builder.Configuration.GetValue<string>("JwtTokenSettings:ValidAudience");
var symmetricSecurityKey = builder.Configuration.GetValue<string>("JwtTokenSettings:SymmetricSecurityKey");

if (symmetricSecurityKey == null)
    throw new ArgumentNullException(nameof(symmetricSecurityKey));

// Configure JWT token validation/auth
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = validIssuer,
            ValidAudience = validAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(symmetricSecurityKey)
            ),
        };
    });

// Retrieve default culture setting from appsettings.json
var defaultCulture = builder.Configuration["CultureSettings:DefaultCulture"] ?? "sk";

// Retrieve supported cultures from appsettings.json
var supportedCultures = builder.Configuration.GetSection("CultureSettings:SupportedCultures")
    .GetChildren()
    .Select(c => new CultureInfo(c.Value ?? string.Empty))
    .ToList();

// Configure localization
var requestLocalizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(defaultCulture);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Build the app
var app = builder.Build();

// Create DB Tables if they don´t exist and run migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
    dbContext.Database.Migrate();
}

// Create default admin user upon first application deployment (when there is no admin user in DB)
using (var serviceScope = app.Services.CreateScope())
{
    var adminEmail = builder.Configuration.GetValue<string>("SiteSettings:AdminEmail") ?? "admin@admin.com";
    var adminPassword = builder.Configuration.GetValue<string>("SiteSettings:AdminPassword") ?? "admin";
    
    var userService = serviceScope.ServiceProvider.GetRequiredService<IUserService>();
    await userService.EnsureAdminUserCreatedAsync(adminEmail, adminPassword);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestLocalization(requestLocalizationOptions);
app.UseHttpsRedirection();
app.UseStatusCodePages();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();