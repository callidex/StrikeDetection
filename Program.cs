using Microsoft.EntityFrameworkCore;
using StrikeDetection.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LightningContext>(options => options.UseNpgsql("Host=b3.vk4ya.com;Database=lightning;Username=***;Password=***"));
builder.Services.AddScoped<IDbService, LightningContext>();
var app = builder.Build();


app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();