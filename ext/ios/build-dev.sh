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
SRCDIR=./sdk-dev
LIBOUTDIR=../../Assets/Adjust/iOS
SDK_STATIC_FRAMEWORK=$SRCDIR/Frameworks/Static/AdjustSdk.framework
TEST_STATIC_FRAMEWORK=$SRCDIR/Frameworks/Static/AdjustTestLibrary.framework

# ======================================== #

echo -e "${CYAN}[ADJUST][IOS][BUILD-DEV]:${GREEN} Building AdjustStatic framework target ... ${NC}"
(cd $SRCDIR; xcodebuild -target AdjustStatic -configuration Release clean build)
cp -v $SDK_STATIC_FRAMEWORK/Versions/A/AdjustSdk $LIBOUTDIR/AdjustSdk.a
cp -v $SDK_STATIC_FRAMEWORK/Versions/A/Headers/* $LIBOUTDIR
echo -e "${CYAN}[ADJUST][IOS][BUILD-DEV]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][IOS][BUILD-DEV]:${GREEN} Building AdjustTestLibraryStatic framework target ... ${NC}"
(cd $SRCDIR/AdjustTests/AdjustTestLibrary; xcodebuild -target AdjustTestLibraryStatic -configuration Release clean build)
cp -v $TEST_STATIC_FRAMEWORK/Versions/A/AdjustTestLibrary $LIBOUTDIR/AdjustTestLibrary.a
cp -v $TEST_STATIC_FRAMEWORK/Versions/A/Headers/* $LIBOUTDIR
echo -e "${CYAN}[ADJUST][IOS][BUILD-DEV]:${GREEN} Done! ${NC}"

# ======================================== #

echo -e "${CYAN}[ADJUST][IOS][BUILD-DEV]:${GREEN} Script completed! ${NC}"

# ======================================== #