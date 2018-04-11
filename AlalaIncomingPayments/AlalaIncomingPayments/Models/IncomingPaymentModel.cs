using System;
using System.Collections.Generic;

namespace AlalaIncomingPayments.Models
{
    public class IncomingPaymentModel
    {
        public int Entry { get; set; }
        public string BusinessPartner { get; set; }
        public DateTime DocDate { get; set; }
        public double CashSum { get; set; }
        public string CashAccount { get; set; }
        public List<IncomingPaymentInvoiceModel> Invoices { get; set; }
    }
}
