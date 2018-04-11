using System;

using SAPbobsCOM;

namespace AlalaIncomingPayments.Utilities
{
    public class IncomingPaymentUtility
    {
        public enum IncomingPaymentInvoiceTypes { Invoice, JournalEntry };
        
        public BoRcptInvTypes ConvertIncomingPaymentInvoiceType(IncomingPaymentInvoiceTypes invoiceType)
        {
            var invType = BoRcptInvTypes.it_AllTransactions;

            switch (invoiceType)
            {
                case IncomingPaymentInvoiceTypes.Invoice:
                    invType = BoRcptInvTypes.it_Invoice;
                    break;
                case IncomingPaymentInvoiceTypes.JournalEntry:
                    invType = BoRcptInvTypes.it_JournalEntry;
                    break;
                default:
                    throw new ArgumentException();
            }

            return invType;
        }
    }
}
