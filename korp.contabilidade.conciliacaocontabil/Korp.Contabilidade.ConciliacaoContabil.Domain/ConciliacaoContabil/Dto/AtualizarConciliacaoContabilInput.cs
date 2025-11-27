using System;
using System.Collections.Generic;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public class AtualizarConciliacaoContabilInput
{
    public Guid Id { get; set; }
    public bool Conciliado { get; set; }
    public List<int> Empresas { get; set; }
    public List<TipoApuracaoConciliacaoContabil> TipoApuracoes { get; set; }
}