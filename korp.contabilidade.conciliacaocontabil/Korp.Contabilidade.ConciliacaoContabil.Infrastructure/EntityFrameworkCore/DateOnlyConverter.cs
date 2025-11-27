using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Korp.Contabilidade.ConciliacaoContabil.Infrastructure.EntityFrameworkCore;

public class DateOnlyConverter: ValueConverter<DateOnly,string>
{
    public DateOnlyConverter() : base(l => l.ToString("yyyyMMdd"), s => DateOnly.ParseExact(s,"yyyyMMdd"))
    {
        
    }
}

public class DateOnlyNullableConverter: ValueConverter<DateOnly?,string>
{
    public DateOnlyNullableConverter() : base(l => l.HasValue ? l.Value.ToString("yyyyMMdd") : null, s => string.IsNullOrWhiteSpace(s) ? null : DateOnly.ParseExact(s,"yyyyMMdd"))
    {
        
    }
}