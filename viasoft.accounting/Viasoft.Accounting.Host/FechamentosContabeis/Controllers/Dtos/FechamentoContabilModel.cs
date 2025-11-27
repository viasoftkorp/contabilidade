using System;
using System.Globalization;
using Viasoft.Accounting.Domain.Entities;

namespace Viasoft.Accounting.Host.PeriodosContabeis.Controllers.Dtos;

public class FechamentoContabilModel
{
    public Guid Id { get; set; }

    public DateTime Data { get; set; }
    public FechamentoContabilModel()
    {
        
    }

    public FechamentoContabilModel(LegacyFechamentoPeriodoContabil legacyFechamentoPeriodoContabil)
    {
        Id = legacyFechamentoPeriodoContabil.Id;
        Data = legacyFechamentoPeriodoContabil.Data;
    }
}