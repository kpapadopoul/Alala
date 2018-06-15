using System;
using System.Linq;
using System.Runtime.InteropServices;

using SAPbobsCOM;

using AlalaDiConnector.Controllers;
using AlalaDiConnector.Interfaces;
using AlalaDocuments.Models;

namespace AlalaDocuments.Controllers
{
    public class Orders : Interfaces.IOrders
    {
        private readonly Company _company;

        /// <summary>
        /// Default constructor of orders controller
        /// initializing company DI object.
        /// </summary>
        /// <param name="connection">An interface represents the DI connection
        /// to be used for initializing the DI company object.</param>
        public Orders(IDiConnection connection)
        {
            _company = connection.Company;
        }

        /// <summary>
        /// Gets the details of an order.
        /// </summary>
        /// <param name="docEntry">The entry of the order to be returned.</param>
        /// <returns>A model that represents the order info.</returns>
        public OrderModel GetById(int docEntry)
        {
            // Prepare the object
            var orderObj = (Documents)_company.GetBusinessObject(BoObjectTypes.oOrders);

            OrderModel order = null;
            if (orderObj.GetByKey(docEntry))
            {
                order = new OrderModel();
                order.DocEntry = orderObj.DocEntry;
                order.BusinessPartner = orderObj.CardCode;

                // TODO: Add code to retrieve line data of the order.
            }

            Marshal.ReleaseComObject(orderObj);
            return order;
        }

        /// <summary>
        /// Creates an order to the database.
        /// </summary>
        /// <param name="invoice">A model that contains the order info
        /// to be created.</param>
        public void Create(OrderModel order)
        {
            // Prepare the object
            var orderObj = (Documents)_company.GetBusinessObject(BoObjectTypes.oOrders);

            // Set header values
            orderObj.CardCode = order.BusinessPartner;
            orderObj.DocDueDate = DateTime.Now;
            orderObj.BPL_IDAssignedToInvoice = 1;

            // Set line values
            foreach (var item in order.Items)
            {
                if (item != order.Items.First())
                {
                    orderObj.Lines.Add();
                }

                orderObj.Lines.ItemCode = item.ItemCode;
                orderObj.Lines.Quantity = item.Quantity;
            }

            // Add it to database
            var success = orderObj.Add().Equals(0);
            if (!success)
            {
                // Error handling
                int code;
                string msg;
                _company.GetLastError(out code, out msg);
                throw new Exception($"Something went wrong\n{code} {msg}");
            }

            Marshal.ReleaseComObject(orderObj);
        }

        /// <summary>
        /// Updates items of a given order.
        /// </summary>
        /// <param name="docEntry">The entry of the order to be updated.</param>
        /// <param name="invoice">A model that contains the order items to be updated.</param>
        /// <returns>A boolean value that is set to true whether the order
        /// found in the database.</returns>
        public bool UpdateItems(int docEntry, OrderModel order)
        {
            // Prepare the object
            var orderObj = (Documents)_company.GetBusinessObject(BoObjectTypes.oOrders);

            var orderFound = false;
            if (orderObj.GetByKey(docEntry))
            {
                orderFound = true;

                foreach (var item in order.Items)
                {
                    if (orderObj.Lines.Count > 0)
                    {
                        orderObj.Lines.Add();
                    }

                    orderObj.Lines.ItemCode = item.ItemCode;
                    orderObj.Lines.Quantity = item.Quantity;
                    orderObj.Lines.TaxCode = item.TaxCode;
                    orderObj.Lines.AccountCode = item.AccountCode;
                }

                var success = orderObj.Update().Equals(0);
                if (!success)
                {
                    // Error handling
                    int code;
                    string msg;
                    _company.GetLastError(out code, out msg);
                    throw new Exception($"Something went wrong\n{code} {msg}");
                }
            }

            Marshal.ReleaseComObject(orderObj);
            return orderFound;
        }

        /// <summary>
        /// Deletes an order from the database.
        /// </summary>
        /// <param name="docEntry">The entry of the order to be deleted.</param>
        /// <returns>A boolean value that is set to true whether the order
        /// found in the database.</returns>
        public bool Delete(int docEntry)
        {
            // Prepare the object
            var orderObj = (Documents)_company.GetBusinessObject(BoObjectTypes.oOrders);

            var orderFound = false;
            if (orderObj.GetByKey(docEntry))
            {
                orderFound = true;

                // Remove it from database
                var success = orderObj.Remove().Equals(0);
                if (!success)
                {
                    // Error handling
                    int code;
                    string msg;
                    _company.GetLastError(out code, out msg);
                    throw new Exception($"Something went wrong\n{code} {msg}");
                }
            }

            Marshal.ReleaseComObject(orderObj);
            return orderFound;
        }
    }
}
