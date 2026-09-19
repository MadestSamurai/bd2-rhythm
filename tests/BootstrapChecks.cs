using System.Text;
using BD2Rhythm.Compatibility;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Mono.Cecil;
public static class BootstrapChecks {
 public static void Prepare(string managed,string output){
  Directory.CreateDirectory(output);
  var refs=new[]{"mscorlib.dll","System.dll","System.Core.dll"}.Select(f=>MetadataReference.CreateFromFile(Path.Combine(managed,f))).ToList();
  byte[] Compile(string name,string source,IEnumerable<MetadataReference> references,OutputKind kind=OutputKind.DynamicallyLinkedLibrary,ResourceDescription[]? resources=null){using var b=new MemoryStream();var result=CSharpCompilation.Create(name,new[]{CSharpSyntaxTree.ParseText(source)},references,new CSharpCompilationOptions(kind,optimizationLevel:OptimizationLevel.Release)).Emit(b,manifestResources:resources);if(!result.Success)throw new Exception(string.Join("\n",result.Diagnostics));return b.ToArray();}
  var harmony=Compile("0Harmony","namespace HarmonyLib { public class Harmony { public Harmony(string name){} } }",refs);
  var all=refs.Append(MetadataReference.CreateFromImage(harmony));
  var stub="""
namespace BD2Rhythm.Runtime {
 public class RhythmRuntimeStatus {public string State;public string Error;public string AtUtc;public int ProcessId;}
 internal static class LocalStorage {
  internal static string DataRoot {get{return System.Environment.GetEnvironmentVariable("RHYTHM_PROBE_OUTPUT");}}
  internal static void Log(string message){System.IO.File.AppendAllText(System.IO.Path.Combine(DataRoot,"probe.log"),message+"\n");}
  internal static void WriteJsonAtomically(string path,object obj){var s=(RhythmRuntimeStatus)obj;System.IO.File.WriteAllText(path,s.State+"|"+s.Error);}
 }
 internal class RuntimeEngine {
  HarmonyLib.Harmony harmony;
  internal void Start(){harmony=new HarmonyLib.Harmony("probe");Loader.Status("active","");}
  internal void Stop(){LocalStorage.Log("cleanup");}
 }
}
""";
  var loader=Encoding.UTF8.GetString(HookCompiler.Resource("Hook.Loader.cs"));
  var resources=new[]{new ResourceDescription("BD2Rhythm.Harmony.dll",()=>new MemoryStream(harmony),true)};
  var fixedPayload=Compile("BD2Rhythm.Runtime3",loader+stub,all,resources:resources);
  using(var asm=AssemblyDefinition.ReadAssembly(new MemoryStream(fixedPayload))){
   var type=asm.MainModule.GetType("BD2Rhythm.Runtime.Loader");
   if(type.Fields.Any(f=>f.FieldType.FullName.Contains("RuntimeEngine")||f.FieldType.FullName.Contains("Harmony")))throw new Exception("Loader eagerly references engine field");
   foreach(var method in type.Methods.Where(m=>m.HasBody))foreach(var instruction in method.Body.Instructions){
    if(instruction.Operand is MemberReference m && (m.DeclaringType?.FullName.Contains("RuntimeEngine")==true || m.DeclaringType?.FullName.Contains("HarmonyLib")==true))throw new Exception("Bootstrap has eager runtime dependency");
   }
  }
  File.WriteAllBytes(Path.Combine(output,"fixed.dll"),fixedPayload);
  var realHarmony=HookCompiler.Resource("BD2Rhythm.Harmony.dll");
  File.WriteAllBytes(Path.Combine(output,"real-harmony.dll"),Compile("BD2Rhythm.Runtime3",loader+stub,refs.Append(MetadataReference.CreateFromImage(realHarmony)),resources:new[]{new ResourceDescription("BD2Rhythm.Harmony.dll",()=>new MemoryStream(realHarmony),true)}));
  // Reproduce the original typed-field/direct-construction bootstrap using the same resolver.
  var old=loader.Replace("static object engine;","static RuntimeEngine engine;").Replace("candidate=Activator.CreateInstance(own.GetType(\"BD2Rhythm.Runtime.RuntimeEngine\",true),true);","candidate=new RuntimeEngine();").Replace("engine=candidate;","engine=(RuntimeEngine)candidate;");
  File.WriteAllBytes(Path.Combine(output,"old.dll"),Compile("BD2Rhythm.Runtime1",old+stub,all,resources:resources));
  var runner="""
using System;
using System.IO;
using System.Reflection;
class Runner {static int Main(string[] args){try {Assembly.Load(File.ReadAllBytes(args[0])).GetType("BD2Rhythm.Runtime.Loader",true).GetMethod("Load").Invoke(null,null);return 0;}catch(Exception e){File.WriteAllText(Path.Combine(Environment.GetEnvironmentVariable("RHYTHM_PROBE_OUTPUT"),"outer-error.txt"),e.ToString());return 1;}}}
""";
  File.WriteAllBytes(Path.Combine(output,"Probe.exe"),Compile("Probe",runner,refs,OutputKind.ConsoleApplication));
 }
}
