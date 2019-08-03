﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper.UoW.ConsoleUI.Data.Commands;

namespace Dapper.UoW.ConsoleUI
{
    /// <summary>
    /// here are some mockups for the methods i've implemented in my repository
    /// they're meant to be customizable, which means, you can create your own commands
    /// according to your requirements and change them as you wish to make them fit in your application context
    /// </summary>
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

            //Console.WriteLine($"\nExecuting a delete command...\n");
            //RemovePerson(6);

            // async methods calls

            //Console.WriteLine($"Fetching one result from database:");
            //Task.Run(async () => await PrintPersonAsync(2));

            //Console.WriteLine($"Listing all the results from database:");
            //Task.Run(async () => await PrintPeopleAsync());

            //Console.WriteLine($"\nExecuting a delete command...\n");
            //Task.Run(async () => await RemovePersonAsync(6));

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
                    var person = uow.Get(new GetPersonByIdCommand<int>(Id)).FirstOrDefault();
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
        /// remove an entry from the database
        /// </summary>
        /// <param name="Id"></param>
        private static void RemovePerson(int Id)
        {
            try
            {
                // initialize the connection builder
                var factory = new UnitOfWorkFactory(ConnectionString);
                // initialize the repository with transactions set to true
                using (var uow = factory.Create(true))
                {
                    // executes the command
                    var result = uow.Delete(new DeletePersonByIdCommand(6));
                    // prints the affected rows
                    Console.WriteLine($"Deleted: {result}");
                    // commits the changes
                    uow.Commit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// asynchronously remove an entry from the database
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private async static Task RemovePersonAsync(int Id)
        {
            try
            {
                // initialize the connection builder
                var factory = new UnitOfWorkFactory(ConnectionString);
                // initialize the repository with transactions set to true
                using (var uow = await factory.CreateAsync(true))
                {
                    // executes the command
                    var result = await uow.DeleteAsync(new DeletePersonByIdCommand(6));
                    // prints the affected rows
                    Console.WriteLine($"Deleted: {result}");
                    // commits the changes
                    uow.Commit();
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