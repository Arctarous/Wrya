using CommunityToolkit.Mvvm.Input;
using Wryava.Core;

namespace Wryava.Avalonia.Sample.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
   public string Greeting { get; } = "Welcome to Avalonia!";
   public IWebView? WebView { get; set; }
   
   [RelayCommand]
   public void GoBack() => WebView?.GoBack();
   
   [RelayCommand]
   public void GoForward() => WebView?.GoForward();
   
   [RelayCommand]
   public void GoBaidu() => WebView?.Navigate(new Uri("https://www.baidu.com"));
   
   [RelayCommand]
   public async Task Alert() => await WebView!.ExecuteScriptAsync("alert(\"Hello! Avalonia!!\");");
}