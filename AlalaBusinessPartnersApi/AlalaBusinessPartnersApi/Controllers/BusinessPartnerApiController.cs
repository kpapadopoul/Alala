using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using AlalaBusinessPartners.Controllers;
using AlalaBusinessPartners.Models;
using AlalaDiConnector.Controllers;
using AlalaDiConnector.Models;

namespace AlalaBusinessPartnersApi.Controllers
{
    [Route("api/BusinessPartner")]
    public class BusinessPartnerApiController : ApiController
    {
        private DiConnectionController _connector;
        private BusinessPartnerController _bpController;
        
        public BusinessPartnerApiController()
        {
            _connector = new DiConnectionController(
                new DiConnectionModel {
                    Server = "To be updated",
                    CompanyDB = "SBODemoGB",
                    Username = "manager",
                    Password = "12345"
                });
            _connector.Connect();

            _bpController = new BusinessPartnerController(_connector.Company);
        }

        ~BusinessPartnerApiController()
        {
            _connector.Disconnect();
        }

        // GET api/<controller>/5
        [HttpGet]
        public BusinessPartnerModel Get(string code)
        {
            return _bpController.Get(code);
        }

        // POST api/<controller>
        [HttpPost]
        public void Create([FromBody]BusinessPartnerModel bpModel)
        {
            _bpController.Create(bpModel);
        }

        [HttpPost]
        public void UpdateContactEmployees([FromBody]BusinessPartnerModel bpModel)
        {
            _bpController.UpdateContactEmployees(bpModel);
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public void Delete(string code)
        {
            _bpController.Delete(code);
        }
    }
}