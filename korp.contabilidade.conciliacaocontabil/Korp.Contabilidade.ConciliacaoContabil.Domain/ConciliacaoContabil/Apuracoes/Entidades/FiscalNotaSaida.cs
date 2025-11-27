using System;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Viasoft.Core.DDD.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;

public class FiscalNotaSaida: Entity, IPeriodoContabilNota
{
    public int LegacyId { get; set; }
    public int LegacyCompanyId {get; set;}
    public DateOnly Data {get; set;}
    public int Documento {get; set;}
    public int Serie { get; set; }
    public string CodigoCliente {get; set;}
    public string RazaoSocial {get; set;}
    public string Situacao { get; set; }
    public bool Servico { get; set; }
}