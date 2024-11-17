using Cocona;
using Fend.Cli;
using Fend.Cli.Commands;
using Fend.DependencyGraph;
using Fend.Infrastructure;

var builder = CoconaApp.CreateBuilder(args);

builder.Services.AddCli();
builder.Services.AddInfrastructure();
builder.Services.AddCommandHandlers();
builder.Services.AddDependencyGraph();

var app = builder.Build();

app.AddCommands<ScanCommands>();

app.Run();