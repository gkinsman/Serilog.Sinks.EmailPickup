# Serilog.Sinks.EmailPickup [![NuGet](https://img.shields.io/nuget/vpre/Serilog.Sinks.EmailPickup.svg)](https://www.nuget.org/packages/Serilog.Sinks.EmailPickup/) [![license](https://img.shields.io/github/license/gkinsman/Serilog.Sinks.EmailPickup.svg)](LICENSE)

A sink that sends log events to the file system in an email format that can be picked up by, for example, IIS. Does the same thing as the log4net SmtpPickupDirAppender.

**Package** - [Serilog.Sinks.EmailPickup](http://nuget.org/packages/serilog.sinks.emailpickup)
| **Platforms** - .NET 4.5, .Net Standard 1.6

```csharp
var log = new LoggerConfiguration()
    .WriteTo.EmailPickup(
        fromEmail: "app@example.com",
        toEmail: "support@example.com",
        pickupDirectory: @"c:\logs\emailpickup",
        subject: "UH OH",
        fileExtension: ".email",
        restrictedToMinimumLevel: LogEventLevel.Error)
    .CreateLogger();
```
