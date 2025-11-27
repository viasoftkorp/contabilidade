using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.LancamentoContabil.Servicos;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Repositorios;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Sagas.Messages;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Servicos;
using Microsoft.Extensions.Logging;
using Viasoft.Core.AmbientData;
using Viasoft.Core.AmbientData.Extensions;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.Identity.Abstractions.Store;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.PushNotifications.Abstractions.Notification;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApplicationService;

public class ConciliacaoContabilApplicationService: IConciliacaoContabilApplicationService, ITransientDependency
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAmbientData _ambientData;
    private readonly IUserStore _userStore ;
    private readonly IServiceBus _serviceBus;
    private readonly IConciliacaoContabilRepositorio _conciliacaoContabilRepositorio ;
    private readonly ITipoConciliacaoContabilRepositorio _tipoConciliacaoContabilRepositorio;
    private readonly ILancamentoContabilService _lancamentoContabilService;
    private readonly IApuracaoFactory _apuracaoFactory;
    private readonly ILogger<ConciliacaoContabilApplicationService> _logger;
    private readonly IConciliacaoContabilBatch _conciliacaoContabilBatch;
    private readonly IPushNotification _pushNotification;
    
    public ConciliacaoContabilApplicationService(IUnitOfWork unitOfWork, IConciliacaoContabilRepositorio conciliacaoContabilRepositorio, IAmbientData ambientData, 
        IUserStore userStore, IServiceBus serviceBus, ILancamentoContabilService lancamentoContabilService, IApuracaoFactory apuracaoFactory, ILogger<ConciliacaoContabilApplicationService> logger, IConciliacaoContabilBatch conciliacaoContabilBatch, ITipoConciliacaoContabilRepositorio tipoConciliacaoContabilRepositorio, IPushNotification pushNotification)
    {
        _unitOfWork = unitOfWork;
        _conciliacaoContabilRepositorio = conciliacaoContabilRepositorio;
        _ambientData = ambientData;
        _userStore = userStore;
        _serviceBus = serviceBus;
        _lancamentoContabilService = lancamentoContabilService;
        _apuracaoFactory = apuracaoFactory;
        _logger = logger;
        _conciliacaoContabilBatch = conciliacaoContabilBatch;
        _tipoConciliacaoContabilRepositorio = tipoConciliacaoContabilRepositorio;
        _pushNotification = pushNotification;
    }
	
    public async Task ApurarLancamentosContabeis(Guid id)
    {
        _logger.LogInformation("Inserindo Lancamentos {id}",id);
        var conciliacaoContabil = await _conciliacaoContabilRepositorio.BuscarConciliacaoContabil(id);
        _logger.LogInformation("Buscou conciliacao lancamento {id} do tipo {conciliacaoContabil}", id, conciliacaoContabil.TipoConciliacaoContabil.TipoApuracao.ToString());

        await conciliacaoContabil.ApurarLancamentosContabeis(_lancamentoContabilService);
        _logger.LogInformation("Buscou Lancamento {id} do tipo {conciliacaoContabil}", id, conciliacaoContabil.TipoConciliacaoContabil.TipoApuracao.ToString());
        
        using (_unitOfWork.Begin(l => l.LazyTransactionInitiation = false))
        {
            await _conciliacaoContabilBatch.InserirBatch(conciliacaoContabil.Lancamentos);
            //Estamos limpando pois decidimos utilizar o EFBulk para inserir em batch
            //pois não foi possivel configurar o EFCORE para inserir mais de 42 registros por vez, e para nao inserir duplicado precisamos limpar a lista
            conciliacaoContabil.Lancamentos.Clear();
            foreach (var comando in conciliacaoContabil.ComandosLocais)
            {
                await _serviceBus.SendLocal(comando);
            }
            await _unitOfWork.CompleteAsync();  
        }
        _logger.LogInformation("Inseriu lancamento  {id} do tipo {conciliacaoContabil}", id, conciliacaoContabil.TipoConciliacaoContabil.TipoApuracao.ToString());    

    }

    public async Task FalhouInesperadamente(Guid id, string erro)
    {
        var conciliacaoContabil = await _conciliacaoContabilRepositorio.BuscarConciliacaoContabil(id);
        conciliacaoContabil.FalhouInesperadamente(erro);
        
        using (_unitOfWork.Begin())
        {
            foreach (var comando in conciliacaoContabil.ComandosLocais)
            {
                await _serviceBus.SendLocal(comando);
            }
            await _unitOfWork.CompleteAsync();  
        }
    }

    public async Task FinalizarConciliacao(Guid id)
    {
        var conciliacaoContabil = await _conciliacaoContabilRepositorio.BuscarConciliacaoContabilLancamentosApuracoes(id);
        if (conciliacaoContabil.FinalizouConciliacaoApuracao() && conciliacaoContabil.FinalizouConciliacaoLancamentos())
        {
            using (_unitOfWork.Begin())
            {
                conciliacaoContabil.Finalizar();
                await _pushNotification.SendCommandAsync("ConciliacaoContabilFinalizada", conciliacaoContabil.LegacyId);
                await _unitOfWork.CompleteAsync();
            } 
        }
    }

    public async Task<ListResultDto<BuscarConciliacaoContabilOutput>> BuscarTodasConciliacoesContabeis(BuscarTodasConciliacoesContabeisInput input)
    {
        return await _conciliacaoContabilRepositorio.BuscarTodasConciliacoesContabeis(input);
    }

    public async Task ApurarTipo(Guid id)
    {
        _logger.LogInformation("Inserindo apuração {id}",id);
        
        var conciliacaoContabil = await _conciliacaoContabilRepositorio.BuscarConciliacaoContabil(id);
        _logger.LogInformation("Buscou conciliacao apuracao {id} do tipo {conciliacaoContabil}", id, conciliacaoContabil.TipoConciliacaoContabil.TipoApuracao.ToString());
       
        var apuracaoDomainService = _apuracaoFactory.CriarApuracao(conciliacaoContabil);
        await conciliacaoContabil.Apurar(apuracaoDomainService);
        _logger.LogInformation("Buscou apuracao {id} do tipo {conciliacaoContabil}", id, conciliacaoContabil.TipoConciliacaoContabil.TipoApuracao.ToString());

        using (_unitOfWork.Begin(l => l.LazyTransactionInitiation = false))
        {
            await _conciliacaoContabilBatch.InserirBatch(conciliacaoContabil.Apuracoes);
            //Estamos limpando pois decidimos utilizar o EFBulk para inserir em batch
            //pois não foi possivel configurar o EFCORE para inserir mais de 42 registros por vez, e para nao inserir duplicado precisamos limpar a lista
            conciliacaoContabil.Apuracoes.Clear();
            foreach (var comando in conciliacaoContabil.ComandosLocais)
            {
                await _serviceBus.SendLocal(comando);
            }
            await _unitOfWork.CompleteAsync();  
        }
        
        _logger.LogInformation("Inseriu apuracao  {id} do tipo {conciliacaoContabil}", id, conciliacaoContabil.TipoConciliacaoContabil.TipoApuracao.ToString());    
    }
    
    public async Task ConciliarLancamentoApuracao(Guid id)
    {
        _logger.LogInformation("Conciliando Lancamento apuracao  {id}", id);    

        var conciliacaoContabil = await _conciliacaoContabilRepositorio.BuscarConciliacaoContabilLancamentosApuracoes(id);
        _logger.LogInformation("Buscou Lancamento Apuracao conciliacao {id} do tipo {conciliacaoContabil}", id, conciliacaoContabil.TipoConciliacaoContabil.TipoApuracao.ToString());

        // somente podemos conciliar os lançamentos quando as apurações tiverem sido inseridos
        if (!conciliacaoContabil.FinalizouApuracao())
        {
            await _serviceBus.DeferLocal(TimeSpan.FromSeconds(5), new ConciliarLancamentoApuracaoCommand { IdConciliacaoContabil = id });
            _logger.LogInformation("Apuracoes não finalizaram de inserir, delay de 5 segundos para a proxima {id}", id);

            return;
        }
        
        using (_unitOfWork.Begin(l => l.LazyTransactionInitiation = false))
        {
            conciliacaoContabil.ConciliarLancamentos();
            await _conciliacaoContabilBatch.InserirBatch(conciliacaoContabil.Lancamentos.SelectMany(l => l.LancamentosDetalhamento).ToList());
            //Estamos limpando pois decidimos utilizar o EFBulk para inserir em batch
            //pois não foi possivel configurar o EFCORE para inserir mais de 42 registros por vez, e para nao inserir duplicado precisamos limpar a lista
            foreach (var lancamento in  conciliacaoContabil.Lancamentos)
            {
                lancamento.LancamentosDetalhamento.Clear();
            }
            foreach (var evento in conciliacaoContabil.EventosLocais)
            {
                await _serviceBus.SendLocal(evento);
            }
            await _unitOfWork.CompleteAsync();  
        }
        
        _logger.LogInformation("Conciliou Lancamento Apuracao {id} do tipo {conciliacaoContabil}", id, conciliacaoContabil.TipoConciliacaoContabil.TipoApuracao.ToString());
    }

    public async Task ConciliarApuracaoLancamento(Guid id)
    {
        _logger.LogInformation("Conciliando apuracao Lancamento  {id}", id);    
        var conciliacaoContabil = await _conciliacaoContabilRepositorio.BuscarConciliacaoContabilLancamentosApuracoes(id);
        _logger.LogInformation("Buscou Apuracao Lancamento conciliacao {id} do tipo {conciliacaoContabil}", id, conciliacaoContabil.TipoConciliacaoContabil.TipoApuracao.ToString());

        // somente podemos conciliar as apurações quando os lançamentos tiverem sido inseridos
        if (!conciliacaoContabil.FinalizouApuracaoLancamentos())
        {
            await _serviceBus.DeferLocal(TimeSpan.FromSeconds(5), new ConciliarApuracaoLancamentoCommand { IdConciliacaoContabil = id });
            _logger.LogInformation("Apuracoes não finalizaram de inserir, delay de 5 segundos para a proxima {id}", id);
            return;
        }
        
        using (_unitOfWork.Begin(l => l.LazyTransactionInitiation = false))
        {
            conciliacaoContabil.ConciliarApuracao();
            await _conciliacaoContabilBatch.InserirBatch(conciliacaoContabil.Apuracoes.SelectMany(l => l.ApuracoesDetalhamento).ToList());
            //Estamos limpando pois decidimos utilizar o EFBulk para inserir em batch
            //pois não foi possivel configurar o EFCORE para inserir mais de 42 registros por vez, e para nao inserir duplicado precisamos limpar a lista
            foreach (var apuracao in  conciliacaoContabil.Apuracoes)
            {
                apuracao.ApuracoesDetalhamento.Clear();
            }
            foreach (var evento in conciliacaoContabil.EventosLocais)
            {
                await _serviceBus.SendLocal(evento);
            }
            await _unitOfWork.CompleteAsync();  
        }
        _logger.LogInformation("Conciliou Apuracao Lancamento {id} do tipo {conciliacaoContabil}", id, conciliacaoContabil.TipoConciliacaoContabil.TipoApuracao.ToString());

    }

    public async Task<ConciliacaoContabilOutput> CriarConciliacaoContabil(CriarConciliacaoContabilInput input)
    {
        var usuario = await _userStore.GetUserDetailsAsync(_ambientData.GetUserId());
        using (_unitOfWork.Begin())
        {
            List<ConciliacaoContabil> conciliacoesCriadas = new List<ConciliacaoContabil>();

            foreach (var tipo in input.TipoApuracoes)
            {
                var tipoApuracao = await _tipoConciliacaoContabilRepositorio.BuscarConciliacaoContabilTipo(tipo);
                var conciliacaoContabil = new ConciliacaoContabil(tipoApuracao, input.Empresas)
                {
                    Descricao = input.Descricao + " " +tipo.Descricao(),
                    DataHora = DateTime.Now,
                    DataInicial = DateOnly.Parse(DateTime.ParseExact(input.DataInicial, "yyyyMMdd", null).ToString("yyyy-MM-dd")),
                    DataFinal = DateOnly.Parse(DateTime.ParseExact(input.DataFinal, "yyyyMMdd", null).ToString("yyyy-MM-dd")),
                    Usuario = usuario.Login 
                };
                
                await _conciliacaoContabilRepositorio.Create(conciliacaoContabil);
                
                conciliacoesCriadas.Add(conciliacaoContabil);
                await _conciliacaoContabilRepositorio.Create(conciliacaoContabil);
                foreach (var comando in conciliacaoContabil.ComandosLocais)
                {
                    await _serviceBus.SendLocal(comando);   
                }
            }
            await _unitOfWork.CompleteAsync();
            var output =  new ConciliacaoContabilOutput();
            foreach (var conciliacao in conciliacoesCriadas)
            {
                output.idsConciliacao.Add(conciliacao.LegacyId);
            }
            return output;
        }
    }

    public async Task IniciarConciliacaoContabil(Guid idConciliacaoContabil)
    {
        var conciliacaoContabil = await _conciliacaoContabilRepositorio.BuscarConciliacaoContabil(idConciliacaoContabil);
        using (_unitOfWork.Begin())
        {
            conciliacaoContabil.Iniciar();
            foreach (var comando in conciliacaoContabil.ComandosLocais)
            {
                await _serviceBus.SendLocal(comando);
            }
            await _unitOfWork.CompleteAsync();
        }
    }
    
    public async Task AtualizarConciliacaoContabil(AtualizarConciliacaoContabilInput input)
    {
        using (_unitOfWork.Begin())
        {
            foreach (var tipo in input.TipoApuracoes)
            {
                await _conciliacaoContabilRepositorio.Update(input, tipo);
            }
            await _unitOfWork.CompleteAsync();
        }
    }

    public async Task DeletarConciliacaoContabil(int legacyId)
    {
        using (_unitOfWork.Begin())
        {
            await _conciliacaoContabilRepositorio.Delete(legacyId);
            await _unitOfWork.CompleteAsync();
        }
    }

    public async Task<BuscarConciliacaoContabilOutput> BuscarConciliacaoContabil(Guid id)
    {
        var conciliacaoContabil = await _conciliacaoContabilRepositorio.BuscarConciliacaoContabil(id);
        return null;
    }
    
    public async Task<BuscarConciliacaoContabilOutput> BuscarConciliacaoContabilPorLegacyId(int legacyId)
    {
        return await _conciliacaoContabilRepositorio.BuscarConciliacaoContabilPorLegacyId(legacyId);
    }
}