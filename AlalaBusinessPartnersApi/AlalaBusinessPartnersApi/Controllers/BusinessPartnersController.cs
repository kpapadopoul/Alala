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
        
        /// <summary>
        /// The default constructor of the business partner controller
        /// getting the DI connection configuration, initializing interfaces
        /// and connecting to ERP.
        /// </summary>
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
            var passwordPath = Path.Combine(
                    confPath,
                    "AlalaBusinessPartners.dat");

            _connector = new DiConnectionMockup(connection, passwordPath); // TODO: Turn this to actual controller for integration testing.

            _connector.Connect();

            _bpController = new BusinessPartnersMockup(_connector); // TODO: Turn this to actual controller for integration testing.
        }

        /// <summary>
        /// The default destructor of the business partner controller
        /// disconnecting from the ERP.
        /// </summary>
        ~BusinessPartnersController()
        {
            _connector.Disconnect();
        }

        /// <summary>
        /// An HTTP interface that retrieves a business partner details
        /// given their ID.
        /// </summary>
        /// <param name="id">The ID of the business partner the details of
        /// whom are to be retrieved.</param>
        /// <returns>An HTTP action result represents the HTTP response including 
        /// the business partner details.</returns>
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

        /// <summary>
        /// An HTTP request that creates a new business partner to the
        /// database.
        /// </summary>
        /// <param name="businessPartner">A model that represents the business partner
        /// is to be created.</param>
        /// <returns>An HTTP action result represents the HTTP response (i.e., success or failure
        /// of the actual event).</returns>
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

        /// <summary>
        /// An HTTP request that updates contact employees of a given business partner.
        /// </summary>
        /// <param name="id">The ID of the business partner the contact employees
        /// of whom is to be updated.</param>
        /// <param name="businessPartner">The business partner model that is to be used as 
        /// input for the contact employees to be updated.</param>
        /// <returns>An HTTP action result represents the HTTP response (i.e., success or failure
        /// of the actual event).</returns>
        [HttpPut, Route("UpdateContactEmployees")]
        public IHttpActionResult UpdateContactEmployees(string id, [FromBody]BusinessPartnerModel businessPartner)
        {
            if (businessPartner == null ||
                businessPartner.Code != id)
            {
                return BadRequest();
            }

            var bpFound = _bpController.UpdateContactEmployees(id, businessPartner);
            if (!bpFound)
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        /// An HTTP request that deletes a business partner from the database.
        /// </summary>
        /// <param name="id">The ID of the business partner is to be deleted.</param>
        /// <returns>An HTTP action result represents the HTTP response (i.e., success or failure
        /// of the actual event).</returns>
        [HttpDelete, Route("Delete")]
        public IHttpActionResult Delete(string id)
        {
            var bpFound = _bpController.Delete(id);
            if (!bpFound)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}