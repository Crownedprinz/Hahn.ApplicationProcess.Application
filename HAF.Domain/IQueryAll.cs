using System.Collections.Generic;

namespace  HAF.Domain
{
    public interface IQueryAll<out TResult>
    {
        IEnumerable<TResult> Execute();
    }
}