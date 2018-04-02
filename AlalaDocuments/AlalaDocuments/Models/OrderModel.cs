using System.Collections.Generic;

namespace AlalaDocuments.Models
{
    public class OrderModel
    {
        public int DocEntry { get; set; }
        public string BusinessPartner { get; set; }
        public List<ItemModel> ItemList { get; set; }
    }
}
