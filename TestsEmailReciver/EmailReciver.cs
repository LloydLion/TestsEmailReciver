using Limilabs.Client.IMAP;
using Limilabs.Mail;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestsEmailReciver
{
	class EmailReciver
	{
		private Imap client;


		public AccauntInfo Info { get; }


		public EmailReciver(AccauntInfo info)
		{
			Info = info;
		}


		~EmailReciver()
		{
			try
			{
				client.Close();
				client.Dispose();
			}
			catch (Exception) { }
		}


		public void Connect()
		{
			client = new Imap();

			client.ConnectSSL(Info.ImapServer);
			client.UseBestLogin(Info.Email, Info.Password);
		}

		public IEnumerable<IMail> GetInbox()
		{
			client.SelectInbox();
			var newEmails = client.Search(Flag.All);
			newEmails.Reverse();
			return newEmails.Where((s, i) => i <= 35).ToArray().Select(s =>  new MailBuilder().CreateFromEml(client.GetMessageByUID(s)));
		}
	}
}
