using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TestsEmailReciver
{
	class MainWindowViewModel : INotifyPropertyChanged
	{
		private TestRecord selectedRecord;
		private IReadOnlyList<TestRecord> pureRecords;
		private readonly Filtrator<TestRecord> filtrator = new();
		private readonly EmailReciver reciver;
		private readonly TestRecordComposeParser parser;
		private EmailReciver.EmailDownloader emailLoader;


		public MainWindowViewModel(EmailReciver reciver, TestRecordComposeParser parser)
		{
			this.reciver = reciver;
			this.parser = parser;

			reciver.Connect();
			RefreshEmails();

			filtrator.FiltersChanged += (s, a) => { OnPropertyChanged(nameof(Records)); };

			RefreshCommand = new DelegateCommand(RefreshCommandDelegate);
			SetFilterCommand = new DelegateCommand<string[]>(s => SetFilter(s[0], s[1]));
			OpenAccountWindowCommand = new DelegateCommand(OpenAccountWindow);
			LoadMoreEmailsCommand = new DelegateCommand<int>(LoadMoreEmailsCommandDelegate, s => s >= 0);
		}


		public event PropertyChangedEventHandler PropertyChanged;


		public ICommand RefreshCommand { get; }

		public ICommand SetFilterCommand { get; }

		public ICommand OpenAccountWindowCommand { get; }

		public ICommand LoadMoreEmailsCommand { get; }


		private IReadOnlyList<TestRecord> PureRecords { get => pureRecords; set { pureRecords = value; OnPropertyChanged(nameof(Records)); OnPropertyChanged(nameof(Tests)); } }

		public IReadOnlyList<TestRecord> Records { get => filtrator.ApplyFilters(PureRecords); }

		public IReadOnlyList<string> Tests { get => Records.Select(s => s.TestName).Distinct().ToArray(); }

		public TestRecord SelectedRecord { get => selectedRecord; set { selectedRecord = value; OnPropertyChanged(); } }


		private void OnPropertyChanged([CallerMemberName] string caller = "Caller member name")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
		}

		private void RefreshCommandDelegate()
		{
			var res = MessageBox.Show("Получение новых писем с электронной почты может занять много времени.\n" +
				"Вы уверены, что хотите продолжить?", "Предупреждение о долгой операции", MessageBoxButton.OKCancel);

			if (res == MessageBoxResult.Cancel) return;

			try
			{
				RefreshEmails();
				MessageBox.Show("Письма обновлены", "Успех загруски");
			}
			catch(Exception)
			{
				MessageBox.Show("Невозможно загрузить письма. Проверьте интернет или правельность данных аккаунта.", "Ошибка загруски");
			}

		}

		public void RefreshEmails()
		{
			emailLoader = reciver.GetInbox();
			LoadEmails(35);
		}

		public void LoadEmails(int count)
		{
			PureRecords = emailLoader.Load(count).Select(s => (parser.Parse(s.From[0].Address, s.Html, out _, out bool canParse), canParse)).Where(s => s.canParse).Select(s => s.Item1).ToList();
		}

		public void LoadMoreEmails(int count)
		{
			PureRecords = emailLoader.LoadMore(count).Select(s => (parser.Parse(s.From[0].Address, s.Html, out _, out bool canParse), canParse)).Where(s => s.canParse).Select(s => s.Item1).ToList();
		}

		private void LoadMoreEmailsCommandDelegate(int count)
		{
			var res = MessageBox.Show("Получение новых писем с электронной почты может занять много времени.\n" +
						"Вы уверены, что хотите продолжить?", "Предупреждение о долгой операции", MessageBoxButton.OKCancel);

			if (res == MessageBoxResult.Cancel) return;

			try
			{
				LoadMoreEmails(count);
				MessageBox.Show("Письма обновлены", "Успех загруски");
			}
			catch (Exception)
			{
				MessageBox.Show("Невозможно загрузить письма. Проверьте интернет или правельность данных аккаунта.", "Ошибка загруски");
			}
		}

		public Filtrator<TestRecord>.FilterHandler SetFilter(string filterCode, string value)
		{
			return filterCode switch
			{
				"class" => filtrator.AddFilter(new Filtrator<TestRecord>.Filter("class", (a) => a.Class == value)),
				"name" => filtrator.AddFilter(new Filtrator<TestRecord>.Filter("name", (a) => a.StudentName.StartsWith(value))),
				"test" => filtrator.AddFilter(new Filtrator<TestRecord>.Filter("test", (a) => a.TestName == value)),
				_ => throw new ArgumentException("Invalid filter code - " + filterCode, nameof(filterCode)),
			};
		}

		public void OpenAccountWindow()
		{
		invalidData:
			var window = new AccountSettingsWindow((Application.Current as App).Account.Account);
			window.ShowDialog();

			MessageBox.Show("Сейчас будут загружены новые письма с почты. Это может занять много времени.", "Уведомление");

			try
			{
				(Application.Current as App).Account.UseNewAccount(window.NewAccount);
				MessageBox.Show("Письма обновлены", "Успех загруски");
			}
			catch (Exception)
			{
				MessageBox.Show("Невозможно загрузить письма. Проверьте интернет или правельность данных аккаунта.", "Ошибка загруски");
				goto invalidData;
			}

			(Application.Current as App).Account.SaveAccount();
		}
	}
}
