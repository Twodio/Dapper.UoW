using Dapper.UnitOfWork.Interfaces;
using System;
using System.Threading.Tasks;

namespace Dapper.UnitOfWork
{
	public class RetryOptions
	{
        public int _maxRetries { get; }
        public int _delayMilliseconds { get; }
        public int _maxDelayMilliseconds { get; }
        private int _pow;

        public RetryOptions(int maxRetries, int delayMilliseconds, int maxDelayMilliseconds)
        {
            _maxRetries = maxRetries;
            _delayMilliseconds = delayMilliseconds;
            _maxDelayMilliseconds = maxDelayMilliseconds;
            _pow = 1;
        }

        public Task Delay(int counter, Exception exception = default)
        {
            if(!SqlTransientExceptionDetector.ShouldRetryOn(exception) || counter == _maxRetries)
            {
                throw exception;
            }
            if(counter < 31)
            {
                _pow = _pow << 1;
            }
            var timer = Math.Min(_delayMilliseconds * (_pow - 1) / 2, _maxDelayMilliseconds);
            return Task.Delay(timer);
        }
    }
}