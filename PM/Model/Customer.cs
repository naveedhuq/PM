using System;

namespace PM.Model
{
    public class Customer
    {
        public int ID { get; set; }
        public bool IsActive { get; set; }
        public DateTime? OpeningDate { get; set; }
        public Enumerators.CustomerTypeEnum CustomerType { get; set; }
        public string CustomerName { get; set; }
        public string Personal_Gender { get; set; }
        public DateTime? Personal_BirthDate { get; set; }
        public string Personal_SSN { get; set; }
        public string Personal_LicenseID { get; set; }
        public string Business_TypeOfCompany { get; set; }
        public string Business_TaxID { get; set; }
        public string Notes { get; set; }
    }
}
