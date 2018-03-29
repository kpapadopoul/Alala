using AlalaDocuments.Models;

namespace AlalaDocuments.Interfaces
{
    public interface IInvoices
    {
        InvoiceModel GetById(int docEntry);
        void Create(InvoiceModel invoice);
        void CreateBasedOnOrder(InvoiceModel invoice, int orderId);
        bool UpdateItems(int docEntry, InvoiceModel invoice);
        bool Delete(int docEntry);
    }
}
