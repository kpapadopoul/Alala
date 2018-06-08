using SAPbobsCOM;

using AlalaDiConnector.Interfaces;
using AlalaDiConnector.Models;

namespace AlalaDiConnector.Mockups
{
    public class DiConnectionMockup : IDiConnection
    {
        public Company Company { get; set; }

        public DiConnectionMockup(DiConnectionModel connection, string passwordPath) { }

        public void Connect() { }
        public void Disconnect() { }
    }
}
