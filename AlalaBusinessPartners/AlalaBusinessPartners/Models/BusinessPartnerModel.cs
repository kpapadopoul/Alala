using System.Collections.Generic;

using AlalaBusinessPartners.Utilities;

namespace AlalaBusinessPartners.Models
{
    public class BusinessPartnerModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public BusinessPartnerUtility.BusinessPartnerTypes Type { get; set; }
        public List<ContactEmployeeModel> ContactEmployees { get; set; }
    }
}
