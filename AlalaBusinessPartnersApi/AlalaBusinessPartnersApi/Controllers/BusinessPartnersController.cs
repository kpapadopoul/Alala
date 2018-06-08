using System.IO;
using System.Web;
using System.Web.Http;

using Newtonsoft.Json;

using AlalaDiConnector.Controllers;
using AlalaDiConnector.Interfaces;
using AlalaDiConnector.Mockups;
using AlalaDiConnector.Models;

using AlalaBusinessPartners.Controllers;
using AlalaBusinessPartners.Interfaces;
using AlalaBusinessPartners.Mockups;
using AlalaBusinessPartners.Models;

namespace AlalaBusinessPartnersApi.Controllers
{
    [Route("api/BusinessPartners")]
    public class BusinessPartnersController : ApiController
    {
        private IDiConnection _connector;
        private IBusinessPartners _bpController;
        
        public BusinessPartnersController()
        {
            // Get connection details from configuration file.
            var confPath = Path.Combine(
                    HttpRuntime.AppDomainAppPath,
                    "Configuration");

            var connectionPath = File.ReadAllText(
                Path.Combine(
                    confPath,
                    "AlalaBusinessPartners.conf"));

            var connection = JsonConvert.DeserializeObject<DiConnectionModel>(connectionPath);            
            _connector = new DiConnectionMockup(connection); // TODO: Turn this to actual controller for integration testing.

            _connector.Connect();

            _bpController = new BusinessPartnersMockup(_connector); // TODO: Turn this to actual controller for integration testing.
        }

        ~BusinessPartnersController()
        {
            _connector.Disconnect();
        }

        [HttpGet, Route("GetById", Name = "GetById")]
        public IHttpActionResult GetById(string id)
        {
            var businessPartner = _bpController.GetById(id);

            if (businessPartner == null)
            {
                return NotFound();
            }

            return Ok(businessPartner);
        }

        [HttpPost, Route("Create")]
        public IHttpActionResult Create([FromBody]BusinessPartnerModel businessPartner)
        {
            if (businessPartner == null)
            {
                return BadRequest();
            }

            _bpController.Create(businessPartner);

            return Ok();
        }

        [HttpPut, Route("UpdateContactEmployees")]
        public IHttpActionResult UpdateContactEmployees(string code, [FromBody]BusinessPartnerModel businessPartner)
        {
            if (businessPartner == null ||
                businessPartner.Code != code)
            {
                return BadRequest();
            }

            var bpFound = _bpController.UpdateContactEmployees(code, businessPartner);
            if (!bpFound)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete, Route("Delete")]
        public IHttpActionResult Delete(string code)
        {
            var bpFound = _bpController.Delete(code);
            if (!bpFound)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}