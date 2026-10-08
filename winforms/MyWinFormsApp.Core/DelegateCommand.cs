// 記事1「昔の苦労」の節で比較するための自前実装です(このアプリでは未使用。ViewModel は CommunityToolkit.Mvvm を使用)。
using System.Windows.Input;

namespace MyWinFormsApp;

// DelegateCommandも自前で書く
public class DelegateCommand : ICommand
{
  private readonly Action _execute;
  private readonly Func<bool>? _canExecute;

  public DelegateCommand(Action execute, Func<bool>? canExecute = null)
  {
    _execute = execute;
    _canExecute = canExecute;
  }

  public event EventHandler? CanExecuteChanged;

  public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

  public void Execute(object? parameter) => _execute();

  public void RaiseCanExecuteChanged()
      => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}