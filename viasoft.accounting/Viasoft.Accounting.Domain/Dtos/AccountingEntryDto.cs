using System;
using Viasoft.Accounting.Domain.Enums;
using Viasoft.Core.DDD.Application.Dto.Entities;

namespace Viasoft.Accounting.Domain.Dtos
{
    public class AccountingEntryDto : FullAuditedEntityDto
    {
        public string EntryType { get; set; }   
        
        public string EntryDateLegacy { get; set; }

        public string CreationTimeLegacy { get; set; }


        public int? AccountingYear { get; set; }        
        public int? AccountingMonth { get; set; }
        
        public string Notes { get; set; }        
       
        public string Customer { get; set; }
        
        public int CompanyCode { get; set; }   
        
        public string Series { get; set; }        

        public EntrySourceType? SourceType { get; set; }
        
        public Guid? SourceId { get; set; }
        
        public DateTime? EntryDate{ get; set; }
        public string Status { get; set; }
        
        public string Usuario { get; set; }
        public string Fornecedor { get; set; }
        public string Entrada { get; set; }
        public int? LegacyIdContaReceber { get; set; }


    }
}