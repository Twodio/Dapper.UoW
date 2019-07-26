using Dapper.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private bool _disposed;
		private IDbConnection _connection;
		private readonly RetryOptions _retryOptions;
		private IDbTransaction _transaction;

        internal UnitOfWork(IDbConnection connection, bool transactional = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, RetryOptions retryOptions = null)
		{
			_connection = connection;
			_retryOptions = retryOptions;
			if (transactional)
            {
                _transaction = connection.BeginTransaction(isolationLevel);
            }
        }

        public IEnumerable<T> Get<T>(IGetCommand<T> command)
        {
            return Retry.Do(() => command.Execute(_connection, _transaction), _retryOptions);
        }

        public IEnumerable<T> Get<T, TId>(IGetCommand<T, TId> command)
        {
            return Retry.Do(() => command.Execute(_connection, _transaction), _retryOptions);
        }

        public T Add<T>(IAddCommand<T> command)
        {
            if (command.RequiresTransaction && _transaction == null)
            {
                throw new Exception($"The command {command.GetType()} requires a transaction");
            }
            return Retry.Do(() => command.Execute(_connection, _transaction), _retryOptions);
        }

        public T Update<T>(IUpdateCommand<T> command)
        {
            if (command.RequiresTransaction && _transaction == null)
            {
                throw new Exception($"The command {command.GetType()} requires a transaction");
            }
            return Retry.Do(() => command.Execute(_connection, _transaction), _retryOptions);
        }

        public T Delete<T>(IDeleteCommand<T> command)
        {
            if (command.RequiresTransaction && _transaction == null)
            {
                throw new Exception($"The command {command.GetType()} requires a transaction");
            }
            return Retry.Do(() => command.Execute(_connection, _transaction), _retryOptions);
        }

        public Task<IEnumerable<T>> GetAsync<T>(IGetCommandAsync<T> command, CancellationToken cancellationToken = default)
        {
            return Retry.DoAsync(() => command.Execute(_connection, _transaction, cancellationToken), _retryOptions);
        }

        public Task<IEnumerable<T>> GetAsync<T, TId>(IGetCommandAsync<T, TId> command, CancellationToken cancellationToken = default)
        {
            return Retry.DoAsync(() => command.Execute(_connection, _transaction, cancellationToken), _retryOptions);
        }

        public Task<T> AddAsync<T>(IAddCommandAsync<T> command, CancellationToken cancellationToken = default)
        {
            if (command.RequiresTransaction && _transaction == null)
            {
                throw new Exception($"The command {command.GetType()} requires a transaction");
            }
            return Retry.DoAsync(() => command.Execute(_connection, _transaction, cancellationToken), _retryOptions);
        }

        public Task<T> UpdateAsync<T>(IUpdateCommandAsync<T> command, CancellationToken cancellationToken = default)
        {
            if (command.RequiresTransaction && _transaction == null)
            {
                throw new Exception($"The command {command.GetType()} requires a transaction");
            }
            return Retry.DoAsync(() => command.Execute(_connection, _transaction, cancellationToken), _retryOptions);
        }

        public Task<T> DeleteAsync<T>(IDeleteCommandAsync<T> command, CancellationToken cancellationToken = default)
        {
            if (command.RequiresTransaction && _transaction == null)
            {
                throw new Exception($"The command {command.GetType()} requires a transaction");
            }
            return Retry.DoAsync(() => command.Execute(_connection, _transaction, cancellationToken), _retryOptions);
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
