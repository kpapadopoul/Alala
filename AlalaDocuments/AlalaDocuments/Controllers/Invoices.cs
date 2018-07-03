using System;
using System.Linq;
using System.Runtime.InteropServices;

using SAPbobsCOM;

using AlalaDiConnector.Interfaces;

using AlalaDocuments.Models;

namespace AlalaDocuments.Controllers
{
    public class Invoices : Interfaces.IInvoices
    {
        private readonly Company _company;

        /// <summary>
        /// Default constructor of invoices controller
        /// initializing company DI object.
        /// </summary>
        /// <param name="connection">An interface represents the DI connection
        /// to be used for initializing the DI company object.</param>
        public Invoices(IDiConnection connection)
        {
            _company = connection.Company;
        }

        /// <summary>
        /// Gets the details of an invoice.
        /// </summary>
        /// <param name="docEntry">The entry of the invoice to be returned.</param>
        /// <returns>A model that represents the invoice info.</returns>
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

        /// <summary>
        /// Creates an invoice to the database.
        /// </summary>
        /// <param name="invoice">A model that contains the invoice info
        /// to be created.</param>
        public void Create(InvoiceModel invoice)
        {
            // Prepare the object
            var invoiceObj = (Documents)_company.GetBusinessObject(BoObjectTypes.oInvoices);

            // Set header values            
            invoiceObj.CardCode = invoice.BusinessPartner;
            invoiceObj.DocDueDate = DateTime.Now;

            // Set line values
            foreach (var item in invoice.Items)
            {
                if (item != invoice.Items.First())
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

        /// <summary>
        /// Creates an invoice based on a given previously created order.
        /// </summary>
        /// <param name="orderId">The ID of the order based on which the 
        /// invoice will be created.</param>
        /// <param name="invoice">A model that contains any additional invoice info
        /// to be created.</param>
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
            foreach (var item in invoice.Items)
            {
                if (!(item == invoice.Items.First()))
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

        /// <summary>
        /// Updates items of a given invoice.
        /// </summary>
        /// <param name="docEntry">The entry of the invoice to be updated.</param>
        /// <param name="invoice">A model that contains the invoice items to be updated.</param>
        /// <returns>A boolean value that is set to true whether the invoice
        /// found in the database.</returns>
        public bool UpdateItems(int docEntry, InvoiceModel invoice)
        {
            // Prepare the object
            var invoiceObj = (Documents)_company.GetBusinessObject(BoObjectTypes.oInvoices);

            var invoiceFound = false;
            if (invoiceObj.GetByKey(docEntry))
            {
                invoiceFound = true;

                foreach (var item in invoice.Items)
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

        /// <summary>
        /// Deletes an invoice from the database.
        /// </summary>
        /// <param name="docEntry">The entry of the invoice to be deleted.</param>
        /// <returns>A boolean value that is set to true whether the invoice
        /// found in the database.</returns>
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
