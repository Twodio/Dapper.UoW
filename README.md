# Dapper.UoW - Dapper Unit Of Work
This dapper unit of work repository implementation, originally implemented using petrhaus's UOW repository, but after many tests i couldn't make it work as i expected. Still, thanks to generosity for sharing his repository, i could enlight myself and find a good structure for my UoW.

Be sure to change your VS language version to C# >= ![C# Version](https://img.shields.io/badge/version-7.x-green.svg) and your .NET Core to >= ![.NET Core Version](https://img.shields.io/badge/version-2.x-green.svg)

It does support both sync and async with trasactions.

## Caveats

IEnumerable<T> Get() and Task<IEnumerable<T>> GetAsync() were suposed to return a single object instead or a list. Both are meant to match and retrieve a single instance in the database. 
  
Since i coulnd't unfold the Task returned in the Async method, i could only return the returned result Dapper's QueryAsync was delivering.

## Changes

petrhaus's repository had a very simple logic that would work in all the possible cases i could imagine for my next project and was well organized as i expected it to be, the only problem was the lazy loading and his transient exception that i didn't need - at first.

#### first stage

After checking the repository and my goal, i decided that i would implement [exponential backoff](https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/explore-custom-http-call-retries-exponential-backoff) from microsoft's page, as they are the same. Got some backlashes but i managed to work it out.

#### second stage

I tried to implement lazy loading and keep the simple queries, but the structure wouldn't allow, and the repository didn't have a generic CRUD, so, only two kind of queries could be executed. I was able to do a partial implementation but there was no consistency, since my async method were being consumed twice and were genereating too much code and the command weren't fully compatible.

#### third stage (actual)

I kept the base repository structure, removed all the methods except those required by the repository itself. 

- Added new interfaces and removed the olds for IUnitOfWork and ICommads.
- Added two new objects (PersonEntity and AddressEntity) and removed the olds.
- Removed the old tests in the Main method.
- Added generic crud to the UoW's interface and Command's as well.

#### fourth stage (implement my last changes)

...

## Percentage to turn the repository visible : 80%

## Note: It's still under development, and there are changes i couldn't implement (have them ready but need to translate to C# because they're in VB.NET).
