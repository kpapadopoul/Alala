using System.IO;
using System.Web;
using System.Web.Http;

using Newtonsoft.Json;

using AlalaDiConnector.Controllers;
using AlalaDiConnector.Models;
using AlalaOutgoingPayments.Controllers;
using AlalaOutgoingPayments.Interfaces;
using AlalaOutgoingPayments.Models;

namespace AlalaOutgoingPaymentsApi.Controllers
{
    [Route("api/OutgoingPayments")]
    public class OutgoingPaymentsController : ApiController
    {
        private DiConnectionController _connector;
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
                    "AlalaIncomingPayments.conf"));

            var connection = JsonConvert.DeserializeObject<DiConnectionModel>(connectionPath);
            _connector = new DiConnectionController(connection);

            // TODO: Uncomment the following line when
            // SAP testbed is ready to test
            //_connector.Connect();

            _payments = new OutgoingPayments(_connector);
        }

        ~OutgoingPaymentsController()
        {
            // TODO: Uncomment the following line when
            // SAP testbed is ready to test
            //_connector.Disconnect();
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
