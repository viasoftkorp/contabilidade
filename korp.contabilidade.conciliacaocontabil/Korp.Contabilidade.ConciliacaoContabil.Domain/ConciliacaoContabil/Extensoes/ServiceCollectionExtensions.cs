using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Cofins.Creditar;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Cofins.Debitar;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Pis.Creditar;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Pis.Debitar;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CofinsOutrosLancamentos = Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Cofins.Creditar.CofinsOutrosLancamentos;
using CofinsRegistroF100 = Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Cofins.Creditar.CofinsRegistroF100;
using PisOutrosLancamentos = Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Pis.Creditar.PisOutrosLancamentos;
using PisRegistroF100 = Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Pis.Creditar.PisRegistroF100;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AdicionarRepositoriosImpostos(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient<ICofinsCreditarRepositorio, CofinsNotaEntradaRepositorio>());
        serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient<ICofinsCreditarRepositorio, CofinsOutrosLancamentos>());
        serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient<ICofinsCreditarRepositorio, CofinsRegistroF100>());
        serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient<ICofinsCreditarRepositorio, CofinsRegistroF120>());
        serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient<ICofinsCreditarRepositorio, CofinsRegistroF130>());
        serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient<ICofinsDebitarRepositorio, CofinsNotaSaidaRepositorio>());
        serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient<ICofinsDebitarRepositorio, Apuracoes.Impostos.Repositorios.Cofins.Debitar.CofinsRegistroF100>());
        serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient<ICofinsDebitarRepositorio, Apuracoes.Impostos.Repositorios.Cofins.Debitar.CofinsOutrosLancamentos>());
        serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient<IPisCreditarRepositorio, PisNotaEntradaRepositorio>());
        serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient<IPisCreditarRepositorio, PisOutrosLancamentos>());
        serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient<IPisCreditarRepositorio, PisRegistroF100>());
        serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient<IPisCreditarRepositorio, PisRegistroF120>());
        serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient<IPisCreditarRepositorio, PisRegistroF130>());
        serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient<IPisDebitarRepositorio, PisNotaSaidaRepositorio>());
        serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient<IPisDebitarRepositorio, Apuracoes.Impostos.Repositorios.Pis.Debitar.PisRegistroF100>());
        serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient<IPisDebitarRepositorio, Apuracoes.Impostos.Repositorios.Pis.Debitar.PisOutrosLancamentos>());
        return serviceCollection;
    }
}