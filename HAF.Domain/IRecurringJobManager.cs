using System;
using System.Linq.Expressions;

namespace  HAF.Domain
{
    public interface IRecurringJobManager
    {
        void AddOrUpdate(string recurringJobId, Expression<Action> job, string cronExpression);
        void AddOrUpdate<T>(string recurringJobId, Expression<Action<T>> job, string cronExpression);
        void RemoveIfExists(string recurringJobId);
        void Trigger(string recurringJobId);
    }
}