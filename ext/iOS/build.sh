#!/usr/bin/env bash

set -e

SRCDIR=./sdk
LIBOUTDIR=../../Assets/Adjust/iOS
STATICFRAMEWORK=$SRCDIR/Frameworks/Static/AdjustSdk.framework

(cd $SRCDIR; xcodebuild -target AdjustStatic -configuration Release clean build)
cp -v $STATICFRAMEWORK/Versions/A/AdjustSdk $LIBOUTDIR/AdjustSdk.a
cp -v $STATICFRAMEWORK/Versions/A/Headers/* $LIBOUTDIR

