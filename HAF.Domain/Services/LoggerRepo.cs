using HAF.Domain;
using System.Collections.Generic;
namespace HAF.Domain.Services
{

    public interface ILoggerRepo
    {
        void AddToLogs(Log log);

        List<Log> GetAllLogs();
    }

    public class LoggerRepo : ILoggerRepo
    {
        public void AddToLogs(Log log)
        {
            LoggerStore.Logs.Add(log);
        }

        public List<Log> GetAllLogs()
        {
            return LoggerStore.Logs;
        }
    }

    public class LoggerStore
    {
        public static List<Log> Logs = new List<Log>();
    }
}