using AlalaJournalEntries.Models;

namespace AlalaJournalEntries.Interfaces
{
    public interface IJournalEntries
    {
        JournalEntryModel GetById(int jdtNum);
        void Create(JournalEntryModel journalEntry);
        bool Delete(int jdtNum);
    }
}
