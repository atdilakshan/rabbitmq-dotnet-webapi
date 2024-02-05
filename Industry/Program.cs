using Common.EventBus.Events;
using Common.EventBus.Services;
using Industry.Events.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// service
builder.Services.AddSingleton<IEventBus, EventBus>();
builder.Services.AddTransient<EventTestHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// eventBus
var eventBus = app.Services.GetRequiredService<IEventBus>();

// Subscribe via queue
//eventBus.SubscribeViaQueue<TestEvent, EventTestHandler>();
eventBus.SubscribeViaTopic<TestEvent, EventTestHandler>();
//eventBus.SubscribeViaFanout<TestEvent, EventTestHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
