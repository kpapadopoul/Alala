using AlalaIncomingPayments.Models;

namespace AlalaIncomingPayments.Interfaces
{
    public interface IIncomingPayments
    {
        /// <summary>
        /// Gets the details of an incoming payment.
        /// </summary>
        /// <param name="docEntry">The entry of the incoming payment to be returned.</param>
        /// <returns>A model that represents the incoming payment info.</returns>
        IncomingPaymentModel GetById(int incomingPaymentEntry);

        /// <summary>
        /// Creates an incoming payment to the database.
        /// </summary>
        /// <param name="invoice">A model that contains the incoming payment info
        /// to be created.</param>
        void Create(IncomingPaymentModel incomingPayment);

        /// <summary>
        /// Deletes an incoming payment from the database.
        /// </summary>
        /// <param name="incomingPaymentEntry">The entry of the incoming payment to be deleted.</param>
        /// <returns>A boolean value that is set to true whether the incoming payment
        /// found in the database.</returns>
        bool Delete(int incomingPaymentEntry);
    }
}
