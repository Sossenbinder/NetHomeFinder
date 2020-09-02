using System;

namespace NetHomeFinder.Common.Utils
{
	public abstract class BaseLazyFactory<T>
	{
		private readonly Lazy<T> _value;

		protected BaseLazyFactory(Func<T> factory)
		{
			_value = new Lazy<T>(factory);
		}

		public T Get() => _value.Value;
	}
}