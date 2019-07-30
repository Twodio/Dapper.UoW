using System;
using System.Threading.Tasks;

namespace Dapper.UnitOfWork
{
    public class Retry
    {
        public readonly RetryOptions retryOptions;

        public Retry(int maxRetries = 5, int delayMilliseconds = 200, int maxDelayMilliseconds = 200)
            => retryOptions = new RetryOptions(maxRetries, delayMilliseconds, maxDelayMilliseconds);

        public T Do<T>(Func<T> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            if (retryOptions == null)
                return func();

            var counter = 1;
            while(counter <= retryOptions._maxRetries)
            {
                try
                {
                    return func();
                } catch(Exception ex) when (ex is TimeoutException || ex is System.Data.SqlClient.SqlException)
                {
                    Task.Run(async () => await retryOptions.Delay(counter, ex));
                }
                counter++;
            }
            return default;
        }

        public async Task<T> DoAsync<T>(Func<Task<T>> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            if (retryOptions == null)
                return await func();

            var counter = 1;
            while (counter <= retryOptions._maxRetries)
            {
                try
                {
                    return await func();
                }
                catch (Exception ex) when (ex is TimeoutException || ex is System.Data.SqlClient.SqlException)
                {
                    await retryOptions.Delay(counter, ex);
                }
                counter++;
            }
            return default;
        }

        public void Do(Action action)
            => Do(() =>
            {
                action();
                return true;
            });

        public async Task DoAsync(Func<Task> action)
            => await DoAsync(async () =>
            {
                await action();
                return true;
            });
    }
}