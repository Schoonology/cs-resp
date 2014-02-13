CSC_OPTS=/nologo
VSTEST_OPTS=
NET20_LIB=/target:library /noconfig /nostdlib /reference:C:\Windows\Microsoft.NET\Framework\v2.0.50727\mscorlib.dll
TEST_LIB=/target:library /reference:"%VSINSTALLDIR%\Common7\IDE\PublicAssemblies\Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll"

default: lib\Resp.dll

test: lib\Resp.Tests.dll

pack: obj test
	nuget pack -OutputDirectory obj -ExcludeEmptyDirectories

clean:
	- rmdir /Q /S lib
	- rmdir /Q /S obj

lib\Resp.dll: lib
	csc $(CSC_OPTS) $(NET20_LIB) /out:lib\Resp.dll src\*.cs

lib\Resp.Tests.dll: lib\Resp.dll
	csc $(CSC_OPTS) $(TEST_LIB) /reference:lib\Resp.dll /out:lib\Resp.Tests.dll test\*.cs
	vstest.console $(VSTEST_OPTS) lib\Resp.Tests.dll

lib:
	mkdir lib

obj:
	mkdir obj
