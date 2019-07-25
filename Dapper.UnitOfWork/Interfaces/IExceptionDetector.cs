using System;

namespace Dapper.UnitOfWork.Interfaces
{
	public interface IExceptionDetector
	{
		bool ShouldRetryOn(Exception ex);
	}
}