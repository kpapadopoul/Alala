using AlalaDiConnector.Interfaces;

using AlalaIncomingPayments.Models;

namespace AlalaIncomingPayments.Mockups
{
    public class IncomingPaymentsMockup : Interfaces.IIncomingPayments
    {
        public IncomingPaymentsMockup(IDiConnection connection) { }

        public IncomingPaymentModel GetById(int incomingPaymentEntry) { return new IncomingPaymentModel(); }
        public void Create(IncomingPaymentModel incomingPayment) { }
        public bool Delete(int incomingPaymentEntry) { return false; }
    }
}
