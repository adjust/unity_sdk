#!/usr/bin/env bash

set -e

# ======================================== #

# Colors for output
NC='\033[0m'
RED='\033[0;31m'
CYAN='\033[1;36m'
GREEN='\033[0;32m'

# ======================================== #

# Set directories of interest for the script
SDK_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
SDK_DIR="$(dirname "$SDK_DIR")"
SDK_DIR="$(dirname "$SDK_DIR")"
BUILD_DIR=sdk-dev/Adjust
JAR_IN_DIR=testlibrary/build/outputs
JAR_OUT_DIR=Assets/Adjust/Android
cd $(dirname $0)

# ======================================== #

echo -e "${CYAN}[ADJUST][ANDROID][BUILD-DEV-TEST]:${GREEN} Running Gradle tasks: clean makeJar ... ${NC}"
cd $BUILD_DIR
./gradlew clean :testlibrary:makeJar
echo -e "${CYAN}[ADJUST][ANDROID][BUILD-DEV-TEST]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][ANDROID][BUILD-DEV-TEST]:${GREEN} Moving test library JAR from ${JAR_IN_DIR} to ${JAR_OUT_DIR} ... ${NC}"
mv -v ${JAR_IN_DIR}/*.jar ${SDK_DIR}/${JAR_OUT_DIR}/adjust-testing.jar
echo -e "${CYAN}[ADJUST][ANDROID][BUILD-DEV-TEST]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][ANDROID][BUILD-DEV-TEST]:${GREEN} Script completed! ${NC}"

# ======================================== #