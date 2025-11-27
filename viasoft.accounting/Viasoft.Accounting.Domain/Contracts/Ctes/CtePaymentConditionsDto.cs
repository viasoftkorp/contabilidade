namespace Viasoft.Accounting.Domain.Contracts.Ctes
{
    public class CtePaymentConditionsDto
    {
        public int? PaymentConditionId { get; set; }
        
        public string PaymentConditionDescription { get; set; }
        
        public bool IsManualInstallment { get; set; }
    }
}