using AlalaOutgoingPayments.Models;

namespace AlalaOutgoingPayments.Interfaces
{
    public interface IOutgoingPayments
    {
        /// <summary>
        /// Gets the details of an outgoing payment.
        /// </summary>
        /// <param name="outgoingPaymentEntry">The entry of the outgoing payment to be returned.</param>
        /// <returns>A model that represents the outgoing payment info.</returns>
        OutgoingPaymentModel GetById(int outgoingPaymentEntry);

        /// <summary>
        /// Creates an outgoing payment to the database.
        /// </summary>
        /// <param name="outgoingPayment">A model that contains the outgoing payment info
        /// to be created.</param>
        void Create(OutgoingPaymentModel outgoingPayment);

        /// <summary>
        /// Deletes an outgoing payment from the database.
        /// </summary>
        /// <param name="outgoingPaymentEntry">The entry of the outgoing payment to be deleted.</param>
        /// <returns>A boolean value that is set to true whether the outgoing payment
        /// found in the database.</returns>
        bool Delete(int outgoingPaymentEntry);
    }
}
