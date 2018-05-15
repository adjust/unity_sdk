#!/usr/bin/env bash

set -e

SRCDIR=./sdk
LIBOUTDIR=../../Assets/Adjust/iOS

SDK_STATIC_FRAMEWORK=$SRCDIR/Frameworks/Static/AdjustSdk.framework
TEST_STATIC_FRAMEWORK=$SRCDIR/Frameworks/Static/AdjustTestLibrary.framework

(cd $SRCDIR; xcodebuild -target AdjustStatic -configuration Release clean build)
cp -v $SDK_STATIC_FRAMEWORK/Versions/A/AdjustSdk $LIBOUTDIR/AdjustSdk.a
cp -v $SDK_STATIC_FRAMEWORK/Versions/A/Headers/* $LIBOUTDIR

(cd $SRCDIR/AdjustTests/AdjustTestLibrary; xcodebuild -target AdjustTestLibraryStatic -configuration Release clean build)
cp -v $TEST_STATIC_FRAMEWORK/Versions/A/AdjustTestLibrary $LIBOUTDIR/AdjustTestLibrary.a
cp -v $TEST_STATIC_FRAMEWORK/Versions/A/Headers/* $LIBOUTDIR