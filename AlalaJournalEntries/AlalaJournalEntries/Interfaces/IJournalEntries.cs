using AlalaJournalEntries.Models;

namespace AlalaJournalEntries.Interfaces
{
    public interface IJournalEntries
    {
        /// <summary>
        /// Gets the details of a journal entry.
        /// </summary>
        /// <param name="jdtNum">The number of the journal entry to be returned.</param>
        /// <returns>A model that represents the journal entry info.</returns>
        JournalEntryModel GetById(int jdtNum);

        /// <summary>
        /// Creates a journal entry to the database.
        /// </summary>
        /// <param name="journalEntry">A model that contains the journal entry info
        /// to be created.</param>
        void Create(JournalEntryModel journalEntry);

        /// <summary>
        /// Deletes a journal entry from the database.
        /// </summary>
        /// <param name="jdtNum">The number of the journal entry to be deleted.</param>
        /// <returns>A boolean value that is set to true whether the journal entry
        /// found in the database.</returns>
        bool Delete(int jdtNum);
    }
}
