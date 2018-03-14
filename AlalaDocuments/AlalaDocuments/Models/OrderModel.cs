using System.Collections.Generic;

namespace AlalaDocuments.Models
{
    public class OrderModel
    {
        public string BusinessPartner { get; set; }
        public List<ItemModel> ItemList { get; set; }
    }
}
