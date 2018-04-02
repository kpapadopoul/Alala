using AlalaDocuments.Models;

namespace AlalaDocuments.Interfaces
{
    public interface IOrders
    {
        OrderModel GetById(int docEntry);
        void Create(OrderModel order);
        bool UpdateItems(int docEntry, OrderModel order);
        bool Delete(int docEntry);
    }
}
