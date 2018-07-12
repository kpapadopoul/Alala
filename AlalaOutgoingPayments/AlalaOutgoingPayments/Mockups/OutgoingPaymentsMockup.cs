using AlalaDiConnector.Interfaces;

using AlalaOutgoingPayments.Models;

namespace AlalaOutgoingPayments.Mockups
{
    public class OutgoingPaymentsMockup : Interfaces.IOutgoingPayments
    {
        /// <summary>
        /// Default constructor of outgoing payments mockup.
        /// </summary>
        /// <param name="connection">An interface to the DI connection.</param>
        public OutgoingPaymentsMockup(IDiConnection connection) { }

        /// <summary>
        /// Gets the details of an outgoing payment.
        /// </summary>
        /// <param name="outgoingPaymentEntry">The entry of the outgoing payment to be returned.</param>
        /// <returns>A model that represents the outgoing payment info.</returns>
        public OutgoingPaymentModel GetById(int outgoingPaymentEntry) { return new OutgoingPaymentModel(); }

        /// <summary>
        /// Creates an outgoing payment to the database.
        /// </summary>
        /// <param name="outgoingPayment">A model that contains the outgoing payment info
        /// to be created.</param>
        public void Create(OutgoingPaymentModel outgoingPayment) { }

        /// <summary>
        /// Deletes an outgoing payment from the database.
        /// </summary>
        /// <param name="outgoingPaymentEntry">The entry of the outgoing payment to be deleted.</param>
        /// <returns>A boolean value that is set to true whether the outgoing payment
        /// found in the database.</returns>
        public bool Delete(int outgoingPaymentEntry) { return true; }
    }
}
