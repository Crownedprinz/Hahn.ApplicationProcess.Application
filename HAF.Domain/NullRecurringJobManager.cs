using System;
using System.Linq.Expressions;

namespace  HAF.Domain
{
    public class NullRecurringJobManager : IRecurringJobManager
    {
        public void AddOrUpdate(string recurringJobId, Expression<Action> job, string cronExpression)
        {
        }

        public void AddOrUpdate<T>(string recurringJobId, Expression<Action<T>> job, string cronExpression)
        {
        }

        public void RemoveIfExists(string recurringJobId)
        {
        }

        public void Trigger(string recurringJobId)
        {
        }
    }
}