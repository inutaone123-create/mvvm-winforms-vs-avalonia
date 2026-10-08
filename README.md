# mvvm-winforms-vs-avalonia

同じ画面・同じ挙動の「挨拶アプリ」を **WinForms** と **Avalonia** の両方で、**CommunityToolkit.Mvvm** を使って実装した対比サンプルです。2本の記事のサンプルコードをまとめています。

画面は共通で、TextBox に名前を入力して「挨拶する」ボタンを押すと、`こんにちは、{名前}さん！` が表示されます。

```
.
├── winforms/   記事1: WinForms × CommunityToolkit.Mvvm
│   ├── MyWinFormsApp/         Form(net10.0-windows)
│   ├── MyWinFormsApp.Core/    ViewModel(net10.0、Windows非依存)
│   │   ├── GreetingViewModel.cs     CommunityToolkit.Mvvm を使った ViewModel(アプリが使用)
│   │   ├── NotificationObject.cs    自前の INotifyPropertyChanged 実装(※未使用)
│   │   └── DelegateCommand.cs       自前の ICommand 実装(※未使用)
│   └── MyWinFormsApp.Tests/   xUnit
└── avalonia/   記事2: 同じ画面を Avalonia で再現
    ├── HelloAvalonia/         アプリ + ViewModel(net10.0)
    └── HelloAvalonia.Tests/   xUnit(同じ3テスト)
```

## 記事の概要

### 記事1: WinForms で MVVM(`winforms/`)
- 自前の `INotifyPropertyChanged` / `ICommand` 実装(ボイラープレート)と、`CommunityToolkit.Mvvm`(`[ObservableProperty]` / `[RelayCommand]`)での書き方を比較。
- .NET 8 以降の WinForms で `Button.Command` が使えるようになった点。
- ViewModel を `net10.0` のクラスライブラリ(`MyWinFormsApp.Core`)に分離すると、Linux でもユニットテストが動く。
- 記事: [WindowsFormsでMVVMをやろうとして自己満足で終わった話と、.NET 10ならこう書ける話](https://qiita.com/inuta-one/items/4cb48340bdfe32b15592)

### 記事2: 同じ画面を Avalonia で再現(`avalonia/`)
- `dotnet new avalonia.mvvm` のテンプレートをベースに、記事1と同じ ViewModel / 画面 / テストを移植。
- テンプレートの ViewModel は `[ObservableProperty] public partial string Name { get; set; }`(partial property 方式)。記事1の `private string _name`(フィールド方式)とは書き方が違うが、テストコードは変わらない。
- アプリ本体が `net10.0`(OS 非依存)なので、**ViewModel を別プロジェクトに分けなくても**テストから直接参照でき、Linux でもテストが通る。
- 記事: [WinFormsのMVVMサンプルをAvaloniaに移植したら、Linuxでもテストが「分離なし」で通った話](https://qiita.com/inuta-one/items/9b8c97042fb790d72994)

## 前提

- [.NET 10 SDK](https://dotnet.microsoft.com/download)(動作確認: Windows は 10.0.100、Linux コンテナは 10.0.401)
- (任意)Docker / Dev Containers

## ビルド・テスト

リポジトリのルートから実行します。

```bash
# WinForms 版
dotnet test winforms/MyWinFormsApp.Tests
dotnet build winforms/MyWinFormsApp      # Windows 以外では EnableWindowsTargeting が有効なためビルドのみ可能

# Avalonia 版
dotnet test avalonia/HelloAvalonia.Tests
dotnet build avalonia/HelloAvalonia
```

アプリの起動:

```bash
dotnet run --project winforms/MyWinFormsApp   # Windows のみ
dotnet run --project avalonia/HelloAvalonia   # ディスプレイのある環境
```

### 動作確認済みの結果

| 環境 | WinForms テスト | Avalonia テスト | 両アプリのビルド |
|---|---|---|---|
| Windows 11 / .NET 10.0.100 | 3/3 合格 | 3/3 合格 | 警告0・エラー0 |
| Linux(`mcr.microsoft.com/dotnet/sdk:10.0` コンテナ) | 3/3 合格 | 3/3 合格 | 警告0・エラー0 |

## Dev Container

`.devcontainer/devcontainer.json` は `mcr.microsoft.com/dotnet/sdk:10.0` イメージを指定するだけの最小構成です。VS Code の「Reopen in Container」か、CLI で起動できます。

```bash
npx @devcontainers/cli up --workspace-folder .
npx @devcontainers/cli exec --workspace-folder . dotnet test avalonia/HelloAvalonia.Tests
```

## 既知の注意点

- **Linux では WinForms アプリは実行できません**。`dotnet run` は Windows 専用です。テストは ViewModel を分離した `MyWinFormsApp.Core` だけを参照するので Linux でも通ります。
  Linux で WinForms アプリ自体をビルドする場合は `EnableWindowsTargeting`(`MyWinFormsApp.csproj` に設定済み)が必要です(未設定だと `NETSDK1100`)。
- **テストから WinForms アプリを直接参照しない**でください。Linux では `NETSDK1073`(`Microsoft.WindowsDesktop.App.WindowsForms` を認識できない)になります。これが `MyWinFormsApp.Core` を分けている理由です。
- **WSL 上の Docker Engine を使っている場合**(Docker Desktop なしで、Windows の Docker CLI から `tcp://127.0.0.1:2375` などで WSL 内のデーモンに接続している場合):
  - Windows のパス(`C:\...`)をバインドマウントできず、`devcontainer up` が `invalid mount config for type "bind": mount path must be absolute` で失敗します。
  - 回避策として、`devcontainer.json` に WSL 形式のパスで `workspaceMount` を指定します(パスは自分の環境に合わせて変更してください)。
    ```json
    "workspaceMount": "type=bind,source=/mnt/c/path/to/mvvm-winforms-vs-avalonia,target=/workspaces/repo",
    "workspaceFolder": "/workspaces/repo"
    ```
  - WSL のファイルシステム内(`~/...`)にクローンして開く方法もあります。
  - Git Bash から `docker` / `devcontainer exec` を使うと、`/workspaces/...` が `C:/Program Files/Git/workspaces/...` に変換されます。`MSYS_NO_PATHCONV=1` を付けてください。
  - 短命な `docker run` の標準出力が返ってこないことがありました。その場合はコンテナ内でログをファイルに書き出して読むと確実です。
- **Windows で開いたフォルダを、そのままコンテナにマウントしない**でください。Windows 側の `bin/` `obj/`(`project.assets.json` にWindowsのパスが入る)とLinuxの restore が衝突する可能性があります。`bin`/`obj` を消してから開くか、クリーンなクローンを使ってください。
- 初回の `dotnet restore` は NuGet のダウンロードで 30〜40 秒ほどかかります。

## ライセンス

[MIT](LICENSE)
