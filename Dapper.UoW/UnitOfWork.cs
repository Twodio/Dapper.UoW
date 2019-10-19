using Dapper.UoW.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.UoW
{
	public class UnitOfWork : IUnitOfWork
	{
		private bool _disposed;
		private IDbConnection _connection;
		private IDbTransaction _transaction;
        private RetryOptions _options;

        internal UnitOfWork(IDbConnection connection, RetryOptions Options = default, bool transactional = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
		{
			_connection = connection;
            _options = Options;
			if (transactional)
            {
                _transaction = connection.BeginTransaction(isolationLevel);
            }
        }

        public IEnumerable<T> Get<T>(IGetCommand<T> command)
        {
            return Retry.Invoke(() => command.Execute(_connection, _transaction), _options);
        }

        public IEnumerable<T> Get<T, TId>(IGetCommand<T, TId> command)
        {
            return Retry.Invoke(()=> command.Execute(_connection, _transaction), _options);
        }

        public T Add<T>(IAddCommand<T> command)
        {
            if (command.RequiresTransaction && _transaction == null)
            {
                throw new Exception($"The command {command.GetType()} requires a transaction");
            }
            return Retry.Invoke(()=> command.Execute(_connection, _transaction), _options);
        }

        public T Update<T>(IUpdateCommand<T> command)
        {
            if (command.RequiresTransaction && _transaction == null)
            {
                throw new Exception($"The command {command.GetType()} requires a transaction");
            }
            return Retry.Invoke(()=> command.Execute(_connection, _transaction), _options);
        }

        public T Delete<T>(IDeleteCommand<T> command)
        {
            if (command.RequiresTransaction && _transaction == null)
            {
                throw new Exception($"The command {command.GetType()} requires a transaction");
            }
            return Retry.Invoke(()=> command.Execute(_connection, _transaction), _options);
        }

        public Task<IEnumerable<T>> GetAsync<T>(IGetCommandAsync<T> command, CancellationToken cancellationToken = default)
        {
            return Retry.InvokeAsync(()=> command.Execute(_connection, _transaction, cancellationToken), _options);
        }

        public Task<IEnumerable<T>> GetAsync<T, TId>(IGetCommandAsync<T, TId> command, CancellationToken cancellationToken = default)
        {
            return Retry.Invoke(()=> command.Execute(_connection, _transaction, cancellationToken), _options);
        }

        public Task<T> AddAsync<T>(IAddCommandAsync<T> command, CancellationToken cancellationToken = default)
        {
            if (command.RequiresTransaction && _transaction == null)
            {
                throw new Exception($"The command {command.GetType()} requires a transaction");
            }
            return Retry.Invoke(()=> command.Execute(_connection, _transaction, cancellationToken), _options);
        }

        public Task<T> UpdateAsync<T>(IUpdateCommandAsync<T> command, CancellationToken cancellationToken = default)
        {
            if (command.RequiresTransaction && _transaction == null)
            {
                throw new Exception($"The command {command.GetType()} requires a transaction");
            }
            return Retry.Invoke(()=> command.Execute(_connection, _transaction, cancellationToken), _options);
        }

        public Task<T> DeleteAsync<T>(IDeleteCommandAsync<T> command, CancellationToken cancellationToken = default)
        {
            if (command.RequiresTransaction && _transaction == null)
            {
                throw new Exception($"The command {command.GetType()} requires a transaction");
            }
            return Retry.Invoke(()=> command.Execute(_connection, _transaction, cancellationToken), _options);
        }

        public void Commit()
			=> _transaction?.Commit();

		public void Rollback()
			=> _transaction?.Rollback();

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~UnitOfWork()
			=> Dispose(false);

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				_transaction?.Dispose();
				_connection?.Dispose();
			}

            _transaction = null;
			_connection = null;

			_disposed = true;
		}
    }
}
