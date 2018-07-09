using System;
using System.IO;
using System.Windows.Forms;

using SAPbobsCOM;

using AlalaSecurePasswordLib;

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
        public DiConnectionMockup(DiConnectionModel connection, string passwordPath)
        {
            if (!File.Exists(passwordPath))
            {
                throw new ArgumentException("The password file does not exist.");
            }

            // Unprotect the SAP DI user password given its encrypted binary file path.
            var passProtector = new PasswordProtector();
            var password = passProtector.Unprotect(passwordPath);

            MessageBox.Show($"Server: {connection.Server}, Company: {connection.Company}, User: {connection.Username}, Password: {password}");
        }

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
