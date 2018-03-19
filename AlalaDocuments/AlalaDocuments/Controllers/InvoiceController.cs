using System;
using System.Linq;
using System.Runtime.InteropServices;

using SAPbobsCOM;

using AlalaDocuments.Models;

namespace AlalaDocuments.Controllers
{
    public class InvoiceController
    {
        private readonly Company _company;

        public InvoiceController(Company company)
        {
            _company = company;
        }

        public void CreateBasedOnOrder(InvoiceModel invoice, int orderId)
        {
            // Prepare the object
            var invoiceObj = (Documents)_company.GetBusinessObject(BoObjectTypes.oInvoices);

            // Set header values
            invoiceObj.CardCode = invoice.BusinessPartner;
            invoiceObj.DocDueDate = DateTime.Now;
            invoiceObj.BPL_IDAssignedToInvoice = 1;

            int count = 0;

            // Set line values
            foreach (var item in invoice.ItemList)
            {
                if (!(item == invoice.ItemList.First()))
                {
                    invoiceObj.Lines.Add();
                }

                invoiceObj.Lines.BaseType = (int)BoObjectTypes.oOrders;
                invoiceObj.Lines.BaseEntry = orderId;
                invoiceObj.Lines.BaseLine = count++;
                invoiceObj.Lines.TaxCode = item.TaxCode;
                invoiceObj.Lines.AccountCode = item.AccountCode;
            }

            // Add it to database
            var success = invoiceObj.Add().Equals(0);
            if (!success)
            {
                // Error handling
                int code;
                string msg;
                _company.GetLastError(out code, out msg);
                throw new Exception($"Something went wrong\n{code} {msg}");
            }

            Marshal.ReleaseComObject(invoiceObj);
        }
    }
}
