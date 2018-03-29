using System;

namespace AlalaJournalEntries.Models
{
    public class JournalEntryLineModel
    {
        public string AccountCode { get; set; }
        public string TaxCode { get; set; }
        public double FCDebit { get; set; }
        public double FCCredit { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReferenceDate { get; set; }
    }
}
