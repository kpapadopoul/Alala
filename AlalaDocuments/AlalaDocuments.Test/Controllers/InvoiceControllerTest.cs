using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using Rhino.Mocks;

using SAPbobsCOM;

using AlalaDocuments.Controllers;
using AlalaDocuments.Models;

namespace AlalaDocuments.Test.Controllers
{
    [TestFixture]
    public class InvoiceControllerTest
    {
        InvoiceController _invoiceController;

        [SetUp]
        public void Init()
        {
            var company = MockRepository.GenerateMock<Company>();
            _invoiceController = new InvoiceController(company);
        }

        [Test]
        public void CreateBasedOnOrderTest()
        {
            // Arrange - nothing other than common setup
            const int orderId = 0;
            var invoiceModel = new InvoiceModel {
                BusinessPartner = "001",
                ItemList = new List<ItemModel>()
            };

            // Act
            _invoiceController.CreateBasedOnOrder(invoiceModel, orderId);

            // Assert
        }
    }
}
