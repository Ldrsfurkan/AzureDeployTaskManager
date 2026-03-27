using Auth.API.Data;
using Auth.API.Features.Authorization;
using Auth.API.Features.DeleteUser;
using Auth.API.Features.GetUsers;
using Auth.API.Features.Login;
using Auth.API.Features.Register;
using Auth.API.Features.UpdateUser;
using BuildingBlocks.CQRS;
using Carter;
using Marten;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// add services

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCarter();

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


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!))
        };
    });

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions().InitializeWith<InitialData>();

//handlers
builder.Services.AddScoped<RegisterUserHandler>();
builder.Services.AddScoped<LoginUserHandler>();
builder.Services.AddScoped<AuthorizationHandler>();
builder.Services.AddScoped<GetUsersHandler>();
builder.Services.AddScoped<DeleteUserHandler>();
builder.Services.AddScoped<UpdateUserRoleHandler>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();
app.UseHttpsRedirection();


app.Run();