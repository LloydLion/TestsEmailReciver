using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsEmailReciver
{
	interface INotifyEnumerable<out T> : IEnumerable<T>, INotifyCollectionChanged
	{

	}
}
