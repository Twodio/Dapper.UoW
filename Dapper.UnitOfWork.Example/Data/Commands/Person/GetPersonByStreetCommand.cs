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
    public class GetPersonByStreetCommand<TId> : IGetCommand<PersonEntity, TId>, IGetCommandAsync<PersonEntity, TId>
    {
        private const string _sql = @"
			SELECT
				p.*,a.*
			FROM
				People p
                    JOIN Addresses a ON
                        p.Address_Id = a.Id
			WHERE
				a.Street = @Street
		";

        public bool RequiresTransaction => true;

        private dynamic _streetName;

        public GetPersonByStreetCommand(TId streetName)
            => _streetName = streetName;

        public IEnumerable<PersonEntity> Execute(IDbConnection connection, IDbTransaction transaction)
            => connection.Query<PersonEntity, AddressEntity, PersonEntity>(_sql, (person, address) =>
            {
                person.Address = address;
                return person;
            },
                    new { Street = _streetName }, transaction, splitOn: "Id");

        public Task<IEnumerable<PersonEntity>> Execute(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default)
            => connection.QueryAsync<PersonEntity, AddressEntity, PersonEntity>(new CommandDefinition(_sql,
                new { Street = _streetName }, transaction: transaction, cancellationToken: cancellationToken),
                (person, address) =>
                {
                    person.Address = address;
                    return person;
                }, splitOn: "Id");
    }
}
