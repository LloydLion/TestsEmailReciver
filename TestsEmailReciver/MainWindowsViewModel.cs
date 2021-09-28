using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestsEmailReciver
{
	class MainWindowsViewModel : INotifyPropertyChanged
	{
		private TestRecord selectedRecord;

		public MainWindowsViewModel(TestRecord[] records)
		{
			Records = records;
		}


		public event PropertyChangedEventHandler PropertyChanged;


		public ICollection<TestRecord> Records { get; }

		public TestRecord SelectedRecord { get => selectedRecord; set { selectedRecord = value; OnPropertyChanged(); } }


		public void OnPropertyChanged([CallerMemberName] string caller = "Caller member name")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
		}
	}
}
