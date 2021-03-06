﻿using System;
using System.Runtime.InteropServices;

using SAPbobsCOM;

using AlalaDiConnector.Interfaces;

using AlalaIncomingPayments.Models;
using AlalaIncomingPayments.Utilities;

namespace AlalaIncomingPayments.Controllers
{
    public class IncomingPayments : Interfaces.IIncomingPayments
    {
        private readonly Company _company;
        private readonly IncomingPaymentUtility _utility;

        /// <summary>
        /// Default constructor of incoming payments controller
        /// initializing company DI object.
        /// </summary>
        /// <param name="connection">An interface represents the DI connection
        /// to be used for initializing the DI company object.</param>
        public IncomingPayments(IDiConnection connection)
        {
            _company = connection.Company;
            _utility = new IncomingPaymentUtility();
        }

        /// <summary>
        /// Gets the details of an incoming payment.
        /// </summary>
        /// <param name="incomingPaymentEntry">The entry of the incoming payment to be returned.</param>
        /// <returns>A model that represents the incoming payment info.</returns>
        public IncomingPaymentModel GetById(int incomingPaymentEntry)
        {
            // Prepare the object
            var paymentObj = (Payments)_company.GetBusinessObject(BoObjectTypes.oIncomingPayments);
            
            // Find the record by its ID
            IncomingPaymentModel payment = null;
            if (paymentObj.GetByKey(incomingPaymentEntry))
            {
                payment = new IncomingPaymentModel();
                                
                payment.Entry = paymentObj.DocEntry;
                payment.BusinessPartner = paymentObj.CardCode;
                payment.DocDate = paymentObj.DocDate;
                payment.CashSum = paymentObj.CashSum;
                payment.CashAccount = paymentObj.CashAccount;

                // TODO: Add code to retrieve invoices corresponds to the payment.
            }

            Marshal.ReleaseComObject(paymentObj);
            return payment;
        }

        /// <summary>
        /// Creates an incoming payment to the database.
        /// </summary>
        /// <param name="incomingPayment">A model that contains the incoming payment info
        /// to be created.</param>
        public void Create(IncomingPaymentModel incomingPayment)
        {
            // Prepare the object
            var paymentObj = (Payments)_company.GetBusinessObject(BoObjectTypes.oIncomingPayments);

            // Set header values
            paymentObj.CardCode = incomingPayment.BusinessPartner;
            paymentObj.DocDate = DateTime.Now;
            paymentObj.CashSum = incomingPayment.CashSum;
            paymentObj.CashAccount = incomingPayment.CashAccount;

            // Set line values
            foreach (var invoice in incomingPayment.Invoices)
            {
                paymentObj.Invoices.DocEntry = invoice.Entry;
                paymentObj.Invoices.InvoiceType = _utility.ConvertIncomingPaymentInvoiceType(invoice.Type);
                paymentObj.Invoices.DocLine = incomingPayment.Invoices.IndexOf(invoice);
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
        /// Deletes an incoming payment from the database.
        /// </summary>
        /// <param name="incomingPaymentEntry">The entry of the incoming payment to be deleted.</param>
        /// <returns>A boolean value that is set to true whether the incoming payment
        /// found in the database.</returns>
        public bool Delete(int incomingPaymentEntry)
        {
            // Prepare the object
            var paymentObj = (Payments)_company.GetBusinessObject(BoObjectTypes.oIncomingPayments);

            // Find the record by its ID
            var paymentFound = false;
            if (paymentObj.GetByKey(incomingPaymentEntry))
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
