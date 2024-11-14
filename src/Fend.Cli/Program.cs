using Cocona;
using Fend.Cli;
using Fend.Cli.Commands;
using Fend.DependencyGraph;

var builder = CoconaApp.CreateBuilder(args);

builder.Services.AddCli();
builder.Services.AddDependencyGraph();

var app = builder.Build();

app.AddCommands<ScanCommand>();

app.Run();