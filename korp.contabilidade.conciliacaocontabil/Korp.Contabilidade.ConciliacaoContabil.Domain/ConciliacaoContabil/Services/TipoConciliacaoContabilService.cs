using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Repositorios;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.IoC.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Services;

public class TipoConciliacaoContabilService : ITipoConciliacaoContabilService, ITransientDependency
{
    private readonly ITipoConciliacaoContabilRepositorio _tipoConciliacaoContabilRepositorio;
    private readonly IPlanoContaRepositorio _planoContaRepositorio;
    private readonly IUnitOfWork _unitOfWork;

    public TipoConciliacaoContabilService(ITipoConciliacaoContabilRepositorio tipoConciliacaoContabilRepositorio, IPlanoContaRepositorio planoContaRepositorio, IUnitOfWork unitOfWork)
    {
        _tipoConciliacaoContabilRepositorio = tipoConciliacaoContabilRepositorio;
        _planoContaRepositorio = planoContaRepositorio;
        _unitOfWork = unitOfWork;
    }

    public async Task<AdicionarContaResponseEnum> AdicionarConta(int legacyId, AdicionarContaInput input)
    {
        var tipoConciliacao = await _tipoConciliacaoContabilRepositorio.BuscarTipoConciliacaoContabilPorId(legacyId);
        
        if (tipoConciliacao == null)
        {
            return AdicionarContaResponseEnum.ConciliacaoNaoEncontrada;
        }

        var tipoConciliacaoContas = await BuscarTodasContasPorConciliacao(legacyId, new PagedFilteredAndSortedRequestInput());

        var contaExiste = tipoConciliacaoContas.Items.Any(conta => conta.CodigoConta == input.CodigoConta);
        
        if (contaExiste)
        {
            return AdicionarContaResponseEnum.ContaJaAdicionada;
        }

        var isContaVirtual = await _planoContaRepositorio.IsContaVirtual(input.CodigoConta);
        if (isContaVirtual && input.ShouldAddLinkedAccounts is null)
        {
            // Conta Virtual selecionada, mas ainda não foi definido se deve ou não fazer o link com as contas vinculadas
            return AdicionarContaResponseEnum.ContaVirtualSemAcaoDefinida;
        }

        var contasVinculadas = new List<PlanoConta>();
        if (isContaVirtual && input.ShouldAddLinkedAccounts.Value)
        {
            contasVinculadas = await _planoContaRepositorio.GetContasVinculadas(input.CodigoConta, tipoConciliacao.LegacyId);
        }

        using (_unitOfWork.Begin())
        {
            await _tipoConciliacaoContabilRepositorio.AdicionarConta(new TipoConciliacaoContabilConta
            {
                CodigoConta = input.CodigoConta,
                Descricao = input.Descricao,
                IdTipoConciliacaoContabil = tipoConciliacao.LegacyId
            });
            if (contasVinculadas.Any())
            {
                var contasVinculadasToAdd = contasVinculadas.Select(c => new TipoConciliacaoContabilConta
                {
                    CodigoConta = c.Codigo,
                    Descricao = c.Descricao,
                    IdTipoConciliacaoContabil = tipoConciliacao.LegacyId,
                });
                await _tipoConciliacaoContabilRepositorio.AdicionarContas(contasVinculadasToAdd);
            }

            await _unitOfWork.CompleteAsync();
        }

        return AdicionarContaResponseEnum.Ok;
    }

    public async Task<ListResultDto<TipoConciliacaoContabilConta>> BuscarTodasContasPorConciliacao(int legacyId, PagedFilteredAndSortedRequestInput input)
    {
        return await _tipoConciliacaoContabilRepositorio.BuscarTodasContasPorConciliacao(legacyId, input);
    }

    public async Task<RemoverContaResponseEnum> DeletarConta(int legacyId, int id, bool? shouldRemoveLinkedAccounts)
    {
        var conta = await _tipoConciliacaoContabilRepositorio.GetConta(legacyId, id);
        if (conta is null)
        {
            return RemoverContaResponseEnum.Ok;
        }
        var isContaVirtual = await _planoContaRepositorio.IsContaVirtual(conta.CodigoConta);
        if (isContaVirtual && shouldRemoveLinkedAccounts is null)
        {
            // Conta Virtual selecionada, mas ainda não foi definido se deve ou não fazer o link com as contas vinculadas
            return RemoverContaResponseEnum.ContaVirtualSemAcaoDefinida;
        }

        var contaVinculadaIds = new List<int>();
        if (isContaVirtual && shouldRemoveLinkedAccounts.Value)
        {
            contaVinculadaIds = await _planoContaRepositorio.GetContasVinculadasIds(conta.CodigoConta);
        }
        using (_unitOfWork.Begin())
        {
            await _tipoConciliacaoContabilRepositorio.DeletarConta(legacyId, id);
            if (contaVinculadaIds.Any())
            {
                await _tipoConciliacaoContabilRepositorio.DeletarContas(legacyId, contaVinculadaIds);
            }
            await _unitOfWork.CompleteAsync();
        }

        return RemoverContaResponseEnum.Ok;
    }
}