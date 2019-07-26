# Dapper.UoW - Dapper Unit Of Work
This dapper unit of work repository implementation, originally implemented using petrhaus's UOW repository, but after many tests i couldn't make it work as i expected. Still, thanks for your generosity for sharing his repository, i could enlight myself and find a good structure for my UoW.

Be sure to change your VS language version to C# >= ![C# Version](https://img.shields.io/badge/version-7.x-green.svg) and your .NET Core to >= ![.NET Core Version](https://img.shields.io/badge/version-2.x-green.svg)

## Let's start

1. Download/Clone the repository;
2. Check your .NET Core Framework version and the project's language version;
3. Generate a dabase using the database.sql, copy and past the connectionString into the Program.cs;
4. Build the solution and run your tests.

It does support both sync and async with trasactions.

## Caveats

IEnumerable<T> Get() and Task<IEnumerable<T>> GetAsync() were suposed to return a single object instead or a list. Both are meant to match and retrieve a single instance in the database. 
  
Since i coulnd't unfold the Task returned in the Async method, i could only return the returned result Dapper's QueryAsync was delivering.

## Changes

See all changes history with details [here](https://github.com/Twodio/Dapper.UoW/wiki/Changelog).

## Note: It's still under development, and there are changes i couldn't implement (have them ready but need to translate to C# because they're in VB.NET).
