using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Signee.DisplayHandler.Hubs;
using Signee.Domain.RepositoryContracts.Areas.Display;
using Signee.Infrastructure.PostgreSql;
using Signee.Infrastructure.PostgreSql.Areas.Display;
using Signee.Services.Areas.Display.Contracts;
using Signee.Services.Areas.Display.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DB Context (connection)
var dbConnectionString = builder.Configuration.GetConnectionString("PostgreSql");
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(dbConnectionString));

// Register services for Dependency Injection
builder.Services.AddScoped<IDisplayRepository>(s => new DisplayRepository(s.GetRequiredService<ApplicationDbContext>()));
builder.Services.AddScoped<IDisplayService>(s => new DisplayService(s.GetRequiredService<IDisplayRepository>()));

// Allow CORS for frontend (when running locally)
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        corsBuilder => corsBuilder.WithOrigins(allowedOrigins ?? ["http://localhost:3000"])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseRequestLocalization(requestLocalizationOptions);
app.UseCors("AllowSpecificOrigin");

// Add SignalR websocket hub to application pipeline
app.MapHub<DisplayMessageHub>("display");

app.Run();
