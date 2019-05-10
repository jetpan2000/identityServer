using Microsoft.Owin.Logging;
using System;
using System.Diagnostics;
using IOdissLogger = Octacom.Odiss.Core.Contracts.Infrastructure.ILogger;

namespace Octacom.Odiss.Core.Identity.Utilities
{
    public class OdissOwinLogger : ILogger, IDisposable
    {
        private readonly IOdissLogger odissLogger;

        public OdissOwinLogger(IOdissLogger odissLogger)
        {
            this.odissLogger = odissLogger;
        }

        public bool WriteCore(TraceEventType eventType, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            var formatted = formatter(state, exception);

            odissLogger.LogSystemActivity("OWIN", new
            {
                eventType, eventId, state, exception, formatted
            });

            return true;
        }

        public void Dispose()
        {
        }
    }
}
