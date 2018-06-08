using AlalaDocuments.Models;

namespace AlalaDocuments.Mockups
{
    public class InvoicesMockup : Interfaces.IInvoices
    {
        public InvoiceModel GetById(int docEntry) { return new InvoiceModel(); }
        public void Create(InvoiceModel invoice) { }
        public void CreateBasedOnOrder(int orderId, InvoiceModel invoice) { }
        public bool UpdateItems(int docEntry, InvoiceModel invoice) { return false; }
        public bool Delete(int docEntry) { return false; }
    }
}
