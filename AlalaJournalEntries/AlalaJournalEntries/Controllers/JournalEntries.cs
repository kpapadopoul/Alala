using System;
using System.Linq;
using System.Runtime.InteropServices;

using SAPbobsCOM;

using AlalaDiConnector.Interfaces;

using AlalaJournalEntries.Models;

namespace AlalaJournalEntries.Controllers
{
    public class JournalEntries : Interfaces.IJournalEntries
    {
        private readonly Company _company;

        /// <summary>
        /// Default constructor of journal entries controller
        /// initializing company DI object.
        /// </summary>
        /// <param name="connection">An interface represents the DI connection
        /// to be used for initializing the DI company object.</param>
        public JournalEntries(IDiConnection connection)
        {
            _company = connection.Company;
        }

        /// <summary>
        /// Gets the details of a journal entry.
        /// </summary>
        /// <param name="jdtNum">The number of the journal entry to be returned.</param>
        /// <returns>A model that represents the journal entry info.</returns>
        public JournalEntryModel GetById(int jdtNum)
        {
            // Prepare the object
            var journalEntryObj = (SAPbobsCOM.JournalEntries)_company.GetBusinessObject(BoObjectTypes.oJournalEntries);

            JournalEntryModel journalEntry = null;
            if (journalEntryObj.GetByKey(jdtNum))
            {
                journalEntry = new JournalEntryModel();

                journalEntry.Number = journalEntryObj.Number;
                journalEntry.Series = journalEntryObj.Series;
                journalEntry.TaxDate = journalEntryObj.TaxDate;
                journalEntry.ReferenceDate = journalEntryObj.ReferenceDate;
                journalEntry.DueDate = journalEntryObj.DueDate;

                // TODO: Check if it works.
                for (int i = 0; i < journalEntryObj.Lines.Count; i++)
                {
                    journalEntryObj.SetCurrentLine(i);
                    journalEntry.Lines.Add(
                        new JournalEntryLineModel
                        {
                            AccountCode = journalEntryObj.Lines.AccountCode,
                            TaxCode = journalEntryObj.Lines.TaxCode,
                            FCCredit = journalEntryObj.Lines.FCCredit,
                            FCDebit = journalEntryObj.Lines.FCDebit,
                            Credit = journalEntryObj.Lines.Credit,
                            Debit = journalEntryObj.Lines.Debit
                        });
                }
            }

            Marshal.ReleaseComObject(journalEntryObj);
            return journalEntry;
        }

        /// <summary>
        /// Creates a journal entry to the database.
        /// </summary>
        /// <param name="journalEntry">A model that contains the journal entry info
        /// to be created.</param>
        public void Create(JournalEntryModel journalEntry)
        {
            // Prepare the object
            var journalEntryObj = (SAPbobsCOM.JournalEntries)_company.GetBusinessObject(BoObjectTypes.oJournalEntries);

            // Set header values
            journalEntryObj.Series = journalEntry.Series;
            journalEntryObj.TaxDate = DateTime.Now;
            journalEntryObj.ReferenceDate = DateTime.Now;
            journalEntryObj.DueDate = DateTime.Now;

            // Set line values
            foreach (var line in journalEntry.Lines)
            {
                if (line != journalEntry.Lines.First())
                    journalEntryObj.Lines.Add();

                journalEntryObj.Lines.AccountCode = line.AccountCode;
                journalEntryObj.Lines.TaxCode = line.TaxCode;
                journalEntryObj.Lines.FCCredit = line.FCCredit;
                journalEntryObj.Lines.FCDebit = line.FCDebit;
                journalEntryObj.Lines.Credit = line.Credit;
                journalEntryObj.Lines.Debit = line.Debit;
            }

            // Add it to database
            var success = journalEntryObj.Add().Equals(0);
            if (!success)
            {
                // Error handling
                int code;
                string msg;
                _company.GetLastError(out code, out msg);
                throw new Exception($"Something went wrong\n{code} {msg}");
            }

            Marshal.ReleaseComObject(journalEntryObj);
        }

        /// <summary>
        /// Deletes a journal entry from the database.
        /// </summary>
        /// <param name="jdtNum">The number of the journal entry to be deleted.</param>
        /// <returns>A boolean value that is set to true whether the journal entry
        /// found in the database.</returns>
        public bool Delete(int jdtNum)
        {
            // Prepare the object
            var journalEntryObj = (SAPbobsCOM.JournalEntries)_company.GetBusinessObject(BoObjectTypes.oJournalEntries);

            var entryFound = false;
            if (journalEntryObj.GetByKey(jdtNum))
            {
                entryFound = true;

                // Remove it from database
                var success = journalEntryObj.Remove().Equals(0);
                if (!success)
                {
                    // Error handling
                    int code;
                    string msg;
                    _company.GetLastError(out code, out msg);
                    throw new Exception($"Something went wrong\n{code} {msg}");
                }
            }

            Marshal.ReleaseComObject(journalEntryObj);
            return entryFound;
        }
    }
}
