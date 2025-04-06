using System.Text.Json;
using ConsumerWorker.Events;
using MassTransit;

namespace ConsumerWorker.Consumer;

public class InvoiceCreatedConsumer : IConsumer<InvoiceEvent>
{
    public Task Consume(ConsumeContext<InvoiceEvent> context)
    {
        Console.WriteLine($"Consumer is working, message: {JsonSerializer.Serialize(context.Message)}");  ;
        Task.Delay(1000).Wait();
        return Task.CompletedTask;
    }
}