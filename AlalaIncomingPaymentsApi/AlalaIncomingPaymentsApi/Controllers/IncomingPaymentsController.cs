using System.IO;
using System.Web;
using System.Web.Http;

using Newtonsoft.Json;

using AlalaDiConnector.Controllers;
using AlalaDiConnector.Models;
using AlalaIncomingPayments.Interfaces;
using AlalaIncomingPayments.Controllers;
using AlalaIncomingPayments.Models;

namespace AlalaIncomingPaymentsApi.Controllers
{
    [Route("api/IncomingPayments")]
    public class IncomingPaymentsController : ApiController
    {
        private DiConnectionController _connector;
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
            _connector = new DiConnectionController(connection);

            // TODO: Uncomment the following line when
            // SAP testbed is ready to test
            //_connector.Connect();

            _payments = new IncomingPayments(_connector);
        }

        ~IncomingPaymentsController()
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
