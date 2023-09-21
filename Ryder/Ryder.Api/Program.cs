using Ryder.Api.Configurations;
using Ryder.Application;
using Ryder.Infrastructure;
using Ryder.Infrastructure.Implementation;
using Ryder.Infrastructure.Interface;
using Ryder.Infrastructure.Seed;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextAndConfigurations(builder.Environment, builder.Configuration);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddScoped<IUserService, UserService>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.AddDbContextAndConfigurations(builder.Environment, builder.Configuration);
builder.Services.ApplicationDependencyInjection();
builder.Services.InjectInfrastructure(builder.Configuration);
<<<<<<< HEAD
builder.Services.ConfigureSignalR(builder.Build());
=======
builder.Services.SetupSeriLog(builder.Configuration);

// Add configuration settings from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

>>>>>>> b69488edbc0e3d3adcb19350129a18c5c69f0872

var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();

await Seeder.SeedData(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();




app.Run();