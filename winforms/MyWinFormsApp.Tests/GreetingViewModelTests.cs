namespace MyWinFormsApp.Tests;

public class GreetingViewModelTests
{
    [Fact]
    public void Greet_名前を入力して挨拶コマンドを実行すると_挨拶文が設定される()
    {
        // Arrange
        var vm = new GreetingViewModel();
        vm.Name = "太郎";

        // Act
        vm.GreetCommand.Execute(null);

        // Assert
        Assert.Equal("こんにちは、太郎さん！", vm.Greeting);
    }

    [Fact]
    public void Greet_名前が空の場合_挨拶文が空名前で設定される()
    {
        // Arrange
        var vm = new GreetingViewModel();
        vm.Name = string.Empty;

        // Act
        vm.GreetCommand.Execute(null);

        // Assert
        Assert.Equal("こんにちは、さん！", vm.Greeting);
    }

    [Fact]
    public void Name_変更時_PropertyChangedが発火する()
    {
        // Arrange
        var vm = new GreetingViewModel();
        var changedProperties = new List<string?>();
        vm.PropertyChanged += (s, e) => changedProperties.Add(e.PropertyName);

        // Act
        vm.Name = "花子";

        // Assert
        Assert.Contains(nameof(vm.Name), changedProperties);
    }
}