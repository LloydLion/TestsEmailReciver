using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsEmailReciver
{
	class Filtrator<T>
	{
		private Dictionary<string, Filter> filters = new Dictionary<string, Filter>();


		public IReadOnlyDictionary<string, Filter> Filters { get => filters; }


		public event EventHandler FiltersChanged;


		public FilterHandler AddFilter(Filter filter)
		{
			filters.Add(filter.UniqueKey, filter);
			FiltersChanged?.Invoke(this, new EventArgs());
			return new FilterHandler(filter.UniqueKey, this);
		}

		public FilterHandler ReplaceFilter(Filter filter)
		{
			filters[filter.UniqueKey] = filter;
			FiltersChanged?.Invoke(this, new EventArgs());
			return new FilterHandler(filter.UniqueKey, this);
		}

		public void RemoveFilter(string key)
		{
			filters.Remove(key);
			FiltersChanged?.Invoke(this, new EventArgs());
		}

		public IReadOnlyList<T> ApplyFilters(IReadOnlyList<T> origin)
		{
			return origin.Where(s => filters.Values.All(f => f.FilterPredicate(s))).ToArray();
		}
		
		public IReadOnlyCollection<T> ApplyFilters(IReadOnlyCollection<T> origin)
		{
			return origin.Where(s => filters.Values.All(f => f.FilterPredicate(s))).ToArray();
		}


		public struct Filter
		{
			public Filter(string uniqueKey, Predicate<T> filterPredicate)
			{
				UniqueKey = uniqueKey;
				FilterPredicate = filterPredicate;
			}


			public string UniqueKey { get; }

			public Predicate<T> FilterPredicate { get; }
		}

		public struct FilterHandler : IDisposable
		{
			public FilterHandler(string uniqueKey, Filtrator<T> owner)
			{
				UniqueKey = uniqueKey;
				Owner = owner;
			}


			public string UniqueKey { get; }

			public Filtrator<T> Owner { get; }


			public void Dispose()
			{
				DisableFilter();
			}

			public void DisableFilter()
			{
				Owner.RemoveFilter(UniqueKey);
			}
		}
	}
}
