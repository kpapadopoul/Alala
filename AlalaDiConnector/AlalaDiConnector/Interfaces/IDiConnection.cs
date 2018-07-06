using SAPbobsCOM;

namespace AlalaDiConnector.Interfaces
{
    public interface IDiConnection
    {
        Company Company { get; set; }

        /// <summary>
        /// Connect to SAP DI.
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnect from SAP DI.
        /// </summary>
        void Disconnect();
    }
}
