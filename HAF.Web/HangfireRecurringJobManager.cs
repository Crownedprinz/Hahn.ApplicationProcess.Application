using System;
using System.Linq.Expressions;
using Hangfire;
using IRecurringJobManager = HAF.Domain.IRecurringJobManager;

namespace HAF.Web
{
    public class HangfireRecurringJobManager : IRecurringJobManager
    {
        public void AddOrUpdate(string recurringJobId, Expression<Action> job, string cronExpression)
        {
            RecurringJob.AddOrUpdate(recurringJobId, job, cronExpression);
        }

        public void AddOrUpdate<T>(string recurringJobId, Expression<Action<T>> job, string cronExpression)
        {
            RecurringJob.AddOrUpdate(recurringJobId, job, cronExpression);
        }

        public void RemoveIfExists(string recurringJobId)
        {
            RecurringJob.RemoveIfExists(recurringJobId);
        }

        public void Trigger(string recurringJobId)
        {
            RecurringJob.Trigger(recurringJobId);
        }
    }
}