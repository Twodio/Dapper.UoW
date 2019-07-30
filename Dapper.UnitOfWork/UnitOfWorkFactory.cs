using Dapper.UnitOfWork.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.UnitOfWork
{
	public interface IUnitOfWorkFactory
	{
		IUnitOfWork Create(bool transactional = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, Retry retry = null);
        Task<IUnitOfWork> CreateAsync(bool transactional = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, Retry retry = null, CancellationToken cancellationToken = default);
    }

	public class UnitOfWorkFactory : IUnitOfWorkFactory
	{
		private readonly string _connectionString;

		public UnitOfWorkFactory(string connectionString)
			=> _connectionString = connectionString;

		public IUnitOfWork Create(bool transactional = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, Retry retry = default)
		{
			var conn = new SqlConnection(_connectionString);
            if (retry != null)
            {
                retry.Do(()=> conn.Open());
            } else
            {
                conn.Open();
            }
            return new UnitOfWork(conn, transactional, isolationLevel, retry);
		}

        public async Task<IUnitOfWork> CreateAsync(bool transactional = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, Retry retry = null, CancellationToken cancellationToken = default)
        {
            var conn = new SqlConnection(_connectionString);
            if (retry != null)
            {
                await retry.DoAsync(() => conn.OpenAsync(cancellationToken));
            }
            else
            {
                await conn.OpenAsync(cancellationToken);
            }
            return new UnitOfWork(conn, transactional, isolationLevel, retry);
        }
	}
}
