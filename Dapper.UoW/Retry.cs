using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.UoW
{
    public class Retry
    {
        private static Dictionary<int, String> TransientErrors = new Dictionary<int, string>();
        public static T Invoke<T>(Func<T> action, RetryOptions Options){
            if(action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (Options == null)
            {
                return action();
            }

            var counter = 1;
            while (counter <= Options.MaxRetries)
            {
                try
                {
                    return action();
                }catch(Exception ex)
                {
                    HandleException(ex, counter, Options).GetAwaiter().GetResult();
                }
                counter++;
            }

            return default;
        }

        public static void Invoke(Action action, RetryOptions options)
        {
            Invoke(() =>
            {
                action();
                return true;
            }, options);
        }

        public static async Task<T> InvokeAsync<T>(Func<Task<T>> action, RetryOptions Options)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (Options == null)
            {
                return await action();
            }

            var counter = 1;
            while (counter <= Options.MaxRetries)
            {
                try
                {
                    return await action();
                }
                catch (Exception ex)
                {
                    await HandleException(ex, counter, Options);
                }
                counter++;
            }

            return default;
        }

        public static async Task InvokeAsync(Func<Task> action, RetryOptions Options)
        {
            await InvokeAsync(async () =>
               {
                   await action();
                   return true;
               }, Options);
        }

        private static Task HandleException(Exception Exception, int counter, RetryOptions options)
        {
            if(!IsTransient(Exception) || counter >= options.MaxRetries)
            {
                throw Exception;
            }
            //System.Diagnostics.Debug.WriteLine($"Erro: {Exception.Message}, a retomar processo ({counter})");
            TimeSpan sleepTime = TimeSpan.FromMilliseconds(Math.Pow(options.WaitMillis, counter));
            //System.Diagnostics.Debug.WriteLine($"Tempo: {sleepTime}");
            return Task.Delay(sleepTime);
        }

        private static bool IsTransient(Exception Exception)
        {
            if (!(Exception is SqlException sqlException))
            {
                return Exception is TimeoutException;
            }

            SqlException sqlEx = (SqlException)Exception;

            return TransientErrors.ContainsKey(sqlEx.Number);
            //return true;
        }

        public static void AddError(int Code, String Name)
        {
            TransientErrors.Add(Code, Name);
        }

        public static bool RemoveError(int Code)
        {
            return TransientErrors.Remove(Code);
        }

        public static void SetSource(Dictionary<int, string> Source)
        {
            if(Source == null)
            {
                throw new ArgumentNullException(nameof(Source));
            }

            TransientErrors = Source;
        }
    }
}
