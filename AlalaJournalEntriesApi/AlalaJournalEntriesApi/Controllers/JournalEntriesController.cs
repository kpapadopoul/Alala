using System.IO;
using System.Web;
using System.Web.Http;

using Newtonsoft.Json;

using AlalaDiConnector.Controllers;
using AlalaDiConnector.Interfaces;
using AlalaDiConnector.Mockups;
using AlalaDiConnector.Models;

using AlalaJournalEntries.Controllers;
using AlalaJournalEntries.Interfaces;
using AlalaJournalEntries.Mockups;
using AlalaJournalEntries.Models;

namespace AlalaJournalEntriesApi.Controllers
{
    [Route("api/JournalEntries")]
    public class JournalEntriesController : ApiController
    {
        private IDiConnection _connector;
        private IJournalEntries _journalEntries;

        public JournalEntriesController()
        {
            // Get connection details from configuration file.
            var confPath = Path.Combine(
                    HttpRuntime.AppDomainAppPath,
                    "Configuration");

            var connectionPath = File.ReadAllText(
                Path.Combine(
                    confPath,
                    "AlalaJournalEntries.conf"));

            var connection = JsonConvert.DeserializeObject<DiConnectionModel>(connectionPath);
            _connector = new DiConnectionMockup(connection); // TODO: Turn this to the actual controller for integration testing.
            
            _connector.Connect();

            _journalEntries = new JournalEntriesMockup(_connector);
        }

        ~JournalEntriesController()
        {
            _connector.Disconnect();
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
