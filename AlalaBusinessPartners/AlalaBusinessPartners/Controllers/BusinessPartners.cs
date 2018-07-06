using System;
using System.Runtime.InteropServices;

using SAPbobsCOM;

using AlalaDiConnector.Controllers;
using AlalaDiConnector.Interfaces;
using AlalaBusinessPartners.Models;
using AlalaBusinessPartners.Utilities;

namespace AlalaBusinessPartners.Controllers
{
    public class BusinessPartners : Interfaces.IBusinessPartners
    {
        private readonly Company _company;
        private readonly BusinessPartnerUtility _utility;

        /// <summary>
        /// The default constructor of the business partner class.
        /// </summary>
        /// <param name="connection">An interface of SAP DI connection to be
        /// used for handling business partner objects.</param>
        public BusinessPartners(IDiConnection connection)
        {
            _company = connection.Company;
            _utility = new BusinessPartnerUtility();
        }

        /// <summary>
        /// Gets a single business partner details.
        /// </summary>
        /// <param name="businessPartnerCode">The code of the business partner the
        /// details of which are to be returned.</param>
        /// <returns>A model that represents the business partner info.</returns>
        public BusinessPartnerModel GetById(string businessPartnerCode)
        {            
            // Prepare the object
            var bp = (SAPbobsCOM.BusinessPartners)_company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
            
            // Find the business partner record by its code.
            BusinessPartnerModel bpModel = null;
            if (bp.GetByKey(businessPartnerCode))
            {
                bpModel = new BusinessPartnerModel();
                bpModel.Code = bp.CardCode;
                bpModel.Name = bp.CardName;
                bpModel.Type = _utility.ConvertBusinessPartnerType(bp.CardType);                
            }

            Marshal.ReleaseComObject(bp);
            return bpModel;
        }

        /// <summary>
        /// Creates a business partner to the ERP.
        /// </summary>
        /// <param name="businessPartner">A model that contains the business partner
        /// info to be created.</param>
        public void Create(BusinessPartnerModel businessPartner)
        {
            // Prepare the object
            var bp = (SAPbobsCOM.BusinessPartners)_company.GetBusinessObject(BoObjectTypes.oBusinessPartners);

            // Set values
            // The built-in auto-complete procedure completes the default values of the other properties.
            bp.CardCode = businessPartner.Code; //Mandatory
            bp.CardName = businessPartner.Name;
            bp.CardType = _utility.ConvertBusinessPartnerType(businessPartner.Type); //Mandatory

            // Add it to the database
            var success = bp.Add().Equals(0);
            if (!success)
            {
                // Error handling
                int code;
                string msg;
                _company.GetLastError(out code, out msg);
                throw new Exception($"Something went wrong\n{code} {msg}");
            }

            Marshal.ReleaseComObject(bp);
        }

        /// <summary>
        /// Updates contact employees of a given business partner.
        /// </summary>
        /// <param name="businessPartnerCode">The code of the business partner 
        /// is to be updated.</param>
        /// <param name="businessPartner">The model that contains the new contact 
        /// employee details.</param>
        /// <returns>A boolean value that is set to true whether the business partner
        /// found in the ERP.</returns>
        public bool UpdateContactEmployees(string businessPartnerCode, BusinessPartnerModel businessPartner)
        {
            // Prepare the object
            var bp = (SAPbobsCOM.BusinessPartners)_company.GetBusinessObject(BoObjectTypes.oBusinessPartners);

            // Find the record to update if exists
            var bpFound = false;
            if (bp.GetByKey(businessPartnerCode))
            {
                bpFound = true;

                foreach (var contact in businessPartner.ContactEmployees)
                {
                    if (bp.ContactEmployees.Count > 0)
                    {
                        bp.ContactEmployees.Add();
                    }

                    bp.ContactEmployees.Name = contact.Name;
                    bp.ContactEmployees.E_Mail = contact.Email;                    
                }

                var success = bp.Update().Equals(0);
                if (!success)
                {
                    // Error handling
                    int code;
                    string msg;
                    _company.GetLastError(out code, out msg);
                    throw new Exception($"Something went wrong\n{code} {msg}");
                }
            }

            Marshal.ReleaseComObject(bp);
            return bpFound;
        }

        /// <summary>
        /// Deletes a business partner from the ERP.
        /// </summary>
        /// <param name="businessPartnerCode">The code of the business partner
        /// is to be deleted.</param>
        /// <returns>A boolean value that is set to true whether the business partner
        /// found in the ERP.</returns>
        public bool Delete(string businessPartnerCode)
        {
            // Prepare the object
            var bp = (SAPbobsCOM.BusinessPartners)_company.GetBusinessObject(BoObjectTypes.oBusinessPartners);

            // Find the record to update if exists
            var bpFound = false;
            if (bp.GetByKey(businessPartnerCode))
            {
                bpFound = true;

                // Remove it from the database
                var success = bp.Remove().Equals(0);
                if (!success)
                {
                    // Error handling
                    int code;
                    string msg;
                    _company.GetLastError(out code, out msg);
                    throw new Exception($"Something went wrong\n{code} {msg}");
                }
            }

            Marshal.ReleaseComObject(bp);
            return bpFound;
        }

        public void ExportAsXml(string businessPartnerCode, string path)
        {
            // Prepare the object
            var bp = (SAPbobsCOM.BusinessPartners)_company.GetBusinessObject(BoObjectTypes.oBusinessPartners);

            // Find the record to update if exists
            if (bp.GetByKey(businessPartnerCode))
            {
                _company.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;
                bp.SaveXML(path);
            }

            Marshal.ReleaseComObject(bp);
        }

        public void ImportFromXml(string path)
        {
            // Get number of business objects in file
            var elementCount = _company.GetXMLelementCount(path);

            // Loop through objects - find BP and add it to database
            for (var i = 0; i < elementCount; i++)
            {
                if (_company.GetXMLobjectType(path, i) != BoObjectTypes.oBusinessPartners) continue;

                var bp = (SAPbobsCOM.BusinessPartners)_company.GetBusinessObjectFromXML(path, i);

                var success = bp.Add().Equals(0);
                if (!success)
                {
                    // Error handling
                    int code;
                    string msg;
                    _company.GetLastError(out code, out msg);
                    throw new Exception($"Something went wrong\n{code} {msg}");
                }

                Marshal.ReleaseComObject(bp);
            }
        }
    }
}
