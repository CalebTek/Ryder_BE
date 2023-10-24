using NLog;
using NLog.Extensions.Logging;
using Ryder.Api.Configurations;
using Ryder.Application;
using Ryder.Application.Common.Hubs;
using Ryder.Infrastructure;
using Ryder.Infrastructure.Implementation;
using Ryder.Infrastructure.Interface;
using Ryder.Infrastructure.Seed;
using static Ryder.Api.Configurations.NLogConfiguration;


var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging((builderContext, config) => { AddNLogging(builderContext, config); });

builder.Services.AddDbContextAndConfigurations(builder.Environment, builder.Configuration);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDocumentUploadService, DocumentUploadService>();
builder.Services.AddTransient<NotificationHub>();
builder.Services.AddSignalR();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

builder.Services.ConfigurePaystack(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.AddDbContextAndConfigurations(builder.Environment, builder.Configuration);
builder.Services.ApplicationDependencyInjection();
builder.Services.InjectInfrastructure(builder.Configuration);
builder.Services.ConfigureCloudinary(builder.Configuration);
builder.Services.AddHttpClient();


// Add configuration settings from appsettings.json
builder.Configuration.SetBasePath(System.IO.Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.WithOrigins("https://ryder-frontend.vercel.app", "https://ryder.decagon.dev",
                "http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("AllowAllOrigins");

app.UseRouting();

app.UseAuthorization();

//app.ConfigureSignalR();
app.MapHub<NotificationHub>("/notificationsHub");
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();

await Seeder.SeedData(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAllOrigins");

app.MapControllers();

app.Run();