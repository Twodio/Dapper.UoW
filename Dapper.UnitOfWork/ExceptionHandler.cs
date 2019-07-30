using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.UnitOfWork
{
    public class ExceptionHandler
    {
        private bool _hasException { get; set; }
        public static readonly int[] Errors =
        {
            20,    // The instance of SQL Server you attempted to connect to does not support encryption.
			64,    // A connection was successfully established with the server, but then an error occurred during the login process.
			121,   // The semaphore timeout period has expired
			233,   // The client was unable to establish a connection because of an error during connection initialization process before login.
			1205,  // Deadlock
			10053, // A transport-level error has occurred when receiving results from the server.
			10054, // A transport-level error has occurred when sending the request to the server.
			10060, // A network-related or instance-specific error occurred while establishing a connection to SQL Server.
			10928, // Resource ID: %d. The %s limit for the database is %d and has been reached. For more information,
			10929, // Resource ID: %d. The %s minimum guarantee is %d, maximum limit is %d and the current usage for the database is %d.
			40197, // The service has encountered an error processing your request. Please try again.
			40501, // The service is currently busy. Retry the request after 10 seconds. Code: (reason code to be decoded).
			40613, // Database XXXX on server YYYY is not currently available. Please retry the connection later.
			41301, // Dependency failure: a dependency was taken on another transaction that later failed to commit.
			41302, // The current transaction attempted to update a record that has been updated since the transaction started.
			41305, // The current transaction failed to commit due to a repeatable read validation failure.
			41325, // The current transaction failed to commit due to a serializable validation failure.
			41839, // Transaction exceeded the maximum number of commit dependencies.
			49918, // Cannot process request. Not enough resources to process request.
			49919, // Cannot process create or update request. Too many create or update operations in progress for subscription "%ld".
			49920  // Cannot process request. Too many operations in progress for subscription "%ld".
        };
        private ExceptionDispatchInfo _capturedException;

        //public enum SQLERROR
        //{
        //    NOT_SUPPORTED_ENCRYPTION = 20,    // The instance of SQL Server you attempted to connect to does not support encryption.
        //    LOGIN_FAILURE = 64,    // A connection was successfully established with the server, but then an error occurred during the login process.
        //    TIMEOUT = 121,   // The semaphore timeout period has expired
        //    LOGIN_INITIALIZATION = 233,   // The client was unable to establish a connection because of an error during connection initialization process before login.
        //    DEADLOCK = 1205,  // Deadlock
        //    FETCH_RESULT = 10053, // A transport-level error has occurred when receiving results from the server.
        //    SEND_RESULT = 10054, // A transport-level error has occurred when sending the request to the server.
        //    CONNECTION_FAILURE = 10060, // A network-related or instance-specific error occurred while establishing a connection to SQL Server.
        //    DATABASE_LIMIT_REACHED = 10928, // Resource ID: %d. The %s limit for the database is %d and has been reached. For more information,
        //    DATABASE_LIMIT = 10929, // Resource ID: %d. The %s minimum guarantee is %d, maximum limit is %d and the current usage for the database is %d.
        //    SERVICE_FAILURE = 40197, // The service has encountered an error processing your request. Please try again.
        //    SERVICE_BUSY = 40501, // The service is currently busy. Retry the request after 10 seconds. Code: (reason code to be decoded).
        //    DATABSE_UNAVAILABLE = 40613, // Database XXXX on server YYYY is not currently available. Please retry the connection later.
        //    TRANSACTION_DEPENDECY = 41301, // Dependency failure: a dependency was taken on another transaction that later failed to commit.
        //    TRANSACTION_UPDATED = 41302, // The current transaction attempted to update a record that has been updated since the transaction started.
        //    TRANSACTION_COMMIT_READ = 41305, // The current transaction failed to commit due to a repeatable read validation failure.
        //    TRANSACTION_COMMIT_SERIALIZATION = 41325, // The current transaction failed to commit due to a serializable validation failure.
        //    TRANSACTION_LIMIT = 41839, // Transaction exceeded the maximum number of commit dependencies.
        //    RESOURCES_FAILURE = 49918, // Cannot process request. Not enough resources to process request.
        //    RESOURCES_SUBCRIPTION = 49919, // Cannot process create or update request. Too many create or update operations in progress for subscription "%ld".
        //    RESOURCES_OVERLOAD = 49920  // Cannot process request. Too many operations in progress for subscription "%ld".
        //}
        //private ExceptionDispatchInfo _capturedException;

        public bool RetryOn(Exception ex)
        {
            if (!(ex is SqlException))
            {
                throw ex; 
            }
            //foreach(SqlError error in Errors)
            //{
            //    return error.
            //}
            return false;
        }

        public ExceptionHandler(Exception exception, Func<Task> action = null)
        {
            _capturedException = ExceptionDispatchInfo.Capture(exception);
            if(action != null)
            {
                action();
            } else
            {
                _hasException = true;
            }
        }

        public bool HasException
        {
            get { return _hasException; }
            set { _hasException = value; }
        }

        public ExceptionDispatchInfo ReturnException()
            => _capturedException;
    }
}
