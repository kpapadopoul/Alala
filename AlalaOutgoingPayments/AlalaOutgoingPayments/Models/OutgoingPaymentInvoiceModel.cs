using AlalaOutgoingPayments.Utilities;

namespace AlalaOutgoingPayments.Models
{
    public class OutgoingPaymentInvoiceModel
    {
        public int Entry { get; set; }
        public OutgoingPaymentUtility.OutgoingPaymentInvoiceTypes Type { get; set; }
    }
}
