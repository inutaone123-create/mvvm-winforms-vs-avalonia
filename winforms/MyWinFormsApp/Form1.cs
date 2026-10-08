namespace MyWinFormsApp;

public partial class Form1 : Form
{
    private readonly GreetingViewModel _vm;

    // コントロールをフィールドとして宣言
    private TextBox textBoxName = new();
    private Label labelGreeting = new();
    private Button buttonGreet = new();

    public Form1()
    {
        InitializeComponent();

        // コントロールの配置
        textBoxName.Location = new Point(20, 20);
        textBoxName.Width = 200;

        labelGreeting.Location = new Point(20, 60);
        labelGreeting.Width = 300;
        labelGreeting.Text = "ここに挨拶が表示されます";

        buttonGreet.Location = new Point(20, 100);
        buttonGreet.Text = "挨拶する";

        Controls.AddRange([textBoxName, labelGreeting, buttonGreet]);

        // ViewModelの生成とバインディング
        _vm = new GreetingViewModel();

        // DataContextを設定（.NET 8以降）
        this.DataContext = _vm;

        // テキストボックスのバインディング
        textBoxName.DataBindings.Add(
            nameof(textBoxName.Text), _vm, nameof(_vm.Name),
            false, DataSourceUpdateMode.OnPropertyChanged);

        labelGreeting.DataBindings.Add(
            nameof(labelGreeting.Text), _vm, nameof(_vm.Greeting));

        // .NET 8以降はButtonにCommandプロパティが追加された！
        buttonGreet.Command = _vm.GreetCommand;
    }
}