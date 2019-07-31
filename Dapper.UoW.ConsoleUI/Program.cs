using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper.UoW.ConsoleUI.Data.Commands;

namespace Dapper.UoW.ConsoleUI
{
	class Program
    {
        // the connection string for the database you'll be using
        private const string ConnectionString = @"your_connection_string";

		static void Main(string[] args)
		{
            Console.WriteLine($"Fetching one result from database...\n");
            PrintPerson(2);

            Console.WriteLine($"\nListing all the results from database...\n");
            PrintPeople();

            // async methods calls

            //Console.WriteLine($"Fetching one result from database:");
            //Task.Run(async () => await PrintPersonAsync(2));

            //Console.WriteLine($"Listing all the results from database:");
            //Task.Run(async () => await PrintPeopleAsync());

            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
        }

        /// <summary>
        /// retrieve one match from Peoples table in the database using its Id
        /// </summary>
        /// <param name="Id"></param>
        private static void PrintPerson(int Id)
        {
            try
            {
                // initialize the connection builder
                var factory = new UnitOfWorkFactory(ConnectionString);
                // initialize the repository with transaction explicitly set to false
                using (var uow = factory.Create())
                {
                    var person = uow.Get(new GetPersobByIdCommand<int>(Id)).FirstOrDefault();
                    Console.WriteLine($"Person: {person?.Name} ({person?.Address?.Street})");
                }
            }
            catch (Exception ex)
            {
                // print the error in console
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// retrive all the entries from Peoples table in the database
        /// </summary>
        private static void PrintPeople()
        {
            try
            {
                var factory = new UnitOfWorkFactory(ConnectionString);
                using (var uow = factory.Create())
                {
                    // fetch all the people in the database
                    var people = uow.Get(new GetPeopleCommand());
                    foreach (var p in people)
                    {
                        Console.WriteLine($"Person: {p?.Name} ({p?.Address?.Street})");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// asynchronously retrieve one match from Peoples table in the database using its Id
        /// </summary>
        /// <param name="Id"></param>
        private async static Task PrintPersonAsync(int Id)
        {
            try
            {
                var factory = new UnitOfWorkFactory(ConnectionString);
                using (var uow = await factory.CreateAsync(true))
                {
                    // find one person by its address street's name
                    var personOne = (await uow.GetAsync(new GetPersonByStreetCommand<int>(Id))).FirstOrDefault();
                    Console.WriteLine($"Person: {personOne?.Name} ({personOne?.Address?.Street})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// asynchronously retrive all the entries from Peoples table in the database
        /// </summary>
        private async static Task PrintPeopleAsync()
        {
            try
            {
                var factory = new UnitOfWorkFactory(ConnectionString);
                using (var uow = await factory.CreateAsync(true))
                {
                    // fetch all the people in the database
                    var people = await uow.GetAsync(new GetPeopleCommand());
                    foreach (var p in people)

                    {
                        Console.WriteLine($"Person: {p?.Name} ({p?.Address?.Street})");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}