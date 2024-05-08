using Errow.Ticketing.EventPlacementApi.Actors;
using Errow.Ticketing.EventPlacementApi.Interfaces;
using Errow.Ticketing.EventPlacementApi.Services;
using Errow.Ticketing.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddDaprClient();

builder.Services.AddActors(options =>
{
    options.Actors.RegisterActor<EventPlacementActor>();
});
builder.Services.AddTransient<IEventPlacementService, EventPlacementService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(static builder => 
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());

app.MapControllers().WithOpenApi();

app.MapActorsHandlers();

app.Run();