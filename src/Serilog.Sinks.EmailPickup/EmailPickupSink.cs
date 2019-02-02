using System;
using System.Collections.Generic;
using System.IO;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.EmailPickup
{
    public class EmailPickupSink : PeriodicBatchingSink
    {
        private readonly IFormatProvider _formatProvider;
        private readonly string _pickupDirectory;
        private readonly string _toEmail;
        private readonly string _subject;
        private readonly string _fileExtension;
        private readonly string _fromEmail;
        private bool _directoryExists;
        
        /// <summary>
        /// A reasonable default for the number of events posted in
        /// each batch.
        /// </summary>
        public const int DefaultBatchPostingLimit = 100;

        /// <summary>
        /// A reasonable default time to wait between checking for event batches.
        /// </summary>
        public static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(30);

        

        public EmailPickupSink(string pickupDirectory, string toEmail, string fromEmail, string subject,
            string fileExtension, IFormatProvider formatProvider, TimeSpan defaultPeriod, int defaultBatchPostingLimit) : base(defaultBatchPostingLimit, defaultPeriod)
        {
            _formatProvider = formatProvider;
            _pickupDirectory = pickupDirectory ?? throw new ArgumentNullException(nameof(pickupDirectory));
            _toEmail = toEmail ?? throw new ArgumentNullException(nameof(toEmail));
            _fromEmail = fromEmail ?? throw new ArgumentNullException(nameof(fromEmail));
            _subject = subject ?? throw new ArgumentNullException(nameof(subject));
            _fileExtension = fileExtension ?? throw new ArgumentNullException(nameof(fileExtension));
        }

        protected override void EmitBatch(IEnumerable<LogEvent> logEvents)
        {
            if (logEvents == null) throw new ArgumentNullException(nameof(logEvents));

            EnsurePickupDirExists();

            var filePath = "";
            try
            {
                filePath = Path.Combine(_pickupDirectory, Guid.NewGuid().ToString("N") + _fileExtension);
                using (var writer = File.CreateText(filePath))
                {
                    writer.WriteLine($"To: {_toEmail}");
                    writer.WriteLine($"From: {_fromEmail}");
                    writer.WriteLine($"Subject: {_subject}");
                    writer.WriteLine($"Date: {DateTime.UtcNow:R}");
                    writer.WriteLine();

                    foreach (var e in logEvents)
                    {
                        writer.WriteLine("-----------");
                        writer.WriteLine();
                        writer.WriteLine($"Level: {e.Level}");
                        e.RenderMessage(writer, _formatProvider);
                        writer.WriteLine();
                        WriterProperties(writer, e.Properties);
                        writer.WriteLine();
                        WriteException(writer, e.Exception);
                        writer.WriteLine();
                    }
                }
            }
            catch (Exception e)
            {
                SelfLog.WriteLine($"Failure when writing writing event to {filePath}: {e.Message}");
            }
        }

        private void EnsurePickupDirExists()
        {
            if (_directoryExists) return;
            Directory.CreateDirectory(_pickupDirectory);
            
            _directoryExists = true;
        }

        private void WriterProperties(TextWriter writer, IReadOnlyDictionary<string, LogEventPropertyValue> properties)
        {
            writer.WriteLine();
            writer.WriteLine("Properties:");
            foreach (var prop in properties.Keys)
            {
                writer.Write($"{prop}: ");
                properties[prop].Render(writer, null, _formatProvider);
                writer.WriteLine();
            }
        }

        private void WriteException(TextWriter writer, Exception e)
        {
            if (e == null)
            {
                writer.WriteLine("No exception was logged");
                return;
            }

            var exText = e.ToString().Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in exText)
            {
                writer.WriteLine(line);
            }
        }
    }
}