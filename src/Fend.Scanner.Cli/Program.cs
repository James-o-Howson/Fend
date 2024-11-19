using Cocona;
using Fend.Core.Abstractions;
using Fend.Scanner.Cli;
using Fend.Scanner.Cli.Commands;
using Fend.Core.Infrastructure;
using Fend.Scanner.Infrastructure;

var builder = CoconaApp.CreateBuilder(args);

builder.Host.AddLogging();
builder.Services.AddCli();
builder.Services.AddCoreAbstractions();
builder.Services.AddInfrastructure();
builder.Services.AddDependencyGraph();

var app = builder.Build();

app.AddCommands<ScanCommands>();

app.Run();