#!/usr/bin/env bash

set -e

MVNDIR=./sdk/Adjust
JARINDIR=./sdk/Adjust/target
JAROUTDIR=../../Assets/Adjust/Android

(cd $MVNDIR; mvn clean)
(cd $MVNDIR; mvn package)

rm -v $JAROUTDIR/adjust-android*
cp -v $JARINDIR/adjust-android-*.*.*.jar $JAROUTDIR
rm -v $JAROUTDIR/*-javadoc.jar
rm -v $JAROUTDIR/*-sources.jar
mv -v $JAROUTDIR/adjust-android-*.*.*.jar $JAROUTDIR/adjust-android.jar
