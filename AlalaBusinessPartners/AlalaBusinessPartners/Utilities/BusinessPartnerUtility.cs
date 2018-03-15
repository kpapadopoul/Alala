using System;

using SAPbobsCOM;

namespace AlalaBusinessPartners.Utilities
{
    public class BusinessPartnerUtility
    {
        public enum BusinessPartnerTypes { Customer, Lid, Supplier };

        public BusinessPartnerTypes ConvertBusinessPartnerType(BoCardTypes cardtype)
        {
            var bpType = BusinessPartnerTypes.Customer;

            switch (cardtype)
            {
                case BoCardTypes.cCustomer:
                    bpType = BusinessPartnerTypes.Customer;
                    break;
                case BoCardTypes.cLid:
                    bpType = BusinessPartnerTypes.Lid;
                    break;
                case BoCardTypes.cSupplier:
                    bpType = BusinessPartnerTypes.Supplier;
                    break;
                default:
                    throw new ArgumentException();
            }

            return bpType;
        }

        public BoCardTypes ConvertBusinessPartnerType(BusinessPartnerTypes bpType)
        {
            var cardtype = BoCardTypes.cCustomer;

            switch (bpType)
            {
                case BusinessPartnerTypes.Customer:
                    cardtype = BoCardTypes.cCustomer;
                    break;
                case BusinessPartnerTypes.Lid:
                    cardtype = BoCardTypes.cLid;
                    break;
                case BusinessPartnerTypes.Supplier:
                    cardtype = BoCardTypes.cSupplier;
                    break;
                default:
                    throw new ArgumentException();
            }

            return cardtype;
        }
    }
}
