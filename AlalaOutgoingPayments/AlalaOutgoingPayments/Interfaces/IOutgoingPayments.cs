using AlalaOutgoingPayments.Models;

namespace AlalaOutgoingPayments.Interfaces
{
    public interface IOutgoingPayments
    {
        OutgoingPaymentModel GetById(int outgoingPaymentEntry);
        void Create(OutgoingPaymentModel outgoingPayment);
        bool Delete(int outgoingPaymentEntry);
    }
}
