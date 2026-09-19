using BD2Rhythm;
public static class SyntheticCharts {
 public static List<Chart> All(){
  var charts=new List<Chart>();
  for(int variant=0;variant<10;variant++){
   var chart=new Chart{Name=$"Synthetic-{variant}",Difficulty=variant%2,Bpm=120+variant};
   for(int i=0;i<80;i++){
    int lane=1+i%2,time=1000+i*(220+variant*5),type=new[]{1,10,20,30}[i%4];
    var note=new Note{Id=i,Type=type,Lane=lane,Time=time};
    if(type==10)note.Extra.Add(new(){Lane=lane,Time=time+200});
    if(type==30)note.Extra.Add(new(){Lane=lane,Time=time+80});
    chart.Notes.Add(note);
   }
   charts.Add(chart);
  }
  return charts;
 }
 public static IEnumerable<Chart> Invalid(){
  yield return null!;
  yield return new();
  yield return new(){Notes=new(){null!}};
  foreach(int type in new[]{0,2,99})yield return new(){Notes=new(){new(){Id=1,Type=type,Lane=1,Time=100}}};
  yield return new(){Notes=new(){new(){Id=1,Type=1,Lane=0,Time=100}}};
  yield return new(){Notes=new(){new(){Id=1,Type=1,Lane=1,Time=-1}}};
  yield return new(){Notes=new(){new(){Id=1,Type=10,Lane=1,Time=100}}};
  yield return new(){Notes=new(){new(){Id=1,Type=10,Lane=1,Time=100,Extra=new(){new(){Lane=2,Time=200}}}}};
  yield return new(){Notes=new(){new(){Id=1,Type=1,Lane=1,Time=100},new(){Id=1,Type=1,Lane=2,Time=200}}};
  yield return new(){Notes=new(){new(){Id=1,Type=1,Lane=1,Time=100},new(){Id=2,Type=1,Lane=1,Time=100}}};
 }
}
