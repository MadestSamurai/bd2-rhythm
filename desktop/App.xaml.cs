using System.IO;
using System.Text.Json;
using System.Windows;
namespace BD2Rhythm.Desktop;
public partial class App:Application {
 Mutex? single;
 protected override async void OnStartup(StartupEventArgs e){base.OnStartup(e);
  if(e.Args.Length==2&&e.Args[0]=="--identity"){File.WriteAllText(e.Args[1],JsonSerializer.Serialize(new{version="0.2.1",runtime=RhythmIdentity.RuntimeName,defaultJitterMs=50,chartSource="live-game",compatibility="local-interface-adaptation",toolFingerprint=Compatibility.HookCompiler.ToolFingerprint,languages=new[]{"zh-CN","en-US"}}));Shutdown();return;}
  if(e.Args.Length==3&&e.Args[0]=="--check-client"){try{var p=await Task.Run(()=>Compatibility.HookCompiler.Prepare(e.Args[1]));File.WriteAllText(e.Args[2],JsonSerializer.Serialize(p.Report));Shutdown();}catch(Exception ex){File.WriteAllText(e.Args[2],ex.ToString());Shutdown(1);}return;}
  if(e.Args.Length==2&&e.Args[0]=="--smoke"){try{Directory.CreateDirectory(e.Args[1]);var w=new RhythmWindow(Path.Combine(Path.GetFullPath(e.Args[1]),"isolated"));MainWindow=w;await w.SmokeAsync(e.Args[1]);Shutdown();}catch(Exception ex){File.WriteAllText(Path.Combine(e.Args[1],"failure.txt"),ex.ToString());Shutdown(1);}return;}
  single=new Mutex(true,"Local\\BD2Rhythm.Private.Desktop",out bool first);if(!first){MessageBox.Show(new Localization.LanguageCatalog(Localization.LanguagePreference.Read(RhythmIdentity.DataRoot)).Text("音游工具已经打开，请使用现有窗口。"),"BD2 Rhythm");Shutdown();return;}MainWindow=new RhythmWindow();MainWindow.Show();
 }
 protected override void OnExit(ExitEventArgs e){single?.Dispose();base.OnExit(e);}
}
