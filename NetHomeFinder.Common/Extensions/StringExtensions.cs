using System;

namespace NetHomeFinder.Common.Extensions
{
	public static class StringExtensions
	{
		/// <summary>
		/// Dumps a string to the stdout with utc timestamp prefix
		/// </summary>
		/// <param name="stringToDump"></param>
		public static void LogWithUtcPrefix(this string stringToDump)
		{
			Console.WriteLine($"{DateTime.UtcNow.ToShortTimeString()} {stringToDump}");
		}
	}
}