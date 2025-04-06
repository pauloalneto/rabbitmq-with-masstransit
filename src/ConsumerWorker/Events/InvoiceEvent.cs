using MassTransit;

namespace ConsumerWorker.Events;

[MessageUrn("email_sender")]
public record InvoiceEvent(Guid Id, decimal Value, DateTime CreatedAt);