using System;
using System.IO;
using System.Runtime.InteropServices;

using SAPbobsCOM;

using AlalaSecurePasswordLib;

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
        public DiConnectionController(DiConnectionModel connection, string passwordPath)
        {
            if (!File.Exists(passwordPath))
            {
                throw new ArgumentException("The password file does not exist.");
            }

            var passProtector = new PasswordProtector();
            var password = passProtector.Unprotect(passwordPath);

            Company = new Company
            {
                Server = connection.Server,
                DbServerType = BoDataServerTypes.dst_MSSQL2014,
                CompanyDB = connection.Company,
                UserName = connection.Username,
                Password = password,
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
