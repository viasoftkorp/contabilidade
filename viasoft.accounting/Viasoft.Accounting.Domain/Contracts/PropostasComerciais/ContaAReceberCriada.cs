using System;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Accounting.Domain.Contracts.PropostasComerciais;

[Endpoint("Viasoft.FinancialAccounting.AccountsReceivable.ContaAReceberCriada")]
public class ContaAReceberCriada : IEvent
{ 
    public Guid IdOperacaoContabil { get; set; }
    public Guid IdCliente { get; set; }
    public DateTime? DataEmissao { get; set; }
    public string Documento { get; set; }
    public Guid IdContaReceber { get; set; }
    public int LegacyIdContaReceber { get; set; }
    public decimal Juros { get; set; }
    public decimal Valor { get; set; }
    public decimal ValorLiquido { get; set; }
    public bool Adiantamento { get; set; }
}