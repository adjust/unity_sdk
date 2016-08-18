#!/usr/bin/env bash

###############################################################################
# When copying from Backup folder instead of using files from Unity 5 assets, #
# it is because files used for Unity 5 are not compatible with Unity 4.       #
###############################################################################

########################################
# End script if one of the lines fails #
######################################## 
set -e

#####################################
# Clear any existing Unity 4 assets #
#####################################
rm -rf ./Unity4Assets

#####################################
# Recreate Unity 4 folder structure #
#####################################
mkdir -p ./Unity4Assets

mkdir -p ./Unity4Assets/Adjust
mkdir -p ./Unity4Assets/Editor
mkdir -p ./Unity4Assets/Plugins

mkdir -p "./Unity4Assets/Adjust/3rd Party"
mkdir -p ./Unity4Assets/Adjust/Android
mkdir -p ./Unity4Assets/Adjust/ExampleGUI
mkdir -p ./Unity4Assets/Adjust/iOS
mkdir -p ./Unity4Assets/Adjust/Metro
mkdir -p ./Unity4Assets/Adjust/Unity
mkdir -p ./Unity4Assets/Adjust/WP8

mkdir -p ./Unity4Assets/Plugins/Android
mkdir -p ./Unity4Assets/Plugins/iOS
mkdir -p ./Unity4Assets/Plugins/Metro
mkdir -p ./Unity4Assets/Plugins/WP8

#################################
# Copy files to Unity 4 folders #
#################################

#################
# Adjust folder #
#################
cp ../Assets/Adjust/Adjust.cs ./Unity4Assets/Adjust/
cp ./Unity4Backup/Adjust.prefab ./Unity4Assets/Adjust/

####################
# 3rd Party folder #
####################
cp ../Assets/Adjust/3rd\ Party/SimpleJSON.cs ./Unity4Assets/Adjust/3rd\ Party/

#########################
# Adjust/Android folder #
#########################
cp ../Assets/Adjust/Android/AdjustAndroid.cs ./Unity4Assets/Adjust/Android/
cp ./Unity4Backup/AdjustAndroidManifest.xml ./Unity4Assets/Adjust/Android/

############################
# Adjust/ExampleGUI folder #
############################
cp ../Assets/Adjust/ExampleGUI/ExampleGUI.cs ./Unity4Assets/Adjust/ExampleGUI/
cp ./Unity4Backup/ExampleGUI.prefab ./Unity4Assets/Adjust/ExampleGUI/
cp ./Unity4Backup/ExampleGUI.unity ./Unity4Assets/Adjust/ExampleGUI/

#####################
# Adjust/iOS folder #
#####################
cp ../Assets/Adjust/iOS/AdjustiOS.cs ./Unity4Assets/Adjust/iOS/

#######################
# Adjust/Metro folder #
#######################
cp ../Assets/Adjust/Metro/AdjustMetro.cs ./Unity4Assets/Adjust/Metro/

#######################
# Adjust/Unity folder #
#######################
cp ../Assets/Adjust/Unity/* ./Unity4Assets/Adjust/Unity/

#####################
# Adjust/WP8 folder #
#####################
cp ../Assets/Adjust/WP8/AdjustWP8.cs ./Unity4Assets/Adjust/WP8/

#################
# Editor folder #
#################
cp ./Unity4Backup/AdjustEditor.cs ./Unity4Assets/Editor/
cp ./Unity4Backup/mod_pbxproj.py ./Unity4Assets/Editor/
cp ./Unity4Backup/PostprocessBuildPlayer_AdjustPostBuildAndroid.py ./Unity4Assets/Editor/
cp ./Unity4Backup/PostprocessBuildPlayer_AdjustPostBuildiOS.py ./Unity4Assets/Editor

##################
# Plugins folder #
##################
cp ../Assets/Adjust/AdjustUnityWP.dll ./Unity4Assets/Plugins/
cp ../Assets/Adjust/AdjustUnityWS.dll ./Unity4Assets/Plugins/

##########################
# Plugins/Android folder #
##########################
cp ../Assets/Adjust/Android/adjust-android.jar ./Unity4Assets/Plugins/Android/

######################
# Plugins/iOS folder #
######################
cp ../Assets/Adjust/iOS/*.h ./Unity4Assets/Plugins/iOS/
cp ../Assets/Adjust/iOS/*.mm ./Unity4Assets/Plugins/iOS/
cp ../Assets/Adjust/iOS/*.a ./Unity4Assets/Plugins/iOS/

########################
# Plugins/Metro folder #
########################
cp ../Assets/Adjust/Metro/*.dll ./Unity4Assets/Plugins/Metro/

######################
# Plugins/WP8 folder #
######################
cp ../Assets/Adjust/WP8/*.dll ./Unity4Assets/Plugins/WP8/

#####################################################################################
# Target for iOS is named "BuildTarget.iOS" in Unity 5                              #
# This is not compatible with Unity 4 where it should be named "BuildTarget.iPhone" #
# Changing BuildTarget.iOS -> BuildTarget.iPhone in AdjustEditor.cs for Unity 4     #
#####################################################################################
sed -e "s/BuildTarget.iOS/BuildTarget.iPhone/" ./Unity4Assets/Editor/AdjustEditor.cs > ./Unity4Assets/Editor/AdjustEditor.cs.temp
mv ./Unity4Assets/Editor/AdjustEditor.cs.temp ./Unity4Assets/Editor/AdjustEditor.cs
