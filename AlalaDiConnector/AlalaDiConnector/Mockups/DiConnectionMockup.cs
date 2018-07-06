using SAPbobsCOM;

using AlalaDiConnector.Interfaces;
using AlalaDiConnector.Models;

namespace AlalaDiConnector.Mockups
{
    public class DiConnectionMockup : IDiConnection
    {
        public Company Company { get; set; }

        /// <summary>
        /// Default constructor of the DI connection mockup.
        /// </summary>
        public DiConnectionMockup(DiConnectionModel connection, string passwordPath) { }

        /// <summary>
        /// Connect to SAP DI.
        /// </summary>
        public void Connect() { }

        /// <summary>
        /// Disconnect from SAP DI.
        /// </summary>
        public void Disconnect() { }
    }
}
