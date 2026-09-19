using System;
using System.IO;
using System.Linq;
using System.Reflection;
namespace BD2Rhythm.Runtime {
 public static class Loader {
  // Keep this bootstrap independent of RuntimeEngine/Harmony at JIT time.
  static bool resolver;static object engine;
  public static void Load(){lock(typeof(Loader)){
   object candidate=null;
   try {
    if(!resolver){AppDomain.CurrentDomain.AssemblyResolve+=Resolve;resolver=true;}
    LocalStorage.Log("bootstrap_enter");
    if(engine!=null)return;
    var own=typeof(Loader).Assembly;
    if(AppDomain.CurrentDomain.GetAssemblies().Any(a=>a!=own&&a.GetName().Name.StartsWith("BD2Rhythm.Runtime")))throw new InvalidOperationException("已有另一版音游组件，请重启游戏后连接。");
    Status("starting","");
    candidate=Activator.CreateInstance(own.GetType("BD2Rhythm.Runtime.RuntimeEngine",true),true);
    candidate.GetType().GetMethod("Start",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(candidate,null);
    engine=candidate;
   }catch(Exception e){
    if(candidate!=null)try{candidate.GetType().GetMethod("Stop",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(candidate,null);}catch{}
    LocalStorage.Log("bootstrap_error "+e);
    Status("error",e.GetBaseException().Message);
   }
  }}
  static Assembly Resolve(object sender,ResolveEventArgs e){
   if(new AssemblyName(e.Name).Name!="0Harmony")return null;
   LocalStorage.Log("resolve_harmony");
   using(var stream=typeof(Loader).Assembly.GetManifestResourceStream("BD2Rhythm.Harmony.dll"))using(var buffer=new MemoryStream()){
    if(stream==null)throw new InvalidOperationException("内置音游依赖缺失：0Harmony。");
    stream.CopyTo(buffer);return Assembly.Load(buffer.ToArray());
   }
  }
  internal static void Status(string state,string error){try{LocalStorage.WriteJsonAtomically(Path.Combine(LocalStorage.DataRoot,"runtime.json"),new RhythmRuntimeStatus{State=state,Error=error,AtUtc=DateTime.UtcNow.ToString("O"),ProcessId=System.Diagnostics.Process.GetCurrentProcess().Id});}catch(Exception e){LocalStorage.Log("status_error "+e);}}
 }
}
