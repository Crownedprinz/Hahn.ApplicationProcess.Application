using System;
using System.Collections.Generic;

namespace  HAF.Domain
{
    public class Bucket<T>
    {
        public Bucket(int bucketIndex, IEnumerable<T> items)
        {
            BucketIndex = bucketIndex;
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public int BucketIndex { get; }
        public IEnumerable<T> Items { get; }
    }
}