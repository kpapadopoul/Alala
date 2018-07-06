using AlalaDocuments.Models;

namespace AlalaDocuments.Interfaces
{
    public interface IInvoices
    {
        /// <summary>
        /// Gets the details of an invoice.
        /// </summary>
        /// <param name="docEntry">The entry of the invoice to be returned.</param>
        /// <returns>A model that represents the invoice info.</returns>
        InvoiceModel GetById(int docEntry);

        /// <summary>
        /// Creates an invoice to the database.
        /// </summary>
        /// <param name="invoice">A model that contains the invoice info
        /// to be created.</param>
        void Create(InvoiceModel invoice);

        /// <summary>
        /// Creates an invoice based on a given previously created order.
        /// </summary>
        /// <param name="orderId">The ID of the order based on which the 
        /// invoice will be created.</param>
        /// <param name="invoice">A model that contains any additional invoice info
        /// to be created.</param>
        void CreateBasedOnOrder(int orderId, InvoiceModel invoice);

        /// <summary>
        /// Updates items of a given invoice.
        /// </summary>
        /// <param name="docEntry">The entry of the invoice to be updated.</param>
        /// <param name="invoice">A model that contains the invoice items to be updated.</param>
        /// <returns>A boolean value that is set to true whether the invoice
        /// found in the database.</returns>
        bool UpdateItems(int docEntry, InvoiceModel invoice);

        /// <summary>
        /// Deletes an invoice from the database.
        /// </summary>
        /// <param name="docEntry">The entry of the invoice to be deleted.</param>
        /// <returns>A boolean value that is set to true whether the invoice
        /// found in the database.</returns>
        bool Delete(int docEntry);
    }
}
