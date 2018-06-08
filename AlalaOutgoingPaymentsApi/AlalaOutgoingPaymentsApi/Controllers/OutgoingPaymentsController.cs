using System.IO;
using System.Web;
using System.Web.Http;

using Newtonsoft.Json;

using AlalaDiConnector.Controllers;
using AlalaDiConnector.Interfaces;
using AlalaDiConnector.Mockups;
using AlalaDiConnector.Models;

using AlalaOutgoingPayments.Controllers;
using AlalaOutgoingPayments.Interfaces;
using AlalaOutgoingPayments.Mockups;
using AlalaOutgoingPayments.Models;

namespace AlalaOutgoingPaymentsApi.Controllers
{
    [Route("api/OutgoingPayments")]
    public class OutgoingPaymentsController : ApiController
    {
        private IDiConnection _connector;
        private IOutgoingPayments _payments;

        public OutgoingPaymentsController()
        {
            // Get connection details from configuration file.
            var confPath = Path.Combine(
                    HttpRuntime.AppDomainAppPath,
                    "Configuration");

            var connectionPath = File.ReadAllText(
                Path.Combine(
                    confPath,
                    "AlalaOutgoingPayments.conf"));

            var connection = JsonConvert.DeserializeObject<DiConnectionModel>(connectionPath);
            var passwordPath = Path.Combine(
                    confPath,
                    "AlalaOutgoingPayments.dat");

            _connector = new DiConnectionMockup(connection, passwordPath); // TODO: Turn this to the actual controller for integration testing.

            _connector.Connect();

            _payments = new OutgoingPaymentsMockup(_connector); // TODO: Turn this to the actual controller for integration testing.
        }

        ~OutgoingPaymentsController()
        {
            _connector.Disconnect();
        }

        [HttpGet, Route("GetById", Name = "GetById")]
        public IHttpActionResult GetById(int paymentEntry)
        {
            var payment = _payments.GetById(paymentEntry);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        [HttpPost, Route("Create")]
        public IHttpActionResult Create([FromBody]OutgoingPaymentModel payment)
        {
            if (payment == null)
            {
                return BadRequest();
            }

            _payments.Create(payment);

            return Ok();
        }

        [HttpDelete, Route("Delete")]
        public IHttpActionResult Delete(int paymentEntry)
        {
            var paymentFound = _payments.Delete(paymentEntry);
            if (!paymentFound)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
