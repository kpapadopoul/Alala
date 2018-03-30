using System.Web.Http;

using AlalaBusinessPartners.Controllers;
using AlalaBusinessPartners.Models;
using AlalaDiConnector.Controllers;
using AlalaDiConnector.Models;

using AlalaBusinessPartnersApi.Resources;

namespace AlalaBusinessPartnersApi.Controllers
{
    [Route("api/BusinessPartner")]
    public class BusinessPartnersController : ApiController
    {
        private DiConnectionController _connector;
        private BusinessPartners _bpController;
        
        public BusinessPartnersController()
        {
            // TODO: Update controller to get connection details
            // from XML file.
            _connector = new DiConnectionController(
                new DiConnectionModel {
                    Server = ConnectionDetails.Server,
                    CompanyDB = ConnectionDetails.CompanyDB,
                    Username = ConnectionDetails.Username,
                    Password = ConnectionDetails.Password
                });

            // TODO: Uncomment the following line when
            // SAP testbed is ready to test
            //_connector.Connect();

            _bpController = new BusinessPartners(_connector);
        }

        ~BusinessPartnersController()
        {
            // TODO: Uncomment the following line when
            // SAP testbed is ready to test
            //_connector.Disconnect();
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