using Dapper.UnitOfWork.Example.Data.Entities;
using Dapper.UnitOfWork.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.UnitOfWork.Example.Data.Commands
{
    public class GetPeopleCommand : IGetCommand<PersonEntity>, IGetCommandAsync<PersonEntity>
    {
        private const string _sql = @"
			SELECT
				p.*,a.*
			FROM
				People p
                    JOIN Addresses a ON
                        p.Address_Id = a.Id
		";

        public bool RequiresTransaction => false;

        public GetPeopleCommand() { }

        public IEnumerable<PersonEntity> Execute(IDbConnection connection, IDbTransaction transaction)
            => connection.Query<PersonEntity, AddressEntity, PersonEntity>(_sql, (person, address) =>
            {
                person.Address = address;
                return person;
            },
                new { }, transaction, splitOn: "Id");

        public Task<IEnumerable<PersonEntity>> Execute(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default)
            => connection.QueryAsync<PersonEntity, AddressEntity, PersonEntity>(new CommandDefinition(_sql,
                new { }, transaction: transaction, cancellationToken: cancellationToken),
                (person, address) =>
                {
                    person.Address = address;
                    return person;
                }, splitOn: "Id");
    }
}
