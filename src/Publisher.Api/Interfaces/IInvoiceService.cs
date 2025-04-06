namespace Publisher.Api.Interfaces;

public interface IInvoiceService
{
    Task<Guid> CreateInvoice(decimal value);
}