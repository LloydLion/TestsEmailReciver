using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestsEmailReciver
{
	class AccountManager
	{
		private readonly FileInfo file;


		public AccountManager(FileInfo file)
		{
			this.file = file;
		}


		public AccountInfo Account { get; private set; }

		public bool CanLoad => file.Exists;


		public event EventHandler NewAccountUsed;


		public void UseNewAccount(AccountInfo account)
		{
			Account = account;
			NewAccountUsed?.Invoke(this, new EventArgs());
		}

		public void SaveAccount()
		{
			using var file = this.file.OpenWrite();

			(Application.Current as App).DefaultSerializator.SerializeAsync(Account, new StreamWriter(file), s => { }, new { UseIndentedFormatting = true }).Wait();
		}

		public void LoadAccount()
		{
			using var file = this.file.OpenRead();

			UseNewAccount((AccountInfo)(Application.Current as App).DefaultSerializator.DeserializeAsync(new StreamReader(file), typeof(AccountInfo), s => { }).Result);
		}
	}
}
