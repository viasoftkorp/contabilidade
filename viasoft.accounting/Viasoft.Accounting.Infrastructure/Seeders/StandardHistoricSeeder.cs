using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Viasoft.Accounting.Domain.Entities;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Data.Seeder.Abstractions;

namespace Viasoft.Accounting.Infrastructure.Seeders
{
    public class StandardHistoricSeeder : ISeedData
    {
        private readonly IRepository<AccountingEntryHistory> _standardHistorics;
        private readonly IUnitOfWork _unitOfWork;

        public StandardHistoricSeeder(IRepository<AccountingEntryHistory> standardHistorics, IUnitOfWork unitOfWork)
        {
            _standardHistorics = standardHistorics;
            _unitOfWork = unitOfWork;
        }

        public async Task SeedDataAsync()
        {
            var historics = _standardHistorics.Where(e => e.IsStatic && e.Module == "CTE"); 

                var initialHistorics = new List<string>(new string[]
                    {
                        "Vlr. Total CTe",
                        "Vlr. Ref. PIS CTe",
                        "Vlr. Ref. Cofins CTe",
                        "Vlr. Ref. ICMS CTe",
                        "Vlr. Ref. INSS CTe",
                        "Vlr. Ref. IR CTe",
                        "Vlr. Ref. CSLL CTe",
                        "Recto. CTe"
                    }
                );

                var historicsToAdd = new List<AccountingEntryHistory>();

                var code = 0;

                foreach (var initialHistoric in initialHistorics)
                {
                    if (historics.Any(e => e.Description == initialHistoric))
                    {
                       continue;
                    }

                    var historic = new AccountingEntryHistory()
                    {
                        Description = initialHistoric,
                        IsStatic = true,
                        Module = "CTE",
                        Code = "CTE"+(++code),
                        Id = Guid.NewGuid()
                    };

                    historicsToAdd.Add(historic);
                }

                using (_unitOfWork.Begin())
                {
                    foreach (var history in historicsToAdd)
                    {
                        await _standardHistorics.InsertAsync(history);
                    }

                    await _unitOfWork.CompleteAsync();
                }
        }

    }
}