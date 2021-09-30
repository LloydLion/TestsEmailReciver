#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsEmailReciver
{
	class TestRecordComposeParser
	{
		private readonly TestRecordParser[] parsers;


		public TestRecordComposeParser(params TestRecordParser[] parsers)
		{
			this.parsers = parsers;
		}


		public TestRecord? Parse(string sender, string message, out bool invalidSender, out bool canParse)
		{
			canParse = invalidSender = false;

			var parser = parsers.SingleOrDefault(s => s.SednerLogin == sender);

			if (parser == null)
			{
				invalidSender = true;
				return null;
			}
			else
			{
				return parser.Parse(message, out canParse);
			}
		}
	}
}
