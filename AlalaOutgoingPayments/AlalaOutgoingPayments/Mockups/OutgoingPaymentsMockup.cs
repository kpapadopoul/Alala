using AlalaDiConnector.Interfaces;

using AlalaOutgoingPayments.Models;

namespace AlalaOutgoingPayments.Mockups
{
    public class OutgoingPaymentsMockup : Interfaces.IOutgoingPayments
    {
        public OutgoingPaymentsMockup(IDiConnection connection) { }

        public OutgoingPaymentModel GetById(int outgoingPaymentEntry) { return new OutgoingPaymentModel(); }
        public void Create(OutgoingPaymentModel outgoingPayment) { }
        public bool Delete(int outgoingPaymentEntry) { return false; }
    }
}
