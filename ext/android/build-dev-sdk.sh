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
MVNDIR=./sdk-dev/Adjust
JARINDIR=./sdk-dev/Adjust/target
JAROUTDIR=../../Assets/Adjust/Android
(cd $MVNDIR; mvn clean)
(cd $MVNDIR; mvn package)

# ======================================== #

echo -e "${CYAN}[ADJUST][ANDROID][BUILD-DEV-SDK]:${GREEN} Previous SDK JAR cleanup and generating new one ... ${NC}"
rm -v $JAROUTDIR/adjust-android*
cp -v $JARINDIR/adjust-android-*.*.*.jar $JAROUTDIR
rm -v $JAROUTDIR/*-javadoc.jar
rm -v $JAROUTDIR/*-sources.jar
mv -v $JAROUTDIR/adjust-android-*.*.*.jar $JAROUTDIR/adjust-android.jar
echo -e "${CYAN}[ADJUST][ANDROID][BUILD-DEV-SDK]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][ANDROID][BUILD-DEV-SDK]:${GREEN} Script completed! ${NC}"

# ======================================== #