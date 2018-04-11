using AlalaIncomingPayments.Models;

namespace AlalaIncomingPayments.Interfaces
{
    public interface IIncomingPayments
    {
        IncomingPaymentModel GetById(int incomingPaymentEntry);
        void Create(IncomingPaymentModel incomingPayment);
        bool Delete(int incomingPaymentEntry);
    }
}
