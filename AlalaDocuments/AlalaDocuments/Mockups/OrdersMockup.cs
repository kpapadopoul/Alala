using AlalaDocuments.Models;

namespace AlalaDocuments.Mockups
{
    public class OrdersMockup : Interfaces.IOrders
    {
        public OrderModel GetById(int docEntry) { return new OrderModel(); }
        public void Create(OrderModel order) { }
        public bool UpdateItems(int docEntry, OrderModel order) { return false; }
        public bool Delete(int docEntry) { return false; }
    }
}
