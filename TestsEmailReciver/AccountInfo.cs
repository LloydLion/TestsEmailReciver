using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestsEmailReciver
{
	record AccountInfo(string ImapServer, int ServerPort, string Email, string Password)
	{
		
	}

	class AccountBuilder
	{
		public string ImapServer { get; set; }

		public int ServerPort { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }


		public AccountInfo Build()
		{
			return new AccountInfo(ImapServer, ServerPort, Email, Password);
		}

		public void From(AccountInfo account)
		{
			ImapServer = account.ImapServer;
			ServerPort = account.ServerPort;
			Email = account.Email;
			Password = account.Password;
		}
	}
}
