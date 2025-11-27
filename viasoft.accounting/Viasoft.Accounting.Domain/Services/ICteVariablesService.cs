using System.Collections.Generic;
using Viasoft.Accounting.Domain.Contracts;
using Viasoft.Accounting.Domain.Contracts.Ctes;

namespace Viasoft.Accounting.Domain.Services
{
    public interface ICteVariablesService
    {
        decimal? GetValueFromCteVariable(CteIssued cte, string variable);

        IEnumerable<string> GetCteTaxVariablesForSelect();

        IEnumerable<string> GetCteAccountingVariablesForSelect();

    }
}