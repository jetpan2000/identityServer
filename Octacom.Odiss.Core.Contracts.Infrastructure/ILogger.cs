using System;

namespace Octacom.Odiss.Core.Contracts.Infrastructure
{
    public interface ILogger
    {
        void LogActivity(string activityType, object data);
        void LogSystemActivity(string activityType, object data);
        void LogException(Exception exception, ExceptionSeverity severity);
    }
}
