using Fend.Application;
using Fend.Identity;
using Fend.Infrastructure;
using Fend.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddIdentity(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseMiddlewareDefaults();

app.Run();