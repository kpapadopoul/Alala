using System;
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

        /// <summary>
        /// The default constructor of the journal entries controller
        /// getting the DI connection configuration, initializing interfaces
        /// and connecting to ERP.
        /// </summary>
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
            var passwordPath = Path.Combine(
                    confPath,
                    "AlalaJournalEntries.dat");

            _connector = new DiConnectionMockup(connection, passwordPath); // TODO: Turn this to the actual controller for integration testing.
            
            _connector.Connect();

            _journalEntries = new JournalEntriesMockup(_connector); // TODO: Turn this to the actual controller for integration testing.
        }

        /// <summary>
        /// The default destructor of the journal entries controller
        /// disconnecting from the ERP.
        /// </summary>
        ~JournalEntriesController()
        {
            _connector.Disconnect();
        }

        /// <summary>
        /// An HTTP interface that retrieves a journal entry details
        /// given their ID.
        /// </summary>
        /// <param name="jdtNum">The ID of the journal entry the details of
        /// which are to be retrieved.</param>
        /// <returns>An HTTP action result represents the HTTP response including 
        /// the journal entry details.</returns>
        [HttpGet, Route("GetById", Name = "GetById")]
        public IHttpActionResult GetById(int jdtNum)
        {
            try
            {
                var journalEntry = _journalEntries.GetById(jdtNum);

                if (journalEntry == null)
                {
                    return NotFound();
                }

                return Ok(journalEntry);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// An HTTP request that creates a new journal entry to the
        /// database.
        /// </summary>
        /// <param name="journalEntry">A model that represents the journal entry
        /// is to be created.</param>
        /// <returns>An HTTP action result represents the HTTP response (i.e., success or failure
        /// of the actual event).</returns>
        [HttpPost, Route("Create")]
        public IHttpActionResult Create([FromBody]JournalEntryModel journalEntry)
        {
            if (journalEntry == null)
            {
                return BadRequest();
            }

            try
            {
                _journalEntries.Create(journalEntry);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// An HTTP request that deletes a journal entry from the database.
        /// </summary>
        /// <param name="jdtNum">The ID of the journal entry is to be deleted.</param>
        /// <returns>An HTTP action result represents the HTTP response (i.e., success or failure
        /// of the actual event).</returns>
        [HttpDelete, Route("Delete")]
        public IHttpActionResult Delete(int jdtNum)
        {
            try
            {
                var jeFound = _journalEntries.Delete(jdtNum);
                if (!jeFound)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
