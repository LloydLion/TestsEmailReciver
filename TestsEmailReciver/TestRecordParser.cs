using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsEmailReciver
{
	abstract class TestRecordParser
	{
		public TestRecordParser(string sednerLogin)
		{
			SednerLogin = sednerLogin;
		}


		public string SednerLogin { get; }


		public abstract TestRecord Parse(string message, out bool canParse);
	}
}
