using Viasoft.Accounting.Domain.Enums;

namespace Viasoft.Accounting.Domain.Contracts.Ctes
{
    public class CteCargoDto
    {
        public string PredominantItem { get; set; }
        public string Unit { get; set; }
        public CteCargoUnit Units { get; set; }
        public string MeasuringType { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalValue { get; set; }
        
        public decimal? RegisteredValue { get; set; }
        
        public string GeneralObs { get; set; }
        
        public string FiscalObs { get; set; }
    }
}