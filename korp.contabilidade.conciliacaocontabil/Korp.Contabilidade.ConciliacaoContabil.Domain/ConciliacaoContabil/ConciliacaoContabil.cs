using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.LancamentoContabil.Servicos;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Sagas.Messages;
using Viasoft.Core.DDD.Entities;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil;

[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
public class ConciliacaoContabil: Entity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LegacyId { get; set; }
    public string Descricao { get; set; }
    public string Usuario { get; set; }
    public DateTime DataHora { get; set; }
    public DateOnly DataInicial { get; set; }
    public DateOnly DataFinal { get; set; }
    public bool Conciliado { get; set; }
    public ConciliacaoContabilStatus? Status { get; set; }
    public byte[] Erro { get; set; }
    public int IdTipoConciliacaoContabil { get; set; }
    public List<ICommand> ComandosLocais { get; } = new();
    public List<IEvent> EventosLocais { get; } = new();
    public TipoConciliacaoContabil TipoConciliacaoContabil { get; set; } = new();
    public List<ConciliacaoContabilEmpresa> Empresas { get; } = new();
    public List<ConciliacaoContabilLancamento> Lancamentos { get;  } = new();
    public List<ConciliacaoContabilApuracao> Apuracoes { get;  } = new();
    public List<ConciliacaoContabilEtapa> Etapas { get; set; } = new();
    public ConciliacaoContabil() {}
    
    public ConciliacaoContabil(TipoConciliacaoContabil tipo, List<int> empresas) 
    {
        Id = Guid.NewGuid();
        TipoConciliacaoContabil = tipo;
        ComandosLocais.Add(new IniciarConciliacaoContabilCommand { IdConciliacaoContabil = Id });
        foreach (var idEmpresa in empresas)
        {
            var empresa = new ConciliacaoContabilEmpresa
            {
                LegacyCompanyId = idEmpresa
            };
            Empresas.Add(empresa);
        }
    }

    public async Task ApurarLancamentosContabeis(ILancamentoContabilService domainService)
    {
        var lancamentos = await domainService.ApurarLancamentoContabil(this);
        Lancamentos.AddRange(lancamentos);
        Etapas.Add(new ConciliacaoContabilEtapa
        {
            ProcessoGeracao = ProcessoGeracao.Lancamento,
            IdConciliacaoContabil = LegacyId
        }); 
        ComandosLocais.Add(new ConciliarLancamentoApuracaoCommand {IdConciliacaoContabil = Id});
    }
    
    public async Task Apurar(IApuracao domainService)
    {
        var apuracao = await domainService.Apurar(this);
        Apuracoes.AddRange(apuracao);
        Etapas.Add(new ConciliacaoContabilEtapa
        {
            ProcessoGeracao = ProcessoGeracao.Apuracao,
            IdConciliacaoContabil = LegacyId
        }); 
        ComandosLocais.Add(new ConciliarApuracaoLancamentoCommand { IdConciliacaoContabil = Id });
    }

    public void FalhouInesperadamente(string erro)
    {
        Status = ConciliacaoContabilStatus.Erro;
        Erro = Encoding.UTF8.GetBytes(erro);
    }
    
    public void ConciliarLancamentos()
    {
        // Conciliar Lancamentos com Apuracoes
        var apuracaoLookup = Apuracoes.ToLookup(a => a.Chave);

        foreach (var lancamento in Lancamentos)
        {
            var apuracoesCorrespondentes = apuracaoLookup[lancamento.Chave];
            if (!apuracoesCorrespondentes.Any())
            {
                lancamento.NaoConciliado();
                continue;
            }
            
            lancamento.ConciliadoComApuracoes(apuracoesCorrespondentes);
        }
        Etapas.Add(new ConciliacaoContabilEtapa
        {
            ProcessoGeracao = ProcessoGeracao.ConciliouLancamento,
            IdConciliacaoContabil = LegacyId
        }); 
        EventosLocais.Add(new FinalizouConciliacaoLancamentoEvent{IdConciliacaoContabil = Id});
    }
    
    public void ConciliarApuracao()
    {
        //Conciliar Apuracoes com Lancamentos
        var lancamentoLookup = Lancamentos.ToLookup(l => l.Chave);

        foreach (var apuracao in Apuracoes)
        {
            var lancamentosCorrespondentes = lancamentoLookup[apuracao.Chave];
            if (!lancamentosCorrespondentes.Any())
            {
                apuracao.NaoConciliada();
                continue;
            }
            
            apuracao.ConciliadoComLancamentos(lancamentosCorrespondentes);
        }
        Etapas.Add(new ConciliacaoContabilEtapa
        {
            ProcessoGeracao = ProcessoGeracao.ConciliouApuracao,
            IdConciliacaoContabil = LegacyId
        }); 
        EventosLocais.Add(new FinalizouConciliacaoApuracaoEvent{IdConciliacaoContabil = Id});
    }

    public bool FinalizouApuracao()
    {
        return Etapas.Any(l => l.ProcessoGeracao == ProcessoGeracao.Apuracao);
    }

    public bool FinalizouApuracaoLancamentos()
    {
        return Etapas.Any(l => l.ProcessoGeracao == ProcessoGeracao.Lancamento);
    }
    
    public bool FinalizouConciliacaoApuracao()
    {
        return Etapas.Any(l => l.ProcessoGeracao == ProcessoGeracao.ConciliouApuracao);
    }

    public bool FinalizouConciliacaoLancamentos()
    {
        return Etapas.Any(l => l.ProcessoGeracao == ProcessoGeracao.ConciliouLancamento);
    }

    public void Iniciar()
    {
        Status = ConciliacaoContabilStatus.Progresso;
        ComandosLocais.Add(new GerarApuracaoLancamentosContabilCommand { IdConciliacaoContabil = Id });
        ComandosLocais.Add(new GerarApuracaoCommand { IdConciliacaoContabil = Id });
    }

    public void Finalizar()
    {
        Status = ConciliacaoContabilStatus.Finalizado;
    }
}