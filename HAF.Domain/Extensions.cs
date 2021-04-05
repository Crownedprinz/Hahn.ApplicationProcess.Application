using System.Collections.Generic;
using System.Linq;

namespace  HAF.Domain
{
    public static class Extensions
    {
        public static IEnumerable<Bucket<T>> ToBuckets<T>(this IEnumerable<T> source, int bucketSize)
        {
            return source.Select((x, i) => new { Item = x, Bucket = i / bucketSize })
                .GroupBy(x => x.Bucket, x => x.Item)
                .Select(x => new Bucket<T>(x.Key, x));
        }
    }
}