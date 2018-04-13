using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace VehicleMonitoring.Common.Core.Extensions
{
    public static class EnumerableExtension
    {
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int n)
        {
            return source.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / n)
                .Select(x => x.Select(v => v.Value));
        }
    }
}
