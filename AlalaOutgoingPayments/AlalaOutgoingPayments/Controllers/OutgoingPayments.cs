﻿using System;
using System.Runtime.InteropServices;

using SAPbobsCOM;

using AlalaDiConnector.Interfaces;

using AlalaOutgoingPayments.Models;
using AlalaOutgoingPayments.Utilities;

namespace AlalaOutgoingPayments.Controllers
{
    public class OutgoingPayments : Interfaces.IOutgoingPayments
    {
        private readonly Company _company;
        private readonly OutgoingPaymentUtility _utility;

        /// <summary>
        /// Default constructor of incoming payments controller
        /// initializing company DI object.
        /// </summary>
        /// <param name="connection">An interface represents the DI connection
        /// to be used for initializing the DI company object.</param>
        public OutgoingPayments(IDiConnection connection)
        {
            _company = connection.Company;
            _utility = new OutgoingPaymentUtility();
        }

        /// <summary>
        /// Gets the details of an outgoing payment.
        /// </summary>
        /// <param name="outgoingPaymentEntry">The entry of the outgoing payment to be returned.</param>
        /// <returns>A model that represents the outgoing payment info.</returns>
        public OutgoingPaymentModel GetById(int outgoingPaymentEntry)
        {
            // Prepare the object
            var paymentObj = (Payments)_company.GetBusinessObject(BoObjectTypes.oVendorPayments);

            // Find the record by its ID
            OutgoingPaymentModel payment = null;
            if (paymentObj.GetByKey(outgoingPaymentEntry))
            {
                payment = new OutgoingPaymentModel();

                payment.Entry = paymentObj.DocEntry;
                payment.BusinessPartner = paymentObj.CardCode;
                payment.DocDate = paymentObj.DocDate;
                payment.TransferAccount = paymentObj.TransferAccount;
                payment.TransferDate = paymentObj.TransferDate;
                payment.TransferSum = paymentObj.TransferSum;

                // TODO: Add code to retrieve account payments.
                // TODO: Add code to retrieve invoices corresponds to the payment.
            }

            Marshal.ReleaseComObject(paymentObj);
            return payment;
        }

        /// <summary>
        /// Creates an outgoing payment to the database.
        /// </summary>
        /// <param name="outgoingPayment">A model that contains the outgoing payment info
        /// to be created.</param>
        public void Create(OutgoingPaymentModel outgoingPayment)
        {
            // Prepare the object
            var paymentObj = (Payments)_company.GetBusinessObject(BoObjectTypes.oVendorPayments);

            // Set header values
            paymentObj.CardCode = outgoingPayment.BusinessPartner;
            paymentObj.DocObjectCode = BoPaymentsObjectType.bopot_OutgoingPayments;
            paymentObj.DocType = BoRcptTypes.rAccount;
            paymentObj.DocTypte = BoRcptTypes.rAccount;
            paymentObj.DocDate = DateTime.Now;
            paymentObj.TransferAccount = outgoingPayment.TransferAccount;
            paymentObj.TransferDate = DateTime.Now;
            paymentObj.TransferSum = outgoingPayment.TransferSum;

            // Add account payments
            foreach (var accountPayment in outgoingPayment.AccountPayments)
            {
                paymentObj.AccountPayments.AccountCode = accountPayment.AccountCode;
                paymentObj.AccountPayments.SumPaid = accountPayment.SumPaid;
                paymentObj.AccountPayments.Add();
            }

            // Add invoices
            foreach (var invoice in outgoingPayment.Invoices)
            {
                paymentObj.Invoices.DocEntry = invoice.Entry;
                paymentObj.Invoices.InvoiceType = _utility.ConvertOutgoingPaymentInvoiceType(invoice.Type);
                paymentObj.Invoices.DocLine = outgoingPayment.Invoices.IndexOf(invoice);
                paymentObj.Invoices.Add();
            }

            // Add it to database
            var success = paymentObj.Add().Equals(0);
            if (!success)
            {
                // Error handling
                int code;
                string msg;
                _company.GetLastError(out code, out msg);
                throw new Exception($"Something went wrong\n{code} {msg}");
            }

            Marshal.ReleaseComObject(paymentObj);
        }

        /// <summary>
        /// Deletes an outgoing payment from the database.
        /// </summary>
        /// <param name="outgoingPaymentEntry">The entry of the outgoing payment to be deleted.</param>
        /// <returns>A boolean value that is set to true whether the outgoing payment
        /// found in the database.</returns>
        public bool Delete(int outgoingPaymentEntry)
        {
            // Prepare the object
            var paymentObj = (Payments)_company.GetBusinessObject(BoObjectTypes.oVendorPayments);

            // Find the record by its ID
            var paymentFound = false;
            if (paymentObj.GetByKey(outgoingPaymentEntry))
            {
                paymentFound = true;

                // Remove it from database
                var success = paymentObj.Remove().Equals(0);
                if (!success)
                {
                    // Error handling
                    int code;
                    string msg;
                    _company.GetLastError(out code, out msg);
                    throw new Exception($"Something went wrong\n{code} {msg}");
                }
            }

            Marshal.ReleaseComObject(paymentObj);
            return paymentFound;
        }
    }
}
