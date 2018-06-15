using AlalaDocuments.Models;

namespace AlalaDocuments.Interfaces
{
    public interface IOrders
    {
        /// <summary>
        /// Gets the details of an order.
        /// </summary>
        /// <param name="docEntry">The entry of the order to be returned.</param>
        /// <returns>A model that represents the order info.</returns>
        OrderModel GetById(int docEntry);

        /// <summary>
        /// Creates an order to the database.
        /// </summary>
        /// <param name="invoice">A model that contains the order info
        /// to be created.</param>
        void Create(OrderModel order);

        /// <summary>
        /// Updates items of a given order.
        /// </summary>
        /// <param name="docEntry">The entry of the order to be updated.</param>
        /// <param name="invoice">A model that contains the order items to be updated.</param>
        /// <returns>A boolean value that is set to true whether the order
        /// found in the database.</returns>
        bool UpdateItems(int docEntry, OrderModel order);

        /// <summary>
        /// Deletes an order from the database.
        /// </summary>
        /// <param name="docEntry">The entry of the order to be deleted.</param>
        /// <returns>A boolean value that is set to true whether the order
        /// found in the database.</returns>
        bool Delete(int docEntry);
    }
}
