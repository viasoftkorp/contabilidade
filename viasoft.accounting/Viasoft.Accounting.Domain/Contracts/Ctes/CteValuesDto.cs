using System;

namespace Viasoft.Accounting.Domain.Contracts.Ctes
{
    public class CteValuesDto
    {
        public Guid ProductId { get; set; }
        
        public string ProductName { get; set; }
        
        public decimal? FreightWeightVolume { get; set; }
        
        public decimal? Freight { get; set; }
        
        public decimal? Dispatch { get; set; }
        
        public decimal? Toll { get; set; }
        
        public decimal? Gris { get; set; }
        
        public decimal? Itr{ get; set; }
        
        public decimal? SecCatCad { get; set; }
        
        public decimal? Others { get; set; }
        
        public decimal Total { get; set; }
    }
}