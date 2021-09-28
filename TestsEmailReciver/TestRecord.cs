using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsEmailReciver
{
	record TestRecord(string StudentName, string Class, string TestName, int Mark, int Percentage, int Scores)
	{
		public string Url { get; init; }

		public DateTime? PassDate { get; init; }

		public TimeSpan? PassingTime { get; init; }

		public string OriginMessage { get; init; }
	}
}
