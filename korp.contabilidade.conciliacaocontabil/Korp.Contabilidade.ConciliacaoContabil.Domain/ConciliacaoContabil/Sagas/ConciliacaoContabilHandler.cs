using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApplicationService;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Sagas.Messages;
using Rebus.Handlers;
using Rebus.Retry.Simple;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Sagas;

public class ConciliacaoContabilHandler: IHandleMessages<IniciarConciliacaoContabilMessage>, 
                                         IHandleMessages<GerarApuracaoLancamentosContabilMessage>,
                                         IHandleMessages<GerarApuracaoMessage>,
                                         IHandleMessages<IFailed<IniciarConciliacaoContabilMessage>>, 
                                         IHandleMessages<IFailed<GerarApuracaoLancamentosContabilMessage>>,
                                         IHandleMessages<IFailed<GerarApuracaoMessage>>,
                                         IHandleMessages<ConciliarLancamentoApuracaoMessage>,
                                         IHandleMessages<IFailed<ConciliarLancamentoApuracaoMessage>>,
                                         IHandleMessages<ConciliarApuracaoLancamentoMessage>,
                                         IHandleMessages<IFailed<ConciliarApuracaoLancamentoMessage>>,
                                         IHandleMessages<FinalizouConciliacaoLancamentoMessage>,
                                         IHandleMessages<IFailed<FinalizouConciliacaoLancamentoMessage>>,
                                         IHandleMessages<FinalizouConciliacaoApuracaoMessage>,
                                         IHandleMessages<IFailed<FinalizouConciliacaoApuracaoMessage>>
{

    private readonly IServiceBus _serviceBus;
    private readonly IConciliacaoContabilApplicationService _applicationService;
    
    public ConciliacaoContabilHandler(IServiceBus serviceBus, IConciliacaoContabilApplicationService applicationService)
    {
        _serviceBus = serviceBus;
        _applicationService = applicationService;
    }
    
    public async Task Handle(IniciarConciliacaoContabilMessage message)
    {
        await _applicationService.IniciarConciliacaoContabil(message.IdConciliacaoContabil);
    }

    public async Task Handle(GerarApuracaoLancamentosContabilMessage message)
    {
        await _applicationService.ApurarLancamentosContabeis(message.IdConciliacaoContabil);
    }

    public async Task Handle(GerarApuracaoMessage message)
    {
        await _applicationService.ApurarTipo(message.IdConciliacaoContabil);
    }
    
    public async Task Handle(ConciliarApuracaoLancamentoMessage message)
    {
        await _applicationService.ConciliarApuracaoLancamento(message.IdConciliacaoContabil);
    }
    
    public async Task Handle(ConciliarLancamentoApuracaoMessage message)
    {
        await _applicationService.ConciliarLancamentoApuracao(message.IdConciliacaoContabil);
    }

    public async Task Handle(IFailed<ConciliarLancamentoApuracaoMessage> message)
    {
        await _applicationService.FalhouInesperadamente(message.Message.IdConciliacaoContabil, message.ErrorDescription);
    }

    public async Task Handle(IFailed<ConciliarApuracaoLancamentoMessage> message)
    {
        await _applicationService.FalhouInesperadamente(message.Message.IdConciliacaoContabil, message.ErrorDescription);
    }
    
    public async Task Handle(IFailed<IniciarConciliacaoContabilMessage> message)
    {
        await _applicationService.FalhouInesperadamente(message.Message.IdConciliacaoContabil, message.ErrorDescription); 
    }

    public async Task Handle(IFailed<GerarApuracaoLancamentosContabilMessage> message)
    {
        await _applicationService.FalhouInesperadamente(message.Message.IdConciliacaoContabil, message.ErrorDescription);
    }

    public async Task Handle(IFailed<GerarApuracaoMessage> message)
    {
        await _applicationService.FalhouInesperadamente(message.Message.IdConciliacaoContabil, message.ErrorDescription);
    }

    public async Task Handle(FinalizouConciliacaoLancamentoMessage message)
    {
        await _applicationService.FinalizarConciliacao(message.IdConciliacaoContabil);
    }

    public async Task Handle(IFailed<FinalizouConciliacaoLancamentoMessage> message)
    {
        await _applicationService.FalhouInesperadamente(message.Message.IdConciliacaoContabil, message.ErrorDescription);
    }

    public async Task Handle(FinalizouConciliacaoApuracaoMessage message)
    {
        await _applicationService.FinalizarConciliacao(message.IdConciliacaoContabil);
    }

    public async Task Handle(IFailed<FinalizouConciliacaoApuracaoMessage> message)
    {
        await _applicationService.FalhouInesperadamente(message.Message.IdConciliacaoContabil, message.ErrorDescription);
    }
}