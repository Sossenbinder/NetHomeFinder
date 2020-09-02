using System.Threading;
using System.Threading.Tasks;

namespace NetHomeFinder.Common.Extensions
{
	public static class CancellationTokenExtensions
	{
		public static Task AsTask(this CancellationToken cancellationToken)
		{
			var tcs = new TaskCompletionSource<object>();

			cancellationToken.Register(() => tcs.TrySetCanceled());

			return tcs.Task;
		}
	}
}