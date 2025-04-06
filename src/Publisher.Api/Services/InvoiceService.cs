using Publisher.Api.Events;
using Publisher.Api.Interfaces;

namespace Publisher.Api.Services;

public class InvoiceService(IBusService busService) : IInvoiceService
{
    public async Task<Guid> CreateInvoice(decimal value)
    {
        var invoiceId = Guid.NewGuid();

        await busService.PublishAsync(new InvoiceEvent(invoiceId, 1550.50m, DateTime.Now), "invoice.created");
        
        return invoiceId;
    }
}