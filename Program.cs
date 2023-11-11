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
app.MapGet("/{hash}", (string hash, Coordinator coordinator, HttpContext context) =>
    {
        var link = coordinator.ReadLink(hash);
        //TODO: this does not redirect invalid links properly yet, it redirects to blank
        if (link == null) link = "";
        
        //uses 301 to indicate a permanent redirect to the client against the hashed url
        context.Response.StatusCode = StatusCodes.Status301MovedPermanently;
        context.Response.Headers.Location = link;

        return Task.CompletedTask;
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