﻿using Dapper.UoW.ConsoleUI.Data.Entities;
using Dapper.UoW.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.UoW.ConsoleUI.Data.Commands
{
    public class GetPersonByIdCommand<TId> : IGetCommand<PersonEntity, TId>, IGetCommandAsync<PersonEntity, TId>
    {
        private const string _sql = @"
			SELECT
				p.*,a.*
			FROM
				People p
                    JOIN Addresses a ON

                        p.Address_Id = a.Id
			WHERE
				p.Id = @Id
		";

        public bool RequiresTransaction => false;

        private dynamic _personId;

        public GetPersonByIdCommand(TId personId)
            => _personId = personId;

        public IEnumerable<PersonEntity> Execute(IDbConnection connection, IDbTransaction transaction)
            => connection.Query<PersonEntity, AddressEntity, PersonEntity>(_sql, (person, address) =>
            {
                person.Address = address;
                return person;
            },
                new { Id = _personId }, transaction, splitOn: "Id");

        public Task<IEnumerable<PersonEntity>> Execute(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default)
            => connection.QueryAsync<PersonEntity, AddressEntity, PersonEntity>(new CommandDefinition(_sql,
                new { Id = _personId }, transaction: transaction, cancellationToken:cancellationToken), 
                (person, address) =>
                {
                    person.Address = address;
                    return person;
                }, splitOn: "Id");
    }
}
