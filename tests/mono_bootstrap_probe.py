import ctypes as c, os, sys
from pathlib import Path
base=Path(sys.argv[3]).resolve()
embed=base/'MonoBleedingEdge/EmbedRuntime'
out=Path(sys.argv[1]).resolve();out.mkdir(parents=True,exist_ok=True)
os.environ['RHYTHM_PROBE_OUTPUT']=str(out)
handle=os.add_dll_directory(str(embed))
m=c.CDLL(str(embed/'mono-2.0-bdwgc.dll'))
def call(name,rest,args):
 f=getattr(m,name);f.restype=rest;f.argtypes=args;return f
call('mono_set_dirs',None,[c.c_char_p,c.c_char_p])(str(base/'BrownDust II_Data/Managed').encode(),str(base/'MonoBleedingEdge/etc').encode())
call('mono_set_assemblies_path',None,[c.c_char_p])(str(base/'BrownDust II_Data/Managed').encode())
call('mono_config_parse',None,[c.c_char_p])(None)
domain=call('mono_jit_init_version',c.c_void_p,[c.c_char_p,c.c_char_p])(b'rhythm-probe',b'v4.0.30319')
assert domain, 'Mono domain failed'
runner=str(Path(sys.argv[4]).resolve()).encode()
assembly=call('mono_domain_assembly_open',c.c_void_p,[c.c_void_p,c.c_char_p])(domain,runner)
assert assembly, 'Runner assembly failed'
args=(c.c_char_p*2)(runner,str(Path(sys.argv[2]).resolve()).encode())
result=call('mono_jit_exec',c.c_int,[c.c_void_p,c.c_void_p,c.c_int,c.POINTER(c.c_char_p)])(domain,assembly,2,args)
print('Mono bootstrap exit:', result, flush=True)
# Let the process exit; no game is opened or controlled.
os._exit(result)
