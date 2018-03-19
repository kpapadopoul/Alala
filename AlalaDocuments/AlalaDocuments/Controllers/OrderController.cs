using System;
using System.Linq;
using System.Runtime.InteropServices;

using SAPbobsCOM;

using AlalaDocuments.Models;

namespace AlalaDocuments.Controllers
{
    public class OrderController
    {
        private readonly Company _company;

        public OrderController(Company company)
        {
            _company = company;
        }

        public void Create(OrderModel order)
        {
            // Prepare the object
            var orderObj = (Documents)_company.GetBusinessObject(BoObjectTypes.oOrders);

            // Set header values
            orderObj.CardCode = order.BusinessPartner;
            orderObj.DocDueDate = DateTime.Now;
            orderObj.BPL_IDAssignedToInvoice = 1;

            // Set line values
            foreach (var item in order.ItemList)
            {
                if (!(item == order.ItemList.First()))
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
    }
}
