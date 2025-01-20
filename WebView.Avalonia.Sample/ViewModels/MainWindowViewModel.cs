using CommunityToolkit.Mvvm.Input;
using WebView.Core;

namespace WebView.Avalonia.Sample.ViewModels;

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
   public async void Alert() => await WebView?.ExecuteScriptAsync("alert(\"Hello! Avalonia!!\");");
}