using System;
using System.Runtime.InteropServices;

using SAPbobsCOM;

using AlalaDiConnector.Controllers;
using AlalaBusinessPartners.Models;
using AlalaBusinessPartners.Utilities;

namespace AlalaBusinessPartners.Controllers
{
    public class BusinessPartners : Interfaces.IBusinessPartners
    {
        private readonly Company _company;
        private readonly BusinessPartnerUtility _utility;

        public BusinessPartners(DiConnectionController connection)
        {
            _company = connection.Company;
            _utility = new BusinessPartnerUtility();
        }

        public BusinessPartnerModel GetById(string businessPartnerCode)
        {            
            // Prepare the object
            var bp = (SAPbobsCOM.BusinessPartners)_company.GetBusinessObject(BoObjectTypes.oBusinessPartners);

            var bpModel = new BusinessPartnerModel();

            // Find the record to update if exists
            if (bp.GetByKey(businessPartnerCode))
            {
                bpModel.Code = bp.CardCode;
                bpModel.Name = bp.CardName;
                bpModel.Type = _utility.ConvertBusinessPartnerType(bp.CardType);                
            }

            Marshal.ReleaseComObject(bp);
            return bpModel;
        }

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
