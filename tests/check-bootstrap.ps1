param([Parameter(Mandatory=$true)][string]$GameDirectory,[string]$Python='python')
$ErrorActionPreference='Stop'
$root=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$outRoot=Join-Path $root '.build/checks'
$managed=Join-Path $GameDirectory 'BrownDust II_Data/Managed'
& dotnet run --project (Join-Path $PSScriptRoot 'BD2Rhythm.Tests.csproj') -c Release -- $outRoot $managed
if($LASTEXITCODE -ne 0){throw 'Scheduling or client compile checks failed'}
$bootstrap=Join-Path $outRoot 'bootstrap'
$results=@()
foreach($mode in @('old','fixed','real-harmony')){
 $out=Join-Path $bootstrap $mode
 & $Python (Join-Path $PSScriptRoot 'mono_bootstrap_probe.py') $out (Join-Path $bootstrap "$mode.dll") $GameDirectory (Join-Path $bootstrap 'Probe.exe')
 $code=$LASTEXITCODE
 if($mode -eq 'old'){
  if($code -ne 1 -or !(Get-Content (Join-Path $out 'outer-error.txt') -Raw).Contains('TypeLoadException')){throw 'Original bootstrap failure was not reproduced'}
 }else{
  if($code -ne 0 -or (Get-Content (Join-Path $out 'runtime.json') -Raw) -ne 'active|'){throw "Fixed bootstrap failed: $mode"}
 }
 $results+=[ordered]@{case=$mode;exitCode=$code;expected=$true}
}
$results | ConvertTo-Json | Set-Content -Encoding UTF8 (Join-Path $bootstrap 'results.json')
Write-Host 'Original TypeLoadException reproduced; isolated and real Harmony bootstraps passed.'
