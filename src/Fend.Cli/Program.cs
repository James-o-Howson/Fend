using Cocona;
using Fend.Cli;
using Fend.Cli.Commands;
using Fend.ServiceDefaults;

var builder = CoconaApp.CreateBuilder(args);

builder.Host.AddLoggingDefaults(builder.Configuration);

builder.Services.AddCliServices();

var app = builder.Build();

app.AddCommands<ScanCommands>();

app.Run();