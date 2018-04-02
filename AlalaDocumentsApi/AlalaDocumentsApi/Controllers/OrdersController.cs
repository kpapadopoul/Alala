using System.Web.Http;

using AlalaDiConnector.Controllers;
using AlalaDiConnector.Models;
using AlalaDocuments.Controllers;
using AlalaDocuments.Interfaces;
using AlalaDocuments.Models;

using AlalaDocumentsApi.Resources;

namespace AlalaDocumentsApi.Controllers
{
    [Route("api/Orders")]
    public class OrdersController : ApiController
    {
        private DiConnectionController _connector;
        private IOrders _orders;

        public OrdersController()
        {
            // TODO: Update controller to get connection details
            // from XML file.
            _connector = new DiConnectionController(
                new DiConnectionModel
                {
                    Server = ConnectionDetails.Server,
                    CompanyDB = ConnectionDetails.CompanyDB,
                    Username = ConnectionDetails.Username,
                    Password = ConnectionDetails.Password
                });

            // TODO: Uncomment the following line when
            // SAP testbed is ready to test
            //_connector.Connect();

            _orders = new Orders(_connector);
        }

        ~OrdersController()
        {
            // TODO: Uncomment the following line when
            // SAP testbed is ready to test
            //_connector.Disconnect();
        }

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
