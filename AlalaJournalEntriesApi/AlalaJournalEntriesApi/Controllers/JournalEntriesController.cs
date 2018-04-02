using System.Web.Http;

using AlalaDiConnector.Controllers;
using AlalaDiConnector.Models;
using AlalaJournalEntries.Controllers;
using AlalaJournalEntries.Interfaces;
using AlalaJournalEntries.Models;

using AlalaJournalEntriesApi.Resources;

namespace AlalaJournalEntriesApi.Controllers
{
    [Route("api/JournalEntries")]
    public class JournalEntriesController : ApiController
    {
        private DiConnectionController _connector;
        private IJournalEntries _journalEntries;

        public JournalEntriesController()
        {
            // TODO: Update controller to get connection details
            // from XML file.
            _connector = new DiConnectionController(
                new DiConnectionModel
                {
                    Server = ConnectionDetails.Server,
                    CompanyDB = ConnectionDetails.CompanyDB,
                    Username = ConnectionDetails.Username,
                    Password = ConnectionDetails.Password
                });

            // TODO: Uncomment the following line when
            // SAP testbed is ready to test
            //_connector.Connect();

            _journalEntries = new JournalEntries(_connector);
        }

        ~JournalEntriesController()
        {
            // TODO: Uncomment the following line when
            // SAP testbed is ready to test
            //_connector.Disconnect();
        }

        [HttpGet, Route("GetById", Name = "GetById")]
        public IHttpActionResult GetById(int jdtNum)
        {
            var journalEntry = _journalEntries.GetById(jdtNum);

            if (journalEntry == null)
            {
                return NotFound();
            }

            return Ok(journalEntry);
        }

        [HttpPost, Route("Create")]
        public IHttpActionResult Create([FromBody]JournalEntryModel journalEntry)
        {
            if (journalEntry == null)
            {
                return BadRequest();
            }

            _journalEntries.Create(journalEntry);

            return Ok();
        }

        [HttpDelete, Route("Delete")]
        public IHttpActionResult Delete(int jdtNum)
        {
            var jeFound = _journalEntries.Delete(jdtNum);
            if (!jeFound)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
