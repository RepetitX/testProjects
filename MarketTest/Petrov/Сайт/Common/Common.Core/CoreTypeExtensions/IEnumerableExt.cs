﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Core
{
    public static class IEnumerableExt
    {
        public static IEnumerable<T> Traverse<T>
               (this IEnumerable<T> source, Func<T, IEnumerable<T>> fnRecurse)
        {
            foreach (T item in source)
            {
                yield return item;

                var seqRecurse = fnRecurse(item);

                if (seqRecurse != null)
                {
                    foreach (T itemRecurse in Traverse(seqRecurse, fnRecurse))
                    {
                        yield return itemRecurse;
                    }
                }
            }
        }
    }
}
