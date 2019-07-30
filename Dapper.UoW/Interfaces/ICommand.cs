using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.UoW.Interfaces
{
    public interface IAddCommand<out T>
    {
        bool RequiresTransaction { get; }
        T Execute(IDbConnection connection, IDbTransaction transaction);
    }

    public interface IUpdateCommand<out T>
    {
        bool RequiresTransaction { get; }
        T Execute(IDbConnection connection, IDbTransaction transaction);
    }

    public interface IDeleteCommand<out T>
    {
        bool RequiresTransaction { get; }
        T Execute(IDbConnection connection, IDbTransaction transaction);
    }

    public interface IGetCommand<out T, TId>
    {
        bool RequiresTransaction { get; }
        IEnumerable<T> Execute(IDbConnection connection, IDbTransaction transaction);
    }

    public interface IGetCommand<out T>
    {
        bool RequiresTransaction { get; }
        IEnumerable<T> Execute(IDbConnection connection, IDbTransaction transaction);
    }

    public interface IAddCommandAsync<T>
    {
        bool RequiresTransaction { get; }
        Task<T> Execute(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default);
    }

    public interface IUpdateCommandAsync<T>
    {
        bool RequiresTransaction { get; }
        Task<T> Execute(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default);
    }

    public interface IDeleteCommandAsync<T>
    {
        bool RequiresTransaction { get; }
        Task<T> Execute(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default);
    }

    public interface IGetCommandAsync<T, TId>
    {
        bool RequiresTransaction { get; }
        Task<IEnumerable<T>> Execute(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default);
    }

    public interface IGetCommandAsync<T>
    {
        bool RequiresTransaction { get; }
        Task<IEnumerable<T>> Execute(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default);
    }
}
