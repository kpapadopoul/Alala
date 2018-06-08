using System;
using System.Runtime.InteropServices;

using SAPbobsCOM;

using AlalaDiConnector.Interfaces;
using AlalaDiConnector.Models;

namespace AlalaDiConnector.Controllers
{
    public class DiConnectionController : IDiConnection
    {
        public Company Company { get; set; } // Property respesents the SAP DI company object

        public DiConnectionController() { }

        /// <summary>
        /// Default constructor of the DI connection controller;
        /// initializes the company object based on a given
        /// DI connection model.
        /// </summary>
        /// <param name="connection">The given model represents the DI connection details.</param>
        public DiConnectionController(DiConnectionModel connection)
        {
            Company = new Company
            {
                Server = connection.Server,
                DbServerType = BoDataServerTypes.dst_MSSQL2014,
                CompanyDB = connection.Company,
                UserName = connection.Username,
                Password = connection.Password,
                language = BoSuppLangs.ln_English
            };
        }

        /// <summary>
        /// Connect to SAP DI.
        /// </summary>
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

        /// <summary>
        /// Disconnect from SAP DI.
        /// </summary>
        public void Disconnect()
        {
            Company.Disconnect();
            Marshal.ReleaseComObject(Company);
        }
    }
}
