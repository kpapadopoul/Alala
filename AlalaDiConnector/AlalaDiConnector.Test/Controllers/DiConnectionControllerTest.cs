using System;

using NUnit.Framework;
using Rhino.Mocks;

using SAPbobsCOM;

using AlalaDiConnector.Controllers;

namespace AlalaDiConnector.Test.Controllers
{
    [TestFixture]
    public class DiConnectionControllerTest
    {
        DiConnectionController _connectionController;

        [SetUp]
        public void Init()
        {
            // Arrange
            _connectionController = new DiConnectionController();
            _connectionController.Company = MockRepository.GenerateMock<Company>();
        }

        [Test]
        public void TestConnect()
        {
            // Arrange - nothing other than common setup

            try
            {
                // Act
                _connectionController.Connect();
            }
            catch (Exception ex)
            {
                // Assert
                Assert.Fail($"Expected no exception, but got: {ex.Message}");
            }
        }

        [Test]
        public void TestDisconnect()
        {
            // Arrange - nothing other than common setup

            // Act
            _connectionController.Disconnect();
        }
    }
}
