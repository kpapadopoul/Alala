using System.Collections.Generic;

using NUnit.Framework;
using Rhino.Mocks;

using AlalaDiConnector.Controllers;

using AlalaDocuments.Controllers;
using AlalaDocuments.Models;

namespace AlalaDocuments.Test.Controllers
{
    [TestFixture]
    public class InvoiceControllerTest
    {
        Invoices _invoiceController;

        [SetUp]
        public void Init()
        {
            var connection = MockRepository.GenerateMock<DiConnectionController>();
            _invoiceController = new Invoices(connection);
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
            _invoiceController.CreateBasedOnOrder(orderId, invoiceModel);

            // Assert
        }
    }
}
