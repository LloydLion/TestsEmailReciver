using LloydLion.Serialization.Common;
using LloydLion.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TestsEmailReciver.Parsers;

[assembly: GenerateGSCMethod(typeof(TestsEmailReciver.App), nameof(TestsEmailReciver.App.InitGSC))]

namespace TestsEmailReciver
{
	public partial class App : Application
	{
		internal MainWindowViewModel StartupDataContext { get; private set; }

		internal AccountManager Account { get; set; }

		internal Serializator DefaultSerializator { get; private set; }


		private void Application_Startup(object sender, StartupEventArgs e)
		{
			DefaultSerializator = new Serializator(GlobalSerializationContext.MainContext, typeof(JsonSerializator));

			Account = new AccountManager(new FileInfo("account.json"));

			Account.NewAccountUsed += (s, a) =>
			{
				var reciver = new EmailReciver(Account.Account);
				var composeParser = new TestRecordComposeParser(new OnlineTestPadParser());

				StartupDataContext = new MainWindowViewModel(reciver, composeParser);
			};

			if(Account.CanLoad)
			{
				Account.LoadAccount();
			}
			else
			{
				var window = new AccountSettingsWindow(null);
				window.ShowDialog();
				window.Close();

				try
				{
					Account.UseNewAccount(window.NewAccount);
					Account.SaveAccount();
					MessageBox.Show("Данные введены правильно. Перезапустите приложение.", "Успех");
					Shutdown();
					return;
				}
				catch(Exception)
				{
					MessageBox.Show("Невозможно загрузить письма. Проверьте интернет или правельность данных аккаунта.", "Ошибка загруски");
					Shutdown();
					return;
				}
			}
		}

		public static void InitGSC()
		{
			GlobalSerializationContext.CreateAssemblyInstance((s) =>
			{
				s
					.AddFormat(new SerializationFormatInfo("json", ReadOnlySet<string>.FromCollection(new string[] { ".json" })))
					.AddCore(new JsonSerializator())
					.AddBuilderFactory(() => new AccountInfoBuilder(), typeof(AccountInfo))
					.AddProcessor(new AccountInfoProcessor());
			});
		}
	}
}
