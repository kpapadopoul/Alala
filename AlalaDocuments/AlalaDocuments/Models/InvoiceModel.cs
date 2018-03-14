using System.Collections.Generic;

namespace AlalaDocuments.Models
{
    public class InvoiceModel
    {
        public string BusinessPartner { get; set; }
        public List<ItemModel> ItemList { get; set; }
    }
}
