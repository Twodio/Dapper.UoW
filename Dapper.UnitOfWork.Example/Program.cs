using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper.UnitOfWork.Example.Data.Commands;

namespace Dapper.UnitOfWork.Example
{
	class Program
    {
        private const string ConnectionString = @"your_connection_string";

		static void Main(string[] args)
		{
			var factory = new UnitOfWorkFactory(ConnectionString);

            // tansactional is enabled, but won't be necessary because we're running non saving queries
            using (var uow = factory.Create(true))
            {
                try
                {
                    // find a person by its Id
                    var person = uow.Get(new GetPersobByIdCommand<int>(2)).FirstOrDefault();
                    Console.WriteLine($"Person: {person?.Name} ({person?.Address?.Street})");

                    // fetch all the people in the database
                    //var people = uow.Get(new GetPeopleCommand());
                    //foreach (var p in people)
                    //{
                    //    Console.WriteLine($"Person: {p?.Name} ({p?.Address?.Street})");
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            //Task.Run(async () => await MainAsync(args));

            Console.WriteLine("Press any key to exit");
			Console.ReadKey();
		}

        private static async Task MainAsync(string[] args)
        {
            var factory = new UnitOfWorkFactory(ConnectionString);

            // tansactional is enabled, but won't be necessary because we're running non saving queries
            using (var uow = await factory.CreateAsync(true))
            {
                //AddressEntity address = new AddressEntity { Street = "Somewhere", Region = "Random Region" };
                //address.Id = await uow.ExecuteAsync<int>(new AddAddressCommand(ref address));
                //await uow.ExecuteAsync(new AddPersonCommand(new PersonEntity { Name = "Jon Doe", Age = 28, Address_id = address.Id }));
                //uow.Commit();
                //var person = await uow.QueryAsync<PersonEntity>(new GetPersonByIdQuery(2));
                //System.Diagnostics.Debug.WriteLine($"Person: {person.Name}");

                // working test

                // find a person by its Id
                var person = (await uow.GetAsync(new GetPersobByIdCommand<int>(29))).FirstOrDefault();
                Console.WriteLine($"Person: {person?.Name} ({person?.Address?.Street})");

                // fetch all the people in the database
                var people = await uow.GetAsync(new GetPeopleCommand());
                foreach(var p in people)
                {
                    Console.WriteLine($"Person: {p?.Name} ({p?.Address?.Street})");
                }

                // find one person by its address street's name
                var personOne = (await uow.GetAsync(new GetPersonByStreetCommand<string>("Elm Street"))).FirstOrDefault();
                Console.WriteLine($"Person: {personOne?.Name} ({personOne?.Address?.Street})");
            }
        }
    }
}