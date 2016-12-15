# Serilog.Sinks.Logzio
[![Build status](https://ci.appveyor.com/api/projects/status/083a7a9q9eb9l7sr?svg=true)](https://ci.appveyor.com/project/AndersAsperheim/serilog-sinks-logzio) [![NuGet Version](http://img.shields.io/nuget/v/Serilog.Sinks.Logzio.svg?style=flat)](https://www.nuget.org/packages/Serilog.Sinks.Logzio/)

A [Serilog](http://serilog.net/) sink that sends HTTP POST requests over the network.

**Package** - [Serilog.Sinks.Logzio](https://www.nuget.org/packages/serilog.sinks.logzio) | **Platforms** - .NET 4.5, .NETStandard 1.5

### Getting started

TBA

```csharp
Serilog.ILogger log = new LoggerConfiguration()
  .MinimumLevel.Verbose()
  .WriteTo.Http("www.mylogs.com")
  .CreateLogger();
```


### Typical use case

TBA


### Install via NuGet

If you want to include the HTTP POST sink in your project, you can [install it directly from NuGet](https://www.nuget.org/packages/Serilog.Sinks.Logzio).

To install the sink, run the following command in the Package Manager Console:

```
PM> Install-Package Serilog.Sinks.Logzio
```
