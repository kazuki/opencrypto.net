RELEASE_TARGET=openCrypto.NET.dll
TEST_TARGET=openCrypto.NET.Tests.dll
EXEC_TARGET=Executable.exe
SOURCE_FILES:=${shell find ./openCrypto.NET -name "*.cs"}
EXEC_FILES:=${shell find ./Executable -name "*.cs"}
TESTS_FILES=${shell find ./UnitTests -name "*.cs"}
COMPILER=gmcs

all: release
release: ${RELEASE_TARGET}
tests: ${TEST_TARGET}
exec: ${EXEC_TARGET}
run: exec
	mono ${EXEC_TARGET}
test: tests
run-test: tests
	nunit-console2 ${TEST_TARGET}

clean:
	rm -f ${RELEASE_TARGET} ${TEST_TARGET} ${EXEC_TARGET}

${RELEASE_TARGET}: ${SOURCE_FILES}
	${COMPILER} -codepage:utf8 -unsafe+ -target:library -optimize+ -out:${RELEASE_TARGET} ${SOURCE_FILES}

${EXEC_TARGET}: ${EXEC_FILES} ${RELEASE_TARGET}
	${COMPILER} -unsafe+ -codepage:utf8 -optimize+ -out:${EXEC_TARGET} -r:${RELEASE_TARGET} ${EXEC_FILES}

${TEST_TARGET}: ${SOURCE_FILES} ${TESTS_FILES}
	${COMPILER} -codepage:utf8 -unsafe+ -target:library      \
        -define:TEST -define:MONO -r:nunit.framework.dll         \
	-out:${TEST_TARGET} ${SOURCE_FILES} ${TESTS_FILES}       \
	-resource:UnitTests/t_camellia.txt                       \
	-resource:UnitTests/ecb_tbl.txt                          \
	-resource:UnitTests/ecb_vt.txt                           \
	-resource:UnitTests/ecb_e_m.txt                          \
	-resource:UnitTests/ecb_vk.txt
