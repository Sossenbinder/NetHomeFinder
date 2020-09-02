using System.Threading.Tasks;

namespace NetHomeFinder.Common.Extensions
{
	public static class TaskExtensions
	{
		public static Task IgnoreTaskCancelledException(this Task originalTask)
		{
			return originalTask.ContinueWith(t => t.Exception?.Handle(x => x is TaskCanceledException));
		}
	}
}