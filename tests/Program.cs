using BD2Rhythm;
using BD2Rhythm.Compatibility;
using System.Text.Json;
int checks=0;void Check(bool value,string name){if(!value)throw new Exception(name);checks++;}
var charts=SyntheticCharts.All();
if(args.Length>2)charts.AddRange(JsonSerializer.Deserialize<List<Chart>>(File.ReadAllText(args[2]))??throw new Exception("Invalid optional local chart fixture"));
Check(charts.Select(c=>c.Fingerprint()).Distinct().Count()==charts.Count,"distinct chart fixtures");
foreach(var chart in charts)foreach(int jitter in new[]{0,50,200})for(int seed=1;seed<=30;seed++){
 var p=InputPlan.Build(chart,jitter,0,seed);Check(p.Events.Zip(p.Events.Skip(1)).All(x=>x.First.At<=x.Second.At),"ordered events");Check(p.Events.Where(e=>e.Kind==InputKind.Press).All(e=>Math.Abs(e.At-e.SourceTime)<=jitter),"bounded jitter");
 var held=new HashSet<(int,int)>();int sent=0;void Send(InputEvent e){var key=(e.Lane,e.Channel);if(e.Kind==InputKind.Press){Check(held.Add(key),"no double down");sent++;}else if(e.Kind==InputKind.Release)Check(held.Remove(key),"release own key");else Check(held.Contains(key),"slide follows press");}
 for(int t=-250;t<=p.Events.Last().At+30;t+=16)p.Advance(t,Send);
 Check(sent==p.Events.Count(e=>e.Kind==InputKind.Press)&&p.Skipped==0,"all notes at 60fps");Check(!p.HasHeld&&held.Count==0,"no held keys after end");
}
var sample=new Chart{Bpm=100,Notes=new(){new(){Id=1,Type=10,Lane=1,Time=100,Extra=new(){new(){Lane=1,Time=500}}},new(){Id=2,Type=20,Lane=2,Time=300},new(){Id=3,Type=1,Lane=2,Time=900}}};
var plan=InputPlan.Build(sample,0,0,1);var events=new List<InputEvent>();plan.Advance(100,events.Add);int cursor=plan.Cursor;plan.Advance(100,events.Add);Check(cursor==plan.Cursor,"stalled clock does not replay");plan.Release(events.Add);Check(!plan.HasHeld&&events.Last().Kind==InputKind.Release,"stop releases hold");
plan=InputPlan.Build(sample,0,0,1);plan.Advance(750,events.Add);Check(plan.Skipped==2&&!plan.HasHeld,"late work skipped without phantom release");plan.Advance(900,events.Add);Check(plan.Sent==1,"future note survives hitch");
long now=DateTime.UtcNow.Ticks;var c=new RhythmControl{Enabled=true,OwnerId="test",ProcessId=123,UntilUtcTicks=now+TimeSpan.FromSeconds(5).Ticks};Check(c.Valid(now,123)&&!c.Valid(now,124),"pid lease");Check(!c.Valid(now+TimeSpan.FromSeconds(6).Ticks,123),"expired lease");Check(new RhythmControl().JitterMs==50,"default 50ms");
var failure=new RhythmConnectionState{Phase="injecting",LastError="FileNotFoundException: 0Harmony"};Check(RhythmConnection.ExistingConnectionFailure(failure).Contains(failure.LastError),"original injection error survives reconnect");
var output=args.Length>0?Path.GetFullPath(args[0]):Path.Combine(AppContext.BaseDirectory,"checks");
Directory.CreateDirectory(output);
if(args.Length>1){BootstrapChecks.Prepare(args[1],Path.Combine(output,"bootstrap"));Check(true,"bootstrap dependency isolation");var prepared=HookCompiler.Prepare(args[1]);Check(prepared.Payload.Length>0&&prepared.Report.Status=="compatible","installed client compile");File.WriteAllText(Path.Combine(output,"client.json"),JsonSerializer.Serialize(prepared.Report));}
var en=new BD2Rhythm.Localization.LanguageCatalog("en-US");
var english=BD2Rhythm.Localization.LanguageCatalog.Read("en-US");var chinese=BD2Rhythm.Localization.LanguageCatalog.Read("zh-CN");
Check(english.Keys.Order().SequenceEqual(chinese.Keys.Order()),"catalog key parity");
Check(english.Values.All(v=>!string.IsNullOrWhiteSpace(v)),"no empty translations");
Check(en.Text("已连接游戏 123 · 音游独立组件")=="Connected to game 123 · Standalone rhythm component","dynamic connection translation");
foreach(var chart in SyntheticCharts.Invalid()){
 bool rejected=false;try{InputPlan.Build(chart,50,0,1);}catch(ArgumentException){rejected=true;}
 Check(rejected,"reject malformed/unsupported chart before input");
}
foreach(int offset in new[]{-500,500}){var shifted=InputPlan.Build(sample,50,offset,123);Check(shifted.Events.Where(e=>e.Kind==InputKind.Press).All(e=>Math.Abs(e.At-e.SourceTime-offset)<=50),"calibration bounds");}
Check(!RhythmControlLink.Fresh(new(){ProcessId=123,Runtime="BD2Rhythm.Runtime2",CapturedUtcTicks=now},DateTime.UtcNow),"reject old runtime snapshot");
TestTransport.Start(Path.Combine(output,"control-tests"),RhythmIdentity.LiveEntries);using(var link=new RhythmControlLink(Path.Combine(output,"control-tests"))){link.Configure(new(){JitterMs=50});link.Start(123);var before=link.OwnerId;BD2Rhythm.Localization.LanguagePreference.Save(Path.Combine(output,"control-tests"),"en-US");Check(link.Enabled&&link.OwnerId==before,"language preference leaves lease untouched");link.Stop();}
File.WriteAllText(Path.Combine(output,"tests.json"),JsonSerializer.Serialize(new{status="pass",checks,chartFixtures=charts.Count}));Console.WriteLine($"Passed {checks} checks across {charts.Count} chart fixtures.");
