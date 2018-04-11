using System.Collections.Generic;

namespace AlalaDocuments.Models
{
    public class InvoiceModel
    {
        public int DocEntry { get; set; }
        public string BusinessPartner { get; set; }
        public List<ItemModel> Items { get; set; }
    }
}
