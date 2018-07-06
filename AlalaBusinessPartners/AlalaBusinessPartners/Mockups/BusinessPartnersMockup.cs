using AlalaDiConnector.Interfaces;

using AlalaBusinessPartners.Models;

namespace AlalaBusinessPartners.Mockups
{
    public class BusinessPartnersMockup : Interfaces.IBusinessPartners
    {
        /// <summary>
        /// The default constructor of the business partner mockup class.
        /// </summary>
        /// <param name="connection">An interface of SAP DI connection to be
        /// used for handling business partner objects.</param>
        public BusinessPartnersMockup(IDiConnection connection) { }

        /// <summary>
        /// Gets a single business partner details.
        /// </summary>
        /// <param name="businessPartnerCode">The code of the business partner the
        /// details of which are to be returned.</param>
        /// <returns>A model that represents the business partner info.</returns>
        public BusinessPartnerModel GetById(string businessPartnerCode) { return new BusinessPartnerModel(); }
        
        /// <summary>
        /// Creates a business partner to the ERP.
        /// </summary>
        /// <param name="businessPartner">A model that contains the business partner
        /// info to be created.</param>
        public void Create(BusinessPartnerModel businessPartner) { }

        /// <summary>
        /// Updates contact employees of a given business partner.
        /// </summary>
        /// <param name="businessPartnerCode">The code of the business partner 
        /// is to be updated.</param>
        /// <param name="businessPartner">The model that contains the new contact 
        /// employee details.</param>
        /// <returns>A boolean value that is set to true whether the business partner
        /// found in the ERP.</returns>
        public bool UpdateContactEmployees(string businessPartnerCode, BusinessPartnerModel businessPartner) { return false; }

        /// <summary>
        /// Deletes a business partner from the ERP.
        /// </summary>
        /// <param name="businessPartnerCode">The code of the business partner
        /// is to be deleted.</param>
        /// <returns>A boolean value that is set to true whether the business partner
        /// found in the ERP.</returns>
        public bool Delete(string businessPartnerCode) { return false; }
    }
}
