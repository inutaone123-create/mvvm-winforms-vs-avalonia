// 記事1「昔の苦労」の節で比較するための自前実装です(このアプリでは未使用。ViewModel は CommunityToolkit.Mvvm を使用)。
using System.ComponentModel;

namespace MyWinFormsApp;

// ボイラープレートの塊
public class NotificationObject : INotifyPropertyChanged
{
  public event PropertyChangedEventHandler? PropertyChanged;

  protected virtual void RaisePropertyChanged(string propertyName)
  {
    var handler = PropertyChanged;
    if (handler != null)
    {
      handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}