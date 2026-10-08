using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HelloAvalonia.ViewModels;

// WinFormsの GreetingViewModel と同じ内容(違いは namespace と基底クラス ViewModelBase、partial property)
public partial class GreetingViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Greeting { get; set; } = string.Empty;

    [RelayCommand]
    private void Greet()
    {
        Greeting = $"こんにちは、{Name}さん！";
    }
}
