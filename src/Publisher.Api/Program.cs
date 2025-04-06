using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Publisher.Api.Events;
using Publisher.Api.Interfaces;
using Publisher.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IBusService, BusService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

builder.Services.AddMassTransit(bus =>
{
    bus.UsingRabbitMq((context, cfg) =>
    {
        string host = builder.Configuration.GetSection("rabbitmq:host").Value ?? "";
        string virtualHost = builder.Configuration.GetSection("rabbitmq:virtualHost").Value ?? "";
        string user = builder.Configuration.GetSection("rabbitmq:user").Value ?? "";
        string password = builder.Configuration.GetSection("rabbitmq:password").Value ?? "";
        
        cfg.Host(host, virtualHost, h =>
        {
            h.Username(user);
            h.Password(password);
        });
        
        cfg.UseRawJsonSerializer();
        
        // cfg.Send<InvoiceEvent>(x => x.UseRoutingKeyFormatter(x => "invoice.created"));
        cfg.Message<InvoiceEvent>(x => x.SetEntityName("invoice"));
        cfg.Publish<InvoiceEvent>(x =>
        {
            x.Durable = true;
            x.AutoDelete = false;
            x.ExchangeType = "direct";
        });
        
        cfg.ConfigureEndpoints(context);
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("create-invoice", async ([FromBody] decimal value, IInvoiceService service) =>
{
    await service.CreateInvoice(value);
    return Results.Created();
});

app.Run();