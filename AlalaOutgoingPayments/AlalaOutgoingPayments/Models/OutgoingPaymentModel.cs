using System;
using System.Collections.Generic;

namespace AlalaOutgoingPayments.Models
{
    public class OutgoingPaymentModel
    {
        public int Entry { get; set; }
        public string BusinessPartner { get; set; }
        public DateTime DocDate { get; set; }
        public string TransferAccount { get; set; }
        public DateTime TransferDate { get; set; }
        public double TransferSum { get; set; }
        public List<AccountPaymentModel> AccountPayments { get; set; }
        public List<OutgoingPaymentInvoiceModel> Invoices { get; set; }
    }
}
