using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MyWinFormsApp;

// partialが必須（ソースジェネレーターが使うため）
public partial class GreetingViewModel : ObservableObject
{
  // [ObservableProperty]を付けるだけでプロパティ+変更通知が自動生成される
  [ObservableProperty]
  private string _name = string.Empty;

  [ObservableProperty]
  private string _greeting = string.Empty;

  // [RelayCommand]を付けるだけでICommandが自動生成される
  [RelayCommand]
  private void Greet()
  {
    Greeting = $"こんにちは、{Name}さん！";
  }
}