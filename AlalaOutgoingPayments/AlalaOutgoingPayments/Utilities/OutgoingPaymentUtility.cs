using System;

using SAPbobsCOM;

namespace AlalaOutgoingPayments.Utilities
{
    public class OutgoingPaymentUtility
    {
        public enum OutgoingPaymentInvoiceTypes { PurchaseInvoice, JournalEntry };

        public BoRcptInvTypes ConvertOutgoingPaymentInvoiceType(OutgoingPaymentInvoiceTypes invoiceType)
        {
            var invType = BoRcptInvTypes.it_AllTransactions;

            switch (invoiceType)
            {
                case OutgoingPaymentInvoiceTypes.PurchaseInvoice:
                    invType = BoRcptInvTypes.it_PurchaseInvoice;
                    break;
                case OutgoingPaymentInvoiceTypes.JournalEntry:
                    invType = BoRcptInvTypes.it_JournalEntry;
                    break;
                default:
                    throw new ArgumentException();
            }

            return invType;
        }
    }
}
