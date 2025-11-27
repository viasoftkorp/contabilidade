namespace Viasoft.Accounting.Domain.Contracts.Ctes
{
    public class CteTaxesDto
    {
        public string IcmsCst { get; set; }
        public decimal? IcmsBase { get; set; }
        public decimal? IcmsTax{ get; set; }
        public decimal? IcmsRedBase { get; set; }
        public decimal? IcmsDeferment { get; set; }
        public decimal? IcmsTotal { get; set; }
        public decimal? PisBase { get; set; }
        public decimal? PisTax { get; set; }
        public decimal? PisTaxWithholding { get; set; }
        public string PisCst { get; set; }
        public decimal? PisTotal { get; set; }
        public decimal? PisTotalWithholding { get; set; }
        public decimal? CofinsBase { get; set; }
        public decimal? CofinsTax { get; set; }
        public decimal? CofinsTaxWithholding { get; set; }
        public string CofinsCst { get; set; }
        public decimal? CofinsTotal { get; set; }
        public decimal? CofinsTotalWithholding { get; set; }
        public decimal? InssBase { get; set; }
        public decimal? InssTax { get; set; }
        public decimal? InssTotal { get; set; }
        public decimal? IrBase { get; set; }
        public decimal? IrTax { get; set; }
        public decimal? IrTotal { get; set; }
        public decimal? CsllBase { get; set; }
        public decimal? CsllTax { get; set; }
        public decimal? CsllTotal { get; set; }
    }
}