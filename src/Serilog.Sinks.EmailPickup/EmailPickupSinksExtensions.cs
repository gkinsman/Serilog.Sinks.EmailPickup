using System;
using Serilog.Configuration;
using Serilog.Events;

namespace Serilog.Sinks.EmailPickup
{
    public static class EmailPickupSinksExtensions
    {
        public static LoggerConfiguration EmailPickup(this LoggerSinkConfiguration loggerConfiguration,
            string pickupDirectory,
            string toEmail,
            string fromEmail,
            string subject,
            string fileExtension = "",
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null,
            TimeSpan? period = null,
            int batchPostingLimit = EmailPickupSink.DefaultBatchPostingLimit)
        {
            return loggerConfiguration.Sink(new EmailPickupSink(pickupDirectory, toEmail, fromEmail, subject,
                    fileExtension, formatProvider, period ?? EmailPickupSink.DefaultPeriod, batchPostingLimit),
                restrictedToMinimumLevel);
        }
    }
}