using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsEmailReciver
{
	static class NotifyExtensions
	{
		public static INotifyEnumerable<T> AsSimpleNotify<T>(this IEnumerable<T> obj)
		{
			return new NotifyEnumerable<T>(obj);
		}


		private class NotifyEnumerable<T> : INotifyEnumerable<T>
		{
			private readonly IEnumerable<T> obj;


			public NotifyEnumerable(IEnumerable<T> obj)
			{
				this.obj = obj;
			}


			public event NotifyCollectionChangedEventHandler CollectionChanged;


			public IEnumerator<T> GetEnumerator()
			{
				CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
				return obj.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}
	}
}
