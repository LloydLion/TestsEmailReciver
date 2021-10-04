using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		private IReadOnlyList<TestRecord> records;
		private readonly EmailReciver reciver;
		private readonly TestRecordComposeParser parser;


		public MainWindowViewModel(EmailReciver reciver, TestRecordComposeParser parser)
		{
			this.reciver = reciver;
			this.parser = parser;

			reciver.Connect();

			Refresh();

			RefreshCommand = new DelegateCommand(Refresh);
		}


		public event PropertyChangedEventHandler PropertyChanged;


		public ICommand RefreshCommand { get; }


		public IReadOnlyList<TestRecord> Records { get => records; private set { records = value; OnPropertyChanged(); } }

		public TestRecord SelectedRecord { get => selectedRecord; set { selectedRecord = value; OnPropertyChanged(); } }


		public void OnPropertyChanged([CallerMemberName] string caller = "Caller member name")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
		}

		private void Refresh()
		{
			var res = MessageBox.Show("Получение новых писем с электронной почты может занять много времени.\n" +
				"Вы уверены, что хотите продолжить?", "Предупреждение о долгой операции", MessageBoxButton.OKCancel);

			if (res == MessageBoxResult.Cancel) return;

			try
			{
				var emailCol = reciver.GetInbox();

				records = emailCol.Select(s => (parser.Parse(s.From[0].Address, s.Html, out _, out bool canParse), canParse)).Where(s => s.canParse).Select(s => s.Item1).ToList();
			}
			catch(Exception)
			{
				MessageBox.Show("Невозможно загрузить письма. Проверьте интернет или правельность данных аккаунта.", "Ошибка загруски");
			}
		}
	}
}
