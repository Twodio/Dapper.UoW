using Dapper.UoW.ConsoleUI.Data.Entities;
using Dapper.UoW.Interfaces;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.UoW.ConsoleUI.Data.Commands
{
    public class AddAddressCommand : IAddCommand<int>, IAddCommandAsync<int>
    {
        // the sql statement to be executed
        // sane could be achieved with select cast(scope_identity() as int)
        private const string _sql = @"
            DECLARE @Ident TABLE(n INT);
                INSERT INTO Addresses(Street, Region)
                    OUTPUT INSERTED.Id INTO @Ident(n)
                VALUES(@Street, @Region);
            SELECT CAST(n AS int) FROM @Ident;";

        // property to store the current entity
        private AddressEntity _entity;

        /// <summary>
        /// prevents invoking the command without an explicit transaction
        /// </summary>
        public bool RequiresTransaction => true;

        /// <summary>
        /// constructor where we pass the object we want to insert
        /// </summary>
        /// <param name="entity"></param>
        public AddAddressCommand(ref AddressEntity entity)
            => _entity = entity;

        /// <summary>
        /// executes a custom statement with dapper which returns the inserted Address's Id
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int Execute(IDbConnection connection, IDbTransaction transaction)
            => _entity.Id = connection.ExecuteScalar<int>(_sql, _entity, transaction);

        /// <summary>
        /// asynchrounously executes a custom statement with dapper which returns the inserted Addresses's Id
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> Execute(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default)
            => connection.ExecuteScalarAsync<int>(new CommandDefinition(commandText: _sql, parameters: _entity, transaction: transaction, cancellationToken: cancellationToken));
    }
}
