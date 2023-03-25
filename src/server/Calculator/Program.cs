using Calculator.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices();

builder
    .Build()
    .ConfigureApp()
    .Run();