using System.IO;
using System.Web;
using System.Web.Http;

using Newtonsoft.Json;

using AlalaDiConnector.Controllers;
using AlalaDiConnector.Interfaces;
using AlalaDiConnector.Mockups;
using AlalaDiConnector.Models;

using AlalaDocuments.Controllers;
using AlalaDocuments.Interfaces;
using AlalaDocuments.Mockups;
using AlalaDocuments.Models;

namespace AlalaDocumentsApi.Controllers
{
    [Route("api/Orders")]
    public class OrdersController : ApiController
    {
        private IDiConnection _connector;
        private IOrders _orders;

        /// <summary>
        /// The default constructor of the orders controller
        /// getting the DI connection configuration, initializing interfaces
        /// and connecting to ERP.
        /// </summary>
        public OrdersController()
        {
            // Get connection details from configuration file.
            var confPath = Path.Combine(
                    HttpRuntime.AppDomainAppPath,
                    "Configuration");

            var connectionPath = File.ReadAllText(
                Path.Combine(
                    confPath,
                    "AlalaDocuments.conf"));

            var connection = JsonConvert.DeserializeObject<DiConnectionModel>(connectionPath);
            var passwordPath = Path.Combine(
                    confPath,
                    "AlalaDocuments.dat");

            _connector = new DiConnectionMockup(connection, passwordPath); // TODO: Turn this to the actual controller for integration testing.

            _connector.Connect();

            _orders = new OrdersMockup(_connector); // TODO: Turn this to the actual controller for integration testing.
        }

        /// <summary>
        /// The default destructor of the orders controller
        /// disconnecting from the ERP.
        /// </summary>
        ~OrdersController()
        {
            _connector.Disconnect();
        }

        /// <summary>
        /// An HTTP interface that retrieves an invoice details
        /// given its ID.
        /// </summary>
        /// <param name="docEntry">The ID of the invoice the details of
        /// which are to be retrieved.</param>
        /// <returns>An HTTP action result represents the HTTP response including 
        /// the invoice details.</returns>
        [HttpGet, Route("GetOrderById", Name = "GetOrderById")]
        public IHttpActionResult GetById(int docEntry)
        {
            var order = _orders.GetById(docEntry);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        /// <summary>
        /// An HTTP request that creates a new order to the
        /// database.
        /// </summary>
        /// <param name="order">A model that represents the order
        /// is to be created.</param>
        /// <returns>An HTTP action result represents the HTTP response (i.e., success or failure
        /// of the actual event).</returns>
        [HttpPost, Route("CreateOrder")]
        public IHttpActionResult Create([FromBody]OrderModel order)
        {
            if (order == null)
            {
                return BadRequest();
            }

            _orders.Create(order);

            return Ok();
        }

        /// <summary>
        ///  HTTP request that updates items of a given order.
        /// </summary>
        /// <param name="docEntry">The ID of the order to be updated.</param>
        /// <param name="order">The order model that is to be used as 
        /// input for the items to be updated.</param>
        /// <returns>An HTTP action result represents the HTTP response (i.e., success or failure
        /// of the actual event).</returns>
        [HttpPut, Route("UpdateOrderItems")]
        public IHttpActionResult UpdateItems(int docEntry, [FromBody]OrderModel order)
        {
            if (order == null ||
                order.DocEntry != docEntry)
            {
                return BadRequest();
            }

            var orderFound = _orders.UpdateItems(docEntry, order);
            if (!orderFound)
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        /// An HTTP request that deletes an order from the database.
        /// </summary>
        /// <param name="docEntry">The ID of the order is to be deleted.</param>
        /// <returns>An HTTP action result represents the HTTP response (i.e., success or failure
        /// of the actual event).</returns>
        [HttpDelete, Route("DeleteOrder")]
        public IHttpActionResult Delete(int docEntry)
        {
            var orderFound = _orders.Delete(docEntry);
            if (!orderFound)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
