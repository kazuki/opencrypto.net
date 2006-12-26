TARGET=openCrypto.exe
RELEASE_DIR=bin/Release
RELEASE_TARGET=${RELEASE_DIR}/${TARGET}
DEBUG_DIR=bin/Debug
DEBUG_TARGET=${DEBUG_DIR}/${TARGET}
TEST_DIR=bin/Tests
TEST_TARGET=${TEST_DIR}/${TARGET}
SOURCE_FILES:=$(foreach f,$(shell cat SourceFiles),$(f))
COMPILER=gmcs
FLAGS=-codepage:utf8 -unsafe+

all: release
release: ${RELEASE_TARGET}
debug: ${DEBUG_TARGET}
tests: ${TEST_TARGET}
test: tests
run-test: tests
	nunit-console2 ${TEST_TARGET}

clean:
	rm -f ${RELEASE_TARGET} ${DEBUG_TARGET} ${TEST_TARGET}

${RELEASE_TARGET}: ${SOURCE_FILES}
	${COMPILER} ${FLAGS} -optimize+ -out:${RELEASE_TARGET} ${SOURCE_FILES}

${DEBUG_TARGET}: ${SOURCE_FILES}
	${COMPILER} ${FLAGS} -out:${DEBUG_TARGET} ${SOURCE_FILES}

${TEST_TARGET}: ${SOURCE_FILES}
	${COMPILER} ${FLAGS} -define:TEST -r:nunit.framework.dll \
	-out:${TEST_TARGET} ${SOURCE_FILES}                      \
	-resource:Tests/t_camellia.txt                           \
	-resource:Tests/ecb_tbl.txt                              \
	-resource:Tests/ecb_vt.txt                               \
	-resource:Tests/ecb_e_m.txt                              \
	-resource:Tests/ecb_vk.txt
