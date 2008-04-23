RELEASE_TARGET=openCrypto.NET.dll
TEST_TARGET=openCrypto.NET.Tests.dll
EXEC_TARGET=Executable.exe
SOURCE_FILES:=${shell find . -name "*.cs"}
EXEC_FILES:=${shell find ../Executable -name "*.cs"}
COMPILER=gmcs
FLAGS=-codepage:utf8 -unsafe+ -target:library

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
	${COMPILER} ${FLAGS} -optimize+ -out:${RELEASE_TARGET} ${SOURCE_FILES}

${EXEC_TARGET}: ${EXEC_FILES} ${RELEASE_TARGET}
	${COMPILER} -unsafe+ -codepage:utf8 -optimize+ -out:${EXEC_TARGET} -r:${RELEASE_TARGET} ${EXEC_FILES}

${TEST_TARGET}: ${SOURCE_FILES}
	${COMPILER} ${FLAGS} -define:TEST -r:nunit.framework.dll \
	-out:${TEST_TARGET} ${SOURCE_FILES}                      \
	-resource:Tests/t_camellia.txt                           \
	-resource:Tests/ecb_tbl.txt                              \
	-resource:Tests/ecb_vt.txt                               \
	-resource:Tests/ecb_e_m.txt                              \
	-resource:Tests/ecb_vk.txt
