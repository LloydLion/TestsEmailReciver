using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestsEmailReciver
{
	class DelegateCommand<T> : ICommand
	{
		public Action<T> Action { get; }

		public Predicate<T> Predicate { get; }


		public event EventHandler CanExecuteChanged;


		public DelegateCommand(Action<T> action, Predicate<T> predicate)
		{
			Action = action;
			Predicate = predicate;
		}
		
		public DelegateCommand(Action<T> action)
		{
			Action = action;
			Predicate = (a) => true;
		}


		public bool CanExecute(object parameter)
		{
			return Predicate((T)parameter);
		}

		public void Execute(object parameter)
		{
			Action((T)parameter);
		}
	}

	class DelegateCommand : ICommand
	{
		public Action Action { get; }


		public event EventHandler CanExecuteChanged;


		public DelegateCommand(Action action)
		{
			Action = action;
		}


		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			Action();
		}
	}
}
