using System;
using System.IO;
using System.Web;
using System.Web.Http;

using Newtonsoft.Json;

using AlalaDiConnector.Controllers;
using AlalaDiConnector.Interfaces;
using AlalaDiConnector.Mockups;
using AlalaDiConnector.Models;

using AlalaIncomingPayments.Controllers;
using AlalaIncomingPayments.Interfaces;
using AlalaIncomingPayments.Mockups;
using AlalaIncomingPayments.Models;

namespace AlalaIncomingPaymentsApi.Controllers
{
    [RoutePrefix("api/IncomingPayments")]
    public class IncomingPaymentsController : ApiController
    {
        private IDiConnection _connector;
        private IIncomingPayments _payments;

        /// <summary>
        /// The default constructor of the incoming payments controller
        /// getting the DI connection configuration, initializing interfaces
        /// and connecting to ERP.
        /// </summary>
        public IncomingPaymentsController()
        {
            // Get connection details from configuration file.
            var confPath = Path.Combine(
                    HttpRuntime.AppDomainAppPath,
                    "Configuration");

            var connectionPath = File.ReadAllText(
                Path.Combine(
                    confPath,
                    "AlalaIncomingPayments.conf"));

            var connection = JsonConvert.DeserializeObject<DiConnectionModel>(connectionPath);
            var passwordPath = Path.Combine(
                    confPath,
                    "AlalaIncomingPayments.dat");

            _connector = new DiConnectionMockup(connection, passwordPath); // TODO: Turn this to the actual controller for integration testing.
            
            _connector.Connect();

            _payments = new IncomingPaymentsMockup(_connector); // TODO: Turn this to the actual controller for integration testing.
        }

        /// <summary>
        /// The default destructor of the incoming payments controller
        /// disconnecting from the ERP.
        /// </summary>
        ~IncomingPaymentsController()
        {
            _connector.Disconnect();
        }

        /// <summary>
        /// An HTTP interface that retrieves an incoming payment details
        /// given their ID.
        /// </summary>
        /// <param name="id">The ID of the incoming payment the details of
        /// which are to be retrieved.</param>
        /// <returns>An HTTP action result represents the HTTP response including 
        /// the incoming payment details.</returns>
        [HttpGet, Route("GetById", Name = "GetById")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var payment = _payments.GetById(id);

                if (payment == null)
                {
                    return NotFound();
                }

                return Ok(payment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// An HTTP request that creates a new incoming payment to the
        /// database.
        /// </summary>
        /// <param name="payment">A model that represents the payment
        /// is to be created.</param>
        /// <returns>An HTTP action result represents the HTTP response (i.e., success or failure
        /// of the actual event).</returns>
        [HttpPost, Route("Create")]
        public IHttpActionResult Create([FromBody]IncomingPaymentModel payment)
        {
            if (payment == null)
            {
                return BadRequest();
            }

            try
            {
                _payments.Create(payment);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// An HTTP request that deletes an incoming payment from the database.
        /// </summary>
        /// <param name="id">The ID of the incoming payment is to be deleted.</param>        
        /// <returns>An HTTP action result represents the HTTP response (i.e., success or failure
        /// of the actual event).</returns>
        [HttpDelete, Route("Delete")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var paymentFound = _payments.Delete(id);
                if (!paymentFound)
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
