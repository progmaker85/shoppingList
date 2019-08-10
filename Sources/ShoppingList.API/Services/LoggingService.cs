using System;
using Microsoft.Extensions.Logging;

namespace ShoppingList.API.Services
{
    public static class LoggingService
    {
        private static ILogger<Startup> _logger;
        public static void InitLogger(ILogger<Startup> logger)
        {
            _logger = logger;
        }

        public static void ToLogInfo(this object logObject)
        {
            _logger.LogInformation(new EventId(1), logObject.ToString());
        }
        public static void ToLogError(this Exception logObject)
        {
            _logger.LogError(new EventId(1), logObject, null, null);
        }
    }
}
