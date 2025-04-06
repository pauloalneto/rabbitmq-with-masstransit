using ConsumerWorker;
using ConsumerWorker.Consumer;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(x =>
{
    string host = builder.Configuration.GetSection("rabbitmq:host").Value ?? "";
    string virtualHost = builder.Configuration.GetSection("rabbitmq:virtualHost").Value ?? "";
    string user = builder.Configuration.GetSection("rabbitmq:user").Value ?? "";
    string password = builder.Configuration.GetSection("rabbitmq:password").Value ?? "";
    string exchange = builder.Configuration.GetSection("rabbitmq:exchange").Value ?? "";
    
    x.AddConsumer<InvoiceCreatedConsumer>();
            
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(host, virtualHost, host =>
        {
            host.Username(user);
            host.Password(password);
        });
                
        cfg.UseRawJsonSerializer();
                
        cfg.ReceiveEndpoint("email_sender", e =>
        {
            e.ConfigureConsumeTopology = false; // Evita recriação automática
            //e.DiscardSkippedMessages();
                    
            e.Bind(exchange, x =>
            {
                x.ExchangeType = "direct";
                x.RoutingKey = "invoice.created";
            });

            e.ConfigureConsumer<InvoiceCreatedConsumer>(context);
        });
                
        cfg.ConfigureEndpoints(context);
    });
});

var host = builder.Build();
host.Run();