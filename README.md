# AdaskoTheBeAsT.Dapper.NodaTime

Noda Time support for Dapper - unofficial fork of Dapper-NodaTime project which development stopped some time ago.
Matt Johnson-Pint is original author and maintainer of Dapper-NodaTime project.
This is continuation of his work.
Refreshed for currently available .NET versions.

## Badges

[![CodeFactor](https://www.codefactor.io/repository/github/adaskothebeast/adaskothebeast.dapper.nodatime/badge)](https://www.codefactor.io/repository/github/adaskothebeast/adaskothebeast.dapper.nodatime)
[![Build Status](https://adaskothebeast.visualstudio.com/AdaskoTheBeAsT.Dapper.NodaTime/_apis/build/status/AdaskoTheBeAsT.AdaskoTheBeAsT.Dapper.NodaTime?branchName=master)](https://adaskothebeast.visualstudio.com/AdaskoTheBeAsT.Dapper.NodaTime/_build/latest?definitionId=21&branchName=master)
![Azure DevOps tests](https://img.shields.io/azure-devops/tests/AdaskoTheBeAsT/AdaskoTheBeAsT.Dapper.NodaTime/21)
![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/AdaskoTheBeAsT/AdaskoTheBeAsT.Dapper.NodaTime/21?style=plastic)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=AdaskoTheBeAsT_AdaskoTheBeAsT.Dapper.NodaTime&metric=alert_status)](https://sonarcloud.io/dashboard?id=AdaskoTheBeAsT_AdaskoTheBeAsT.Dapper.NodaTime)
![Sonar Tests](https://img.shields.io/sonar/tests/AdaskoTheBeAsT_AdaskoTheBeAsT.Dapper.NodaTime?server=https%3A%2F%2Fsonarcloud.io)
![Sonar Test Count](https://img.shields.io/sonar/total_tests/AdaskoTheBeAsT_AdaskoTheBeAsT.Dapper.NodaTime?server=https%3A%2F%2Fsonarcloud.io)
![Sonar Test Execution Time](https://img.shields.io/sonar/test_execution_time/AdaskoTheBeAsT_AdaskoTheBeAsT.Dapper.NodaTime?server=https%3A%2F%2Fsonarcloud.io)
![Sonar Coverage](https://img.shields.io/sonar/coverage/AdaskoTheBeAsT_AdaskoTheBeAsT.Dapper.NodaTime?server=https%3A%2F%2Fsonarcloud.io&style=plastic)
![Nuget](https://img.shields.io/nuget/dt/AdaskoTheBeAsT.Dapper.NodaTime)
[![NuGet Version](https://img.shields.io/nuget/v/AdaskoTheBeAsT.Dapper.NodaTime.svg?style=flat)](https://www.nuget.org/packages/AdaskoTheBeAsT.Dapper.NodaTime/)

## Installation

```powershell
Install-Package AdaskoTheBeAsT.Dapper.NodaTime
```

In your project startup sequence somewhere, call:

```csharp
DapperNodaTimeSetup.Register(<provider>);
```

Provider is one of the following date time zone providers:

1. DateTimeZoneProviders.Tzdb - IANA Time Zone Database
2. DateTimeZoneProviders.Bcl - Windows registry

That registers all of the type handlers.  Alternatively, you can register each type handler separately if you wish.
For example:

```csharp
SqlMapper.AddTypeHandler(LocalDateTimeHandler.Default);
```

Work in progress.  Currently supports the following types:

- CalendarSystem - can be mapped to varchar(50)
- DateTimeZone - can be mapped to varchar(50)
- Duration - can be mapped to bigint
- Instant - can be mapped to datetime, datetime2, datetimeoffset
- LocalDate - can be mapped to date, datetime, datetime2
- LocalDateTime - can be mapped to datetime, datetime2
- LocalTime - can be mapped to time, datetime, datetime2
- Offset - can be mapped to int
- OffsetDateTime - can be mapped to datetimeoffset

Does not yet support:

- Period (working on it)
- ZonedDateTime (although there is trick to keep properties separated and then by using mapping code or AutoMapper (or other mapper) 
compose ZoneDateTime from LocalDateTime, CalendarSystem and DateTimeZone)
