using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
