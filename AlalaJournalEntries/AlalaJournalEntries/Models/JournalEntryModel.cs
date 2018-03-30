using System;
using System.Collections.Generic;

namespace AlalaJournalEntries.Models
{
    public class JournalEntryModel
    {
        public int Number { get; set; }
        public int Series { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReferenceDate { get; set; }
        public DateTime TaxDate { get; set; }
        public List<JournalEntryLineModel> Lines { get; set; }
    }
}
