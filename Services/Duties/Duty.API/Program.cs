using Duty.API.Consumers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//services

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            // Auth.API 'deki appsettings.json'daki deðerlerle eþleþmeli
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!))
        };
    });

builder.Services.AddAuthorization();


builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();//.InitializeWith<InitialData>();

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.UsingRabbitMq((context, factoryConfigurator) =>
    {
        var brokerHost = builder.Configuration["MessageBroker:Host"];
        var brokerUsername = builder.Configuration["MessageBroker:Username"];
        var brokerPassword = builder.Configuration["MessageBroker:Password"];

        factoryConfigurator.Host(brokerHost, "/", hostConfigurator =>
        {
            hostConfigurator.Username(brokerUsername);
            hostConfigurator.Password(brokerPassword);
        });

        factoryConfigurator.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

//pipelines

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();

app.Run();
