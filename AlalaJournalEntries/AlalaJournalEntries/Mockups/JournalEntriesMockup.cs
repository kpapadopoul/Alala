using AlalaDiConnector.Interfaces;

using AlalaJournalEntries.Models;

namespace AlalaJournalEntries.Mockups
{
    public class JournalEntriesMockup : Interfaces.IJournalEntries
    {
        /// <summary>
        /// Default constructor of journal entries mockup.
        /// </summary>
        /// <param name="connection">An interface to the DI connection.</param>
        public JournalEntriesMockup(IDiConnection connection) { }

        /// <summary>
        /// Gets the details of a journal entry.
        /// </summary>
        /// <param name="jdtNum">The number of the journal entry to be returned.</param>
        /// <returns>A model that represents the journal entry info.</returns>
        public JournalEntryModel GetById(int jdtNum) { return new JournalEntryModel(); }
        
        /// <summary>
        /// Creates a journal entry to the database.
        /// </summary>
        /// <param name="journalEntry">A model that contains the journal entry info
        /// to be created.</param>
        public void Create(JournalEntryModel journalEntry) { }

        /// <summary>
        /// Deletes a journal entry from the database.
        /// </summary>
        /// <param name="jdtNum">The number of the journal entry to be deleted.</param>
        /// <returns>A boolean value that is set to true whether the journal entry
        /// found in the database.</returns>
        public bool Delete(int jdtNum) { return true; }
    }
}
