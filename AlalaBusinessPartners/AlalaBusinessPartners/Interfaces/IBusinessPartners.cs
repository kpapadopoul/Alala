using AlalaBusinessPartners.Models;

namespace AlalaBusinessPartners.Interfaces
{
    public interface IBusinessPartners
    {
        /// <summary>
        /// Gets a single business partner details.
        /// </summary>
        /// <param name="businessPartnerCode">The code of the business partner the
        /// details of which are to be returned.</param>
        /// <returns>A model that represents the business partner info.</returns>
        BusinessPartnerModel GetById(string businessPartnerCode);

        /// <summary>
        /// Creates a business partner to the database.
        /// </summary>
        /// <param name="businessPartner">A model that contains the business partner
        /// info to be created.</param>
        void Create(BusinessPartnerModel businessPartner);

        /// <summary>
        /// Updates contact employees of a given business partner.
        /// </summary>
        /// <param name="businessPartnerCode">The code of the business partner 
        /// is to be updated.</param>
        /// <param name="businessPartner">The model that contains the new contact 
        /// employee details.</param>
        /// <returns>A boolean value that is set to true whether the business partner
        /// found in the database.</returns>
        bool UpdateContactEmployees(string businessPartnerCode, BusinessPartnerModel businessPartner);

        /// <summary>
        /// Deletes a business partner from the database.
        /// </summary>
        /// <param name="businessPartnerCode">The code of the business partner
        /// is to be deleted.</param>
        /// <returns>A boolean value that is set to true whether the business partner
        /// found in the database.</returns>
        bool Delete(string businessPartnerCode);
    }
}
