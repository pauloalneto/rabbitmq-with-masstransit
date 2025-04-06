using MassTransit;

namespace Publisher.Api.Events;

//[MessageUrn("email_sender")]
public record InvoiceEvent(Guid Id, decimal Value, DateTime CreatedAt);