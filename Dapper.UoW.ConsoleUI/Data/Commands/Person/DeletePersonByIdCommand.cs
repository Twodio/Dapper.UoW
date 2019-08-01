using Dapper.UoW.Interfaces;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.UoW.ConsoleUI.Data.Commands
{
    public class DeletePersonByIdCommand : IDeleteCommand<int>, IDeleteCommandAsync<int>
    {
        // the sql statement to be executed
        // instead you might want to configure a constraint and delete in cascade
        // the Id is already provided so, i don't think it's worth to retrieve after the delete
        private const string _sql = @"
            DECLARE @Ident TABLE(n INT);
			    INSERT INTO @Ident(n)
                    SELECT Address_Id FROM People
                        WHERE Id = @Id;
            DELETE FROM People WHERE Id = @Id;
            DELETE FROM Addresses WHERE Id = (SELECT TOP 1 n FROM @Ident);
		";

        // property to store the current entity Id
        public int _personId;

        /// <summary>
        /// prevents invoking the command without an explicit transaction
        /// </summary>
        public bool RequiresTransaction => true;

        public DeletePersonByIdCommand(int personId)
            => _personId = personId;

        public int Execute(IDbConnection connection, IDbTransaction transaction)
            => connection.Execute(_sql, new { Id = _personId }, transaction);

        public Task<int> Execute(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default)
            => connection.ExecuteAsync(new CommandDefinition(_sql, new { Id = _personId }, transaction, cancellationToken:cancellationToken));
    }
}
