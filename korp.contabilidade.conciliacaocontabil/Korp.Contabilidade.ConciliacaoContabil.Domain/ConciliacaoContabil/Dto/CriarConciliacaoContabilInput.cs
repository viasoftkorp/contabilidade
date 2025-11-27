using System.Collections.Generic;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public class CriarConciliacaoContabilInput
{
    public string Descricao { get; set; }
    public string DataInicial { get; set; }
    public string DataFinal { get; set; }
    public List<int> Empresas { get; set; }
    public List<TipoApuracaoConciliacaoContabil> TipoApuracoes { get; set; }
}