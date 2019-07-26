using Dapper.UnitOfWork.Example.Data.Entities;
using Dapper.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.UnitOfWork.Example.Data.Commands
{
    public class AddPersonCommand : IAddCommand<int>, IAddCommandAsync<int>
    {
        private const string _sql = @"
			INSERT INTO People(
				Name,
				Age,
                Address_Id)
			VALUES(
				@Name,
				@Age,
                @Address_Id)
		";

        private readonly PersonEntity _entity;

        // Set this to true prevents invoking the command without an explicit transaction
        public bool RequiresTransaction => true;

        public AddPersonCommand(PersonEntity entity)
            => _entity = entity;

        public int Execute(IDbConnection connection, IDbTransaction transaction)
            => _entity.Id = connection.ExecuteScalar<int>(_sql, _entity, transaction);

        public Task<int> Execute(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default)
            => connection.ExecuteScalarAsync<int>(new CommandDefinition(commandText: _sql, parameters: _entity, transaction: transaction, cancellationToken: cancellationToken));
    }
}
