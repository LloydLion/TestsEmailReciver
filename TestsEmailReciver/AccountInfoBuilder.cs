using LloydLion.Serialization.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsEmailReciver
{
	class AccountInfoBuilder : IObjectBuilder
	{
		private Dictionary<string, object> values = new Dictionary<string, object>();


		public Type BuilderType => typeof(AccountInfo);


		public object Build()
		{
			return new AccountInfo((string)values["ImapServer"], (int)values["ServerPort"], (string)values["Email"], (string)values["Password"]);
		}

		public void WithObjectBuilder(string property, IObjectBuilder builder)
		{
			throw new NotImplementedException();
		}

		public void WithValue(string property, object obj)
		{
			values.Add(property, obj);
		}
	}
}
