using System;

namespace Viasoft.Accounting.Domain.Contracts.Ctes
{
    public class CtePersonsOfInterestDto
    {
        public CtePersonsOfInterestDto(Guid? id, Guid? personId, string name, string addressStreet, string addressNumber, string addressCity, string addressState, string addressZipcode, string cnpjCpf, string stateRegistration, string addressCountry, string phone)
        {
            Id = id;
            PersonId = personId;
            Name = name;
            AddressStreet = addressStreet;
            AddressNumber = addressNumber;
            AddressCity = addressCity;
            AddressState = addressState;
            AddressZipcode = addressZipcode;
            CnpjCpf = cnpjCpf;
            StateRegistration = stateRegistration;
            AddressCountry = addressCountry;
            Phone = phone;
        }

        public Guid? Id { get; set; }
        public Guid? PersonId { get; set; }
        
        public string Name { get; set; }
        public string AddressStreet { get; set; }
        public string AddressNumber { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressZipcode { get; set; }
        public string CnpjCpf { get; set; }
        public string StateRegistration { get; set; }
        public string AddressCountry { get; set; }
        public string Phone { get; set; }

    }
}