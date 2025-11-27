using System;

namespace Viasoft.Accounting.Host.Providers.Pessoas
{
    public class PersonOutput
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string CnpjCpf { get; set; }
        public string Code { get; set; }
        public string TradingName { get; set; }
        public string Email { get; set; }
        public decimal? IrTaxRate { get; set; }
        public int? PaymentCondition { get; set; }
        public string FreightCode { get; set; }
        public string RedispatchFreightCode { get; set; }
        public Guid? ShippingCompanyId { get; set; }
        public string RepresentativeCode { get; set; }
        public Guid? RepresentativeId { get; set; }
        public string RedispatchShippingCompanyCode { get; set; }
        public Guid? RedispatchShippingCompanyId { get; set; }
    }
}
