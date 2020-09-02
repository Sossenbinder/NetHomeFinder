using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetHomeFinder.Common.Eventing
{
	public class AsyncEvent
	{
		private readonly List<Func<Task>> _registeredFuncs;

		public AsyncEvent()
		{
			_registeredFuncs = new List<Func<Task>>();
		}

		public async Task<IEnumerable<Exception>> Raise()
		{
			var accumulatedExceptions = new List<Exception>();

			foreach (var registeredAction in _registeredFuncs)
			{
				try
				{
					await registeredAction();
				}
				catch (Exception e)
				{
					accumulatedExceptions.Add(e);
				}
			}

			return accumulatedExceptions;
		}

		public void Register(Action actionItem)
		{
			_registeredFuncs.Add(() =>
			{
				actionItem();

				return Task.CompletedTask;
			});
		}

		public void Register(Func<Task> actionItem)
		{
			_registeredFuncs.Add(actionItem);
		}

		public void Unregister(Func<Task> actionItem)
		{
			_registeredFuncs.Remove(actionItem);
		}

		internal List<Func<Task>> GetAllRegisteredEvents()
		{
			return _registeredFuncs;
		}
	}

	public class AsyncEvent<TEventArgs>
	{
		private readonly List<Func<TEventArgs, Task>> _registeredFuncs;

		public AsyncEvent()
		{
			_registeredFuncs = new List<Func<TEventArgs, Task>>();
		}

		public async Task<IEnumerable<Exception>> Raise(TEventArgs eventArgs)
		{
			var exceptions = new List<Exception>();

			foreach (var registeredAction in _registeredFuncs)
			{
				try
				{
					await registeredAction(eventArgs);
				}
				catch (Exception e)
				{
					exceptions.Add(e);
				}
			}

			return exceptions;
		}

		public void Register(Action<TEventArgs> actionItem)
		{
			_registeredFuncs.Add(args =>
			{
				actionItem(args);

				return Task.CompletedTask;
			});
		}

		public void Register(Func<TEventArgs, Task> actionItem)
		{
			_registeredFuncs.Add(actionItem);
		}

		public void Unregister(Func<TEventArgs, Task> actionItem)
		{
			_registeredFuncs.Remove(actionItem);
		}

		internal List<Func<TEventArgs, Task>> GetAllRegisteredEvents()
		{
			return _registeredFuncs;
		}
	}
}