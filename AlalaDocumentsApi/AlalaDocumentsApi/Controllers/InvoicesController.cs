using System.Web.Http;

using AlalaDiConnector.Controllers;
using AlalaDiConnector.Models;
using AlalaDocuments.Controllers;
using AlalaDocuments.Interfaces;
using AlalaDocuments.Models;

using AlalaDocumentsApi.Resources;

namespace AlalaDocumentsApi.Controllers
{
    [Route("api/Invoices")]
    public class InvoicesController : ApiController
    {
        private DiConnectionController _connector;
        private IInvoices _invoices;

        public InvoicesController()
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

            _invoices = new Invoices(_connector);
        }

        ~InvoicesController()
        {
            // TODO: Uncomment the following line when
            // SAP testbed is ready to test
            //_connector.Disconnect();
        }

        [HttpGet, Route("GetInvoiceById", Name = "GetInvoiceById")]
        public IHttpActionResult GetById(int docEntry)
        {
            var invoice = _invoices.GetById(docEntry);

            if (invoice == null)
            {
                return NotFound();
            }

            return Ok(invoice);
        }

        [HttpPost, Route("CreateInvoice")]
        public IHttpActionResult Create([FromBody]InvoiceModel invoice)
        {
            if (invoice == null)
            {
                return BadRequest();
            }

            _invoices.Create(invoice);

            return Ok();
        }

        [HttpPost, Route("CreateInvoiceBasedOnOrder")]
        public IHttpActionResult CreateBasedOnOrder(int orderId, [FromBody]InvoiceModel invoice)
        {
            if (invoice == null)
            {
                return BadRequest();
            }

            _invoices.CreateBasedOnOrder(orderId, invoice);

            return Ok();
        }

        [HttpPut, Route("UpdateInvoiceItems")]
        public IHttpActionResult UpdateItems(int docEntry, [FromBody]InvoiceModel invoice)
        {
            if (invoice == null ||
                invoice.DocEntry != docEntry)
            {
                return BadRequest();
            }

            var invFound = _invoices.UpdateItems(docEntry, invoice);
            if (!invFound)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete, Route("DeleteInvoice")]
        public IHttpActionResult Delete(int docEntry)
        {
            var invFound = _invoices.Delete(docEntry);
            if (!invFound)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
