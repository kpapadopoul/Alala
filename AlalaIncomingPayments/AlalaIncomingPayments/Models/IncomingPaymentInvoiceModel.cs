using AlalaIncomingPayments.Utilities;

namespace AlalaIncomingPayments.Models
{
    public class IncomingPaymentInvoiceModel
    {
        public int Entry { get; set; }
        public IncomingPaymentUtility.IncomingPaymentInvoiceTypes Type { get; set; }
        //public int Line { get; set; }
    }
}
