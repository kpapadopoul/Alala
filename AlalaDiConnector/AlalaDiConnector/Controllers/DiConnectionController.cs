using System;
using System.Runtime.InteropServices;

using SAPbobsCOM;

using AlalaDiConnector.Models;

namespace AlalaDiConnector.Controllers
{
    class DiConnectionController
    {
        public Company Company { get; }

        public DiConnectionController(DiConnectionModel connection)
        {
            Company = new Company
            {
                Server = connection.Server,
                DbServerType = BoDataServerTypes.dst_MSSQL2014,
                CompanyDB = connection.CompanyDB,
                UserName = connection.Username,
                Password = connection.Password,
                language = BoSuppLangs.ln_English
            };
        }

        public void Connect()
        {
            var success = Company.Connect().Equals(0);
            if (!success)
            {
                int code;
                string msg;
                Company.GetLastError(out code, out msg);
                throw new Exception($"Something went wrong\n {code} {msg}");
            }
        }

        public void Disconnect()
        {
            Company.Disconnect();
            Marshal.ReleaseComObject(Company);
        }
    }
}
