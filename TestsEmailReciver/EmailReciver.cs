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


		public AccountInfo Info { get; }


		public EmailReciver(AccountInfo info)
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

		public EmailDownloader GetInbox()
		{
			client.SelectInbox();

			var downloader = new EmailDownloader(this);
			downloader.PreLoad();
			return downloader;
		}


		public class EmailDownloader
		{
			private readonly EmailReciver owner;
			private long[] emailsUids;
			private readonly List<IMail> cache = new();


			public EmailDownloader(EmailReciver owner)
			{
				this.owner = owner;
			}


			public void PreLoad()
			{
				var list = owner.client.Search(Flag.All);
				list.Reverse();
				emailsUids = list.ToArray();
			}

			public IEnumerable<IMail> Load(int count)
			{
				if(cache.Count >= count)
				{
					return cache.Where((_, i) => i < count);
				}
				else
				{
					return cache.Concat(emailsUids[cache.Count..count].Select(s =>
					{
						var email = new MailBuilder().CreateFromEml(owner.client.GetMessageByUID(s));
						cache.Add(email);
						return email;
					}));
				}
			}

			public IEnumerable<IMail> LoadMore(int count)
			{
				return Load(cache.Count + count);
			}
		}
	}
}
