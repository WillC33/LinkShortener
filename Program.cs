using LinkShortener;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//BL services
builder.Services.AddScoped<Repository>();
builder.Services.AddScoped<ShortenerService>();
builder.Services.AddScoped<Coordinator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Gets the stored link
app.MapGet("/{hash}", (string hash, Coordinator coordinator) =>
    {
        var link = coordinator.ReadLink(hash);
        return link;
    })
    .WithName("Redirect")
    .WithOpenApi();

//Shortens a link
app.MapPost("/shorten", ([FromBody] string link, Coordinator coordinator) =>
    {
        var hash = coordinator.WriteLink(link);
        return hash;
    })
    .WithName("Shorten")
    .WithOpenApi();

app.Run();