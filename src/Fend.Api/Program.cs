using Fend.Api;
using Fend.Application;
using Fend.Infrastructure;
using Fend.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddApiServices();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseMiddlewareDefaults();

app.Run();