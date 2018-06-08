using AlalaDiConnector.Interfaces;

using AlalaJournalEntries.Models;

namespace AlalaJournalEntries.Mockups
{
    public class JournalEntriesMockup : Interfaces.IJournalEntries
    {
        public JournalEntriesMockup(IDiConnection connection) { }

        public JournalEntryModel GetById(int jdtNum) { return new JournalEntryModel(); }
        public void Create(JournalEntryModel journalEntry) { }
        public bool Delete(int jdtNum) { return false; }
    }
}
