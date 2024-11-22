using Fend.Api;
using Fend.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddApiServices();

var app = builder.Build();

app.UseMiddlewareDefaults();

app.Run();