TARGET=openCrypto.exe
RELEASE_DIR=bin/Release
RELEASE_TARGET=${RELEASE_DIR}/${TARGET}
DEBUG_DIR=bin/Debug
DEBUG_TARGET=${DEBUG_DIR}/${TARGET}
TEST_DIR=bin/Tests
TEST_TARGET=${TEST_DIR}/${TARGET}
SOURCE_FILES=*.cs Camellia/*.cs Tests/*.cs
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

${RELEASE_TARGET}: FORCE
	${COMPILER} ${FLAGS} -optimize+ -out:${RELEASE_TARGET} ${SOURCE_FILES}

${DEBUG_TARGET}: FORCE
	${COMPILER} ${FLAGS} -out:${DEBUG_TARGET} ${SOURCE_FILES}

${TEST_TARGET}: FORCE
	${COMPILER} ${FLAGS} -define:TEST -r:nunit.framework.dll \
	-out:${TEST_TARGET} ${SOURCE_FILES} \
	-resource:Tests/t_camellia.txt


FORCE:

