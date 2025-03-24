using LoggingService.Logic.Exceptions;
using LoggingService.Logic.Interfaces;
using LoggingService.Logic.Services;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var logFilePath = builder.Configuration.GetSection("LogSettings:LogFilePath").Value ?? "logs.txt";
builder.Services.AddSingleton<ILogService>(new TextFileLogService(new FileService(), logFilePath));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler(b =>
{
    b.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            InvalidRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
        var response = new { error = exception?.Message };
        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    });
});

app.UseAuthorization();

app.MapControllers();

app.Run();
