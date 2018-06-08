using AlalaBusinessPartners.Models;

namespace AlalaBusinessPartners.Mockups
{
    public class BusinessPartnersMockup : Interfaces.IBusinessPartners
    {
        public BusinessPartnerModel GetById(string businessPartnerCode) { return new BusinessPartnerModel(); }
        public void Create(BusinessPartnerModel businessPartner) { }
        public bool UpdateContactEmployees(string businessPartnerCode, BusinessPartnerModel businessPartner) { return false; }
        public bool Delete(string businessPartnerCode) { return false; }
    }
}
