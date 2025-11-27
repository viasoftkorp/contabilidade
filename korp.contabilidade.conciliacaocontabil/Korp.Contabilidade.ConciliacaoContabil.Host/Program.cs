using System.Threading.Tasks;
using Viasoft.Core.WebHost;

namespace Korp.Contabilidade.ConciliacaoContabil.Host
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await ViasoftCoreWebHost.Main<Startup>(args);
        }
    }
}