DLL := openCrypto.NET.dll
DLL_SRC := ${shell find openCrypto.NET -name *.cs}
EXE := Executable.exe
EXE_SRC := ${shell find Executable -name *.cs}
TEST := openCrypto.NET.Tests.dll
TEST_SRC = ${DLL_SRC}

all: ${DLL} ${EXE}
clean:
	rm -f ${DLL} ${EXE} ${TEST}

${DLL}: ${DLL_SRC}
	gmcs -out:$@ -target:library -optimize+ -unsafe+ ${DLL_SRC}
	
${EXE}: ${DLL}
	gmcs -out:$@ -optimize+ -unsafe+ ${EXE_SRC} -r:${DLL}

test: ${TEST}
${TEST}: ${TEST_SRC}
	gmcs -out:$@ -target:library -unsafe+ -d:TEST ${TEST_SRC} -r:nunit.framework.dll
