using Dapper.UnitOfWork.Example.Data.Entities;
using Dapper.UnitOfWork.Interfaces;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.UnitOfWork.Example.Data.Commands
{
    public class AddAddressCommand : IAddCommand<int>, IAddCommandAsync<int>
    {
        private const string _sql = @"
            DECLARE @Ident TABLE(n INT);
                INSERT INTO Addresses(Street, Region)
                    OUTPUT INSERTED.Id INTO @Ident(n)
                VALUES(@Street, @Region);
            SELECT CAST(n AS int) FROM @Ident;";

        private AddressEntity _entity;

        public bool RequiresTransaction => true;

        public AddAddressCommand(ref AddressEntity entity)
            => _entity = entity;

        public int Execute(IDbConnection connection, IDbTransaction transaction)
            => _entity.Id = connection.ExecuteScalar<int>(_sql, _entity, transaction);

        public Task<int> Execute(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default)
            => connection.ExecuteScalarAsync<int>(new CommandDefinition(commandText: _sql, parameters: _entity, transaction: transaction, cancellationToken: cancellationToken));
    }
}
