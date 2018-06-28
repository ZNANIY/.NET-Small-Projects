﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Observers
{
    public delegate void HandleDelegate(object eventData);

    public class StackOperationsLogger
	{
		private readonly Observer observer = new Observer();
		public void SubscribeOn<T>(ObservableStack<T> stack)
		{
			stack.Add(observer);
		}

		public string GetLog()
		{
			return observer.Log.ToString();
		}
	}

	public interface IObserver
	{
		void HandleEvent(object eventData);
	}

	public class Observer : IObserver
	{
		public StringBuilder Log = new StringBuilder();

		public void HandleEvent(object eventData)
		{
			Log.Append(eventData);
		}
	}

	public class ObservableStack<T>
	{		
		public event HandleDelegate HandleEvent;

		public void Add(IObserver observer)
		{
			HandleEvent += observer.HandleEvent;
		}

		public void Notify(object eventData)
		{
            HandleEvent?.Invoke(eventData);
        }

		public void Remove(IObserver observer)
		{
			HandleEvent -= observer.HandleEvent;
		}

		List<T> data = new List<T>();

		public void Push(T obj)
		{
			data.Add(obj);
			Notify(new StackEventData<T> { IsPushed = true, Value = obj });
		}

		public T Pop()
		{
			if (data.Count == 0)
				throw new InvalidOperationException();
			var result = data[data.Count - 1];
			Notify(new StackEventData<T> { IsPushed = false, Value = result });
			return result;

		}
	}
}
