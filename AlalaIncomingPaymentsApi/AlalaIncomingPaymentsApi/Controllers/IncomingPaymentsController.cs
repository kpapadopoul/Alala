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
    [Route("api/IncomingPayments")]
    public class IncomingPaymentsController : ApiController
    {
        private IDiConnection _connector;
        private IIncomingPayments _payments;

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
            _connector = new DiConnectionMockup(connection); // TODO: Turn this to the actual controller for integration testing.
            
            _connector.Connect();

            _payments = new IncomingPaymentsMockup(_connector); // TODO: Turn this to the actual controller for integration testing.
        }

        ~IncomingPaymentsController()
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
        public IHttpActionResult Create([FromBody]IncomingPaymentModel payment)
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
