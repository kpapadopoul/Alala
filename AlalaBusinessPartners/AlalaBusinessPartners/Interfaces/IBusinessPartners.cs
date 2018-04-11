using AlalaBusinessPartners.Models;

namespace AlalaBusinessPartners.Interfaces
{
    public interface IBusinessPartners
    {
        BusinessPartnerModel GetById(string businessPartnerCode);
        void Create(BusinessPartnerModel businessPartner);
        bool UpdateContactEmployees(string businessPartnerCode, BusinessPartnerModel businessPartner);
        bool Delete(string businessPartnerCode);
    }
}
