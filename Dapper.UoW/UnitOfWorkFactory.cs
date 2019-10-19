using Dapper.UoW.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.UoW
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
	{
		private readonly string _connectionString;

		public UnitOfWorkFactory(string connectionString)
			=> _connectionString = connectionString;

		public IUnitOfWork Create(bool transactional = false, RetryOptions options = default, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
		{
			var conn = new SqlConnection(_connectionString);
            conn.Open();
            return new UnitOfWork(conn, options, transactional, isolationLevel);
		}

        public async Task<IUnitOfWork> CreateAsync(bool transactional = false, RetryOptions options = default, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);
            return new UnitOfWork(conn, options, transactional, isolationLevel);
        }
	}
}
