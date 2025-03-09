using ColorMemory.Repository;
using Microsoft.AspNetCore.Http.Timeouts;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 서비스 등록 전에 Host 옵션 설정
builder.Host.ConfigureHostOptions(options =>
{
    options.ShutdownTimeout = TimeSpan.FromSeconds(60);
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<WeeklyRankingDb>();
builder.Services.AddScoped<NationalRankingDb>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
