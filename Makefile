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

clean:
	rm -f ${RELEASE_TARGET} ${DEBUG_TARGET} ${TEST_TARGET}

${RELEASE_TARGET}: FORCE ${RELEASE_DIR}
	${COMPILER} ${FLAGS} -optimize+ -out:${RELEASE_TARGET} ${SOURCE_FILES}

${DEBUG_TARGET}: FORCE ${DEBUG_DIR}
	${COMPILER} ${FLAGS} -out:${DEBUG_TARGET} ${SOURCE_FILES}

${TEST_TARGET}: FORCE ${TEST_DIR}
	${COMPILER} ${FLAGS} -define:TEST -r:nunit.framework.dll -out:${TEST_TARGET} ${SOURCE_FILES}

${RELEASE_DIR}: ./bin
	mkdir ${RELEASE_DIR}

${DEBUG_DIR}: ./bin
	mkdir ${DEBUG_DIR}

${TEST_DIR}: ./bin
	mkdir ${TEST_DIR}

./bin:
	mkdir bin

FORCE:
