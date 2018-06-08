using SAPbobsCOM;

namespace AlalaDiConnector.Interfaces
{
    public interface IDiConnection
    {
        Company Company { get; set; }

        void Connect();
        void Disconnect();
    }
}
