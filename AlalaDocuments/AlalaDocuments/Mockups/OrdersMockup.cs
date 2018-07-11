using AlalaDiConnector.Interfaces;

using AlalaDocuments.Models;

namespace AlalaDocuments.Mockups
{
    public class OrdersMockup : Interfaces.IOrders
    {
        /// <summary>
        /// Default constructor of orders mockup.
        /// </summary>
        /// <param name="connection">An interface to the DI connection.</param>
        public OrdersMockup(IDiConnection connection) { }

        /// <summary>
        /// Gets the details of an order.
        /// </summary>
        /// <param name="docEntry">The entry of the order to be returned.</param>
        /// <returns>A model that represents the order info.</returns>
        public OrderModel GetById(int docEntry) { return new OrderModel(); }

        /// <summary>
        /// Creates an order to the database.
        /// </summary>
        /// <param name="invoice">A model that contains the order info
        /// to be created.</param>
        public void Create(OrderModel order) { }

        /// <summary>
        /// Updates items of a given order.
        /// </summary>
        /// <param name="docEntry">The entry of the order to be updated.</param>
        /// <param name="invoice">A model that contains the order items to be updated.</param>
        /// <returns>A boolean value that is set to true whether the order
        /// found in the database.</returns>
        public bool UpdateItems(int docEntry, OrderModel order) { return true; }

        /// <summary>
        /// Deletes an order from the database.
        /// </summary>
        /// <param name="docEntry">The entry of the order to be deleted.</param>
        /// <returns>A boolean value that is set to true whether the order
        /// found in the database.</returns>
        public bool Delete(int docEntry) { return true; }
    }
}
