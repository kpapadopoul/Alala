using System;

using SAPbobsCOM;

using AlalaBusinessPartners.Models;

namespace AlalaBusinessPartners.Controllers
{
    class BusinessPartnerController
    {
        private readonly Company _company;

        public BusinessPartnerController(Company company)
        {
            _company = company;
        }

        public void Create(BusinessPartnerModel businessPartner)
        {
            // Prepare the object
            var bp = (BusinessPartners)_company.GetBusinessObject(BoObjectTypes.oBusinessPartners);

            // Set values
            // The built-in auto-complete procedure completes the default values of the other properties.
            bp.CardCode = businessPartner.Code; //Mandatory
            bp.CardName = businessPartner.Name;
            bp.CardType = BoCardTypes.cCustomer; //Mandatory

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
        }
    }
}
