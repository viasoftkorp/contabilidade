using System;
using System.Collections.Generic;
using System.Linq;
using Viasoft.Accounting.Domain.Contracts;
using Viasoft.Accounting.Domain.Contracts.Ctes;
using Viasoft.Accounting.Domain.Enums;
using Viasoft.Core.IoC.Abstractions;

namespace Viasoft.Accounting.Domain.Services
{
    public class CteVariablesService : ICteVariablesService, ITransientDependency
    {
        private Dictionary<string, Func<CteIssued, decimal?>> TaxVariables { get; set; }
        private Dictionary<string, Func<CteIssued, decimal?>> AccountingVariables { get; set; }

        public CteVariablesService()
        {
            TaxVariables = new Dictionary<string, Func<CteIssued, decimal?>>();
            AccountingVariables = new Dictionary<string, Func<CteIssued, decimal?>>();
            
            foreach (var variable in Enum.GetNames(typeof(CteValuesVariable)))
            {
                TaxVariables[variable] = GetValueFunction<CteIssued>(variable, "Values");
            }   
               foreach (var variable in Enum.GetNames(typeof(CteTaxesVariable)))
               {
                   // TODO removed because taxCalculation doesn't allow the use of another taxValue, that will be implemented in the future (CTE-214)
                   // TaxVariables[variable] = GetValueFunction<CteIssued>(variable, "Taxes");
                   AccountingVariables[variable] = GetValueFunction<CteIssued>(variable,  "Taxes");
               }           
            foreach (var variable in Enum.GetNames(typeof(CteVariable)))
            {
                TaxVariables[variable] = GetValueFunction<CteIssued>(variable);
            }
            
            AccountingVariables["Total"] = GetValueFunction<CteIssued>("Total",  "Values");


        }

        private static Func<T, decimal?> GetValueFunction<T>(string prop, string complexType)
        {
            return arg => (decimal?) arg.GetType().GetProperty(complexType)?.PropertyType.GetProperty(prop)?.GetValue(arg.GetType()?.GetProperty(complexType)?.GetValue(arg));
        }       
        private static Func<T, decimal?> GetValueFunction<T>(string prop)
        {
            return arg => (decimal?) arg.GetType()?.GetProperty(prop)?.GetValue(arg);
        }


        public decimal? GetValueFromCteVariable(CteIssued cte, string variable)
        {
            return TaxVariables[variable](cte) ?? 0;
        }

        public IEnumerable<string> GetCteTaxVariablesForSelect()
        {
            return TaxVariables.Keys.ToList();
        }

        public decimal? GetValueFromAccountingValueProperty(CteIssued cte, string variable)
        {
            return AccountingVariables[variable](cte);
        }

        public IEnumerable<string> GetCteAccountingVariablesForSelect()
        {
            return AccountingVariables.Keys.ToList();
        }
    }
}