namespace Viasoft.Accounting.Domain.Contracts.Ctes
{
    public class CteTaxConfigurationDto
    {
        public decimal PisTaxRate { get; set; }
        public decimal PisTaxRateWithholding { get; set; }
        public decimal CofinsTaxRate { get; set; }
        public decimal CofinsTaxRateWithholding { get; set; }
        public decimal InssTaxRate { get; set; }
        public decimal IrTaxRate { get; set; }
        public decimal CsllTaxRate { get; set; }
    }
}