using Dapper.UoW.Interfaces;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.UoW.Interfaces
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create(bool transactional = false, RetryOptions options = default, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        Task<IUnitOfWork> CreateAsync(bool transactional = false, RetryOptions options = default, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);
    }
}
