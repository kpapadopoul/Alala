using System;
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
    [RoutePrefix("api/Invoices")]
    public class InvoicesController : ApiController
    {
        private IDiConnection _connector;
        private IInvoices _invoices;

        /// <summary>
        /// The default constructor of the invoices controller
        /// getting the DI connection configuration, initializing interfaces
        /// and connecting to ERP.
        /// </summary>
        public InvoicesController()
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

            _invoices = new InvoicesMockup(_connector); // TODO: Turn this to the actual controller for integration testing.
        }

        /// <summary>
        /// The default destructor of the invoices controller
        /// disconnecting from the ERP.
        /// </summary>
        ~InvoicesController()
        {
            _connector.Disconnect();
        }

        /// <summary>
        /// An HTTP interface that retrieves an invoice details
        /// given its ID.
        /// </summary>
        /// <param name="id">The ID of the invoice the details of
        /// which are to be retrieved.</param>
        /// <returns>An HTTP action result represents the HTTP response including 
        /// the invoice details.</returns>
        [HttpGet, Route("GetInvoiceById", Name = "GetInvoiceById")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var invoice = _invoices.GetById(id);

                if (invoice == null)
                {
                    return NotFound();
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// An HTTP request that creates a new invoice to the
        /// database.
        /// </summary>
        /// <param name="invoice">A model that represents the invoice
        /// is to be created.</param>
        /// <returns>An HTTP action result represents the HTTP response (i.e., success or failure
        /// of the actual event).</returns>
        [HttpPost, Route("CreateInvoice")]
        public IHttpActionResult Create([FromBody]InvoiceModel invoice)
        {
            if (invoice == null)
            {
                return BadRequest();
            }

            try
            {
                _invoices.Create(invoice);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// An HTTP request that creates a new invoice to the
        /// database based on a given order.
        /// </summary>
        /// <param name="orderId">The ID of the order based on which
        /// the invoice is to be created.</param>
        /// <param name="invoice">A model that represents the invoice
        /// is to be created.</param>
        /// <returns>An HTTP action result represents the HTTP response (i.e., success or failure
        /// of the actual event).</returns>
        [HttpPost, Route("CreateInvoiceBasedOnOrder")]
        public IHttpActionResult CreateBasedOnOrder(int orderId, [FromBody]InvoiceModel invoice)
        {
            if (invoice == null)
            {
                return BadRequest();
            }

            try
            {
                _invoices.CreateBasedOnOrder(orderId, invoice);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// An HTTP request that updates items of a given invoice.
        /// </summary>
        /// <param name="id">The ID of the invoice to be updated.</param>
        /// <param name="invoice">The invoice model that is to be used as 
        /// input for the items to be updated.</param>
        /// <returns>An HTTP action result represents the HTTP response (i.e., success or failure
        /// of the actual event).</returns>
        [HttpPut, Route("UpdateInvoiceItems")]
        public IHttpActionResult UpdateItems(int id, [FromBody]InvoiceModel invoice)
        {
            if (invoice == null ||
                invoice.DocEntry != id)
            {
                return BadRequest();
            }

            try
            {
                var invFound = _invoices.UpdateItems(id, invoice);
                if (!invFound)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// An HTTP request that deletes an invoice from the database.
        /// </summary>
        /// <param name="id">The ID of the invoice is to be deleted.</param>
        /// <returns>An HTTP action result represents the HTTP response (i.e., success or failure
        /// of the actual event).</returns>
        [HttpDelete, Route("DeleteInvoice")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var invFound = _invoices.Delete(id);
                if (!invFound)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
