﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper.UnitOfWork.Example.Data.Commands;

namespace Dapper.UnitOfWork.Example
{
	class Program
    {
        private const string ConnectionString = @"";

		static void Main(string[] args)
		{
			var factory = new UnitOfWorkFactory(ConnectionString);

            using (var uow = factory.Create(true))
            {
                try
                {
                    //AddressEntity address = new AddressEntity { Street = "My Street", Region = "my Region" };
                    //uow.Execute<int>(new AddAddressCommand(ref address));
                    //uow.Execute(new AddPersonCommand(new PersonEntity { Name = "No One", Age = 28, Address_id = address.Id }));
                    //uow.Commit();

                    // working test
                    var person = uow.Get(new GetPersobByIdCommand(2)).FirstOrDefault();
                    System.Diagnostics.Debug.WriteLine($"Person: {person?.Name} ({person?.Address?.Street})");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }

            //Task.Run(async () => await MainAsync(args));

            Console.WriteLine("Press any key to exit");
			Console.ReadKey();
		}

        private static async Task MainAsync(string[] args)
        {
            var factory = new UnitOfWorkFactory(ConnectionString);

            using (var uow = await factory.CreateAsync(true, retryOptions: new RetryOptions(5, 100, new SqlTransientExceptionDetector())))
            {
                //AddressEntity address = new AddressEntity { Street = "Somewhere", Region = "Random Region" };
                //address.Id = await uow.ExecuteAsync<int>(new AddAddressCommand(ref address));
                //await uow.ExecuteAsync(new AddPersonCommand(new PersonEntity { Name = "Jon Doe", Age = 28, Address_id = address.Id }));
                //uow.Commit();
                //var person = await uow.QueryAsync<PersonEntity>(new GetPersonByIdQuery(2));
                //System.Diagnostics.Debug.WriteLine($"Person: {person.Name}");

                // working test
                var people = await uow.GetAsync(new GetPersobByIdCommand(2));
                var person = people.FirstOrDefault();
                System.Diagnostics.Debug.WriteLine($"Person: {person?.Name} ({person?.Address?.Street})");
            }
        }
    }
}