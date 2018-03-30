using System;
using System.Linq;
using System.Runtime.InteropServices;

using SAPbobsCOM;

using AlalaDiConnector.Controllers;
using AlalaDocuments.Models;

namespace AlalaDocuments.Controllers
{
    public class Invoices : Interfaces.IInvoices
    {
        private readonly Company _company;

        public Invoices(DiConnectionController connection)
        {
            _company = connection.Company;
        }

        public InvoiceModel GetById(int docEntry)
        {
            // Prepare the object
            var invoiceObj = (Documents)_company.GetBusinessObject(BoObjectTypes.oInvoices);

            InvoiceModel invoice = null;
            if (invoiceObj.GetByKey(docEntry))
            {
                invoice = new InvoiceModel();
                invoice.DocEntry = invoiceObj.DocEntry;
                invoice.BusinessPartner = invoiceObj.CardCode;

                // TODO: Add code to retrieve line data of the invoice.
            }

            Marshal.ReleaseComObject(invoiceObj);
            return invoice;
        }

        public void Create(InvoiceModel invoice)
        {
            // Prepare the object
            var invoiceObj = (Documents)_company.GetBusinessObject(BoObjectTypes.oInvoices);

            // Set header values            
            invoiceObj.CardCode = invoice.BusinessPartner;
            invoiceObj.DocDueDate = DateTime.Now;

            // Set line values
            foreach (var item in invoice.ItemList)
            {
                if (item != invoice.ItemList.First())
                {
                    invoiceObj.Lines.Add();
                }

                invoiceObj.Lines.ItemCode = item.ItemCode;
                invoiceObj.Lines.Quantity = item.Quantity;
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

        public void CreateBasedOnOrder(int orderId, InvoiceModel invoice)
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

        public bool UpdateItems(int docEntry, InvoiceModel invoice)
        {
            // Prepare the object
            var invoiceObj = (Documents)_company.GetBusinessObject(BoObjectTypes.oInvoices);

            var invoiceFound = false;
            if (invoiceObj.GetByKey(docEntry))
            {
                invoiceFound = true;

                foreach (var item in invoice.ItemList)
                {
                    if (invoiceObj.Lines.Count > 0)
                    {
                        invoiceObj.Lines.Add();
                    }

                    invoiceObj.Lines.ItemCode = item.ItemCode;
                    invoiceObj.Lines.Quantity = item.Quantity;
                    invoiceObj.Lines.TaxCode = item.TaxCode;
                    invoiceObj.Lines.AccountCode = item.AccountCode;
                }

                var success = invoiceObj.Update().Equals(0);
                if (!success)
                {
                    // Error handling
                    int code;
                    string msg;
                    _company.GetLastError(out code, out msg);
                    throw new Exception($"Something went wrong\n{code} {msg}");
                }
            }

            Marshal.ReleaseComObject(invoiceObj);
            return invoiceFound;
        }

        public bool Delete(int docEntry)
        {
            // Prepare the object
            var invoiceObj = (Documents)_company.GetBusinessObject(BoObjectTypes.oInvoices);

            var invoiceFound = false;
            if (invoiceObj.GetByKey(docEntry))
            {
                invoiceFound = true;

                // Remove it from database
                var success = invoiceObj.Remove().Equals(0);
                if (!success)
                {
                    // Error handling
                    int code;
                    string msg;
                    _company.GetLastError(out code, out msg);
                    throw new Exception($"Something went wrong\n{code} {msg}");
                }
            }

            Marshal.ReleaseComObject(invoiceObj);
            return invoiceFound;
        }
    }
}
