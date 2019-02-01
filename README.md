# Serilog.Sinks.EmailPickup

A sink that sends log events to the file system in an email format that can be picked up by, for example, IIS. Does the same thing as the log4net SmtpPickupDirAppender.

**Package** - [Serilog.Sinks.EmailPickup](http://nuget.org/packages/serilog.sinks.emailpickup)
| **Platforms** - .NET 4.5, .Net Standard 2.0

```csharp
var log = new LoggerConfiguration()
    .WriteTo.EmailPickup(
        fromEmail: "app@example.com",
        toEmail: "support@example.com",
        pickupDirectory: "c:\logs\emailpickup",
        subject: "UH OH",
        fileExtension: ".email",
        restrictedToMinimumLevel: LogEventLevel.Error)
    .CreateLogger();
```