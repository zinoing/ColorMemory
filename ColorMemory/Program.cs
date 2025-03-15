using ColorMemory.Data;
using ColorMemory.Repository.Implementations;
using ColorMemory.Services;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureHostOptions(options =>
{
    options.ShutdownTimeout = TimeSpan.FromSeconds(60);
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<WeeklyRankingDb>();
builder.Services.AddScoped<NationalRankingDb>();

builder.Services.AddScoped<ScoreService>();
builder.Services.AddScoped<ArtworkService>();
builder.Services.AddScoped<PlayerService>();

builder.Services.AddDbContext<GameDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 29))));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    // app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
