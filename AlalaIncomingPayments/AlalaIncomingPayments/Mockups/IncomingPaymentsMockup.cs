using AlalaDiConnector.Interfaces;

using AlalaIncomingPayments.Models;

namespace AlalaIncomingPayments.Mockups
{
    public class IncomingPaymentsMockup : Interfaces.IIncomingPayments
    {
        /// <summary>
        /// Default constructor of incoming payments mockup.
        /// </summary>
        /// <param name="connection">An interface to the DI connection.</param>
        public IncomingPaymentsMockup(IDiConnection connection) { }

        /// <summary>
        /// Gets the details of an incoming payment.
        /// </summary>
        /// <param name="docEntry">The entry of the incoming payment to be returned.</param>
        /// <returns>A model that represents the incoming payment info.</returns>
        public IncomingPaymentModel GetById(int incomingPaymentEntry) { return new IncomingPaymentModel(); }

        /// <summary>
        /// Creates an incoming payment to the database.
        /// </summary>
        /// <param name="invoice">A model that contains the incoming payment info
        /// to be created.</param>
        public void Create(IncomingPaymentModel incomingPayment) { }

        /// <summary>
        /// Deletes an incoming payment from the database.
        /// </summary>
        /// <param name="docEntry">The entry of the incoming payment to be deleted.</param>
        /// <returns>A boolean value that is set to true whether the incoming payment
        /// found in the database.</returns>
        public bool Delete(int incomingPaymentEntry) { return true; }
    }
}
