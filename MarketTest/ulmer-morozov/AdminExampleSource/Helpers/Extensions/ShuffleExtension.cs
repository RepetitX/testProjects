using System;
using System.Collections.Generic;
using System.Linq;

namespace Helpers.Extensions
{
    public static class ShuffleExtension
    {
        private static readonly Random Random = new Random();
        private static readonly object SyncLock = new object();

        public static T[] Shuffle<T>(this IEnumerable<T> array)
        {
            lock (SyncLock)
            {
                var myRandomArray = array.OrderBy(x => Random.Next()).ToArray();
                return myRandomArray;
            }

        }
    }
}