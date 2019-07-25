using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEnumerable<T> Get<T>(IGetCommand<T> command);
        IEnumerable<T> GetAll<T>(IGetAllCommand<T> command);
        T Add<T>(IAddCommand<T> command);
        T Update<T>(IUpdateCommand<T> command);
        T Delete<T>(IDeleteCommand<T> command);
        Task<IEnumerable<T>> GetAsync<T>(IGetCommandAsync<T> command, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync<T>(IGetAllCommandAsync<T> command, CancellationToken cancellationToken = default);
        Task<T> AddAsync<T>(IAddCommandAsync<T> command, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync<T>(IUpdateCommandAsync<T> command, CancellationToken cancellationToken = default);
        Task<T> DeleteAsync<T>(IDeleteCommandAsync<T> command, CancellationToken cancellationToken = default);
        void Commit();
        void Rollback();
    }
}
