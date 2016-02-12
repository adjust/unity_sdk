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
rm -rf ./Assets

#####################################
# Recreate Unity 4 folder structure #
#####################################
mkdir -p ./Assets

mkdir -p ./Assets/Adjust
mkdir -p ./Assets/Editor
mkdir -p ./Assets/Plugins

mkdir -p "./Assets/Adjust/3rd Party"
mkdir -p ./Assets/Adjust/Android
mkdir -p ./Assets/Adjust/ExampleGUI
mkdir -p ./Assets/Adjust/iOS
mkdir -p ./Assets/Adjust/Metro
mkdir -p ./Assets/Adjust/Unity
mkdir -p ./Assets/Adjust/WP8

mkdir -p ./Assets/Plugins/Android
mkdir -p ./Assets/Plugins/iOS
mkdir -p ./Assets/Plugins/Metro
mkdir -p ./Assets/Plugins/WP8

#################################
# Copy files to Unity 4 folders #
#################################

#################
# Adjust folder #
#################
cp ../Assets/Adjust/Adjust.cs ./Assets/Adjust/
cp ./Backup/Adjust.prefab ./Assets/Adjust/

####################
# 3rd Party folder #
####################
cp ../Assets/Adjust/3rd\ Party/SimpleJSON.cs ./Assets/Adjust/3rd\ Party/

#########################
# Adjust/Android folder #
#########################
cp ../Assets/Adjust/Android/AdjustAndroid.cs ./Assets/Adjust/Android/
cp ./Backup/AdjustAndroidManifest.xml ./Assets/Adjust/Android/

############################
# Adjust/ExampleGUI folder #
############################
cp ../Assets/Adjust/ExampleGUI/ExampleGUI.cs ./Assets/Adjust/ExampleGUI/
cp ./Backup/ExampleGUI.prefab ./Assets/Adjust/ExampleGUI/
cp ./Backup/ExampleGUI.unity ./Assets/Adjust/ExampleGUI/

#####################
# Adjust/iOS folder #
#####################
cp ../Assets/Adjust/iOS/AdjustiOS.cs ./Assets/Adjust/iOS/

#######################
# Adjust/Metro folder #
#######################
cp ../Assets/Adjust/Metro/AdjustMetro.cs ./Assets/Adjust/Metro/

#######################
# Adjust/Unity folder #
#######################
cp ../Assets/Adjust/Unity/* ./Assets/Adjust/Unity/

#####################
# Adjust/WP8 folder #
#####################
cp ../Assets/Adjust/WP8/AdjustWP8.cs ./Assets/Adjust/WP8/

#################
# Editor folder #
#################
cp ../Assets/Editor/AdjustEditor.cs ./Assets/Editor/
cp ../Assets/Editor/mod_pbxproj.py ./Assets/Editor/
cp ../Assets/Editor/PostprocessBuildPlayer_AdjustPostBuildAndroid.py ./Assets/Editor/
cp ../Assets/Editor/PostprocessBuildPlayer_AdjustPostBuildiOS.py ./Assets/Editor/

##################
# Plugins folder #
##################
cp ../Assets/Adjust/AdjustUnityWP.dll ./Assets/Plugins/
cp ../Assets/Adjust/AdjustUnityWS.dll ./Assets/Plugins/

##########################
# Plugins/Android folder #
##########################
cp ../Assets/Adjust/Android/adjust-android.jar ./Assets/Plugins/Android/

######################
# Plugins/iOS folder #
######################
cp ../Assets/Adjust/iOS/*.h ./Assets/Plugins/iOS/
cp ../Assets/Adjust/iOS/*.mm ./Assets/Plugins/iOS/
cp ../Assets/Adjust/iOS/*.a ./Assets/Plugins/iOS/

########################
# Plugins/Metro folder #
########################
cp ../Assets/Adjust/Metro/*.dll ./Assets/Plugins/Metro/

######################
# Plugins/WP8 folder #
######################
cp ../Assets/Adjust/WP8/*.dll ./Assets/Plugins/WP8/

#####################################################################################
# Target for iOS is named "BuildTarget.iOS" in Unity 5                              #
# This is not compatible with Unity 4 where it should be named "BuildTarget.iPhone" #
# Changing BuildTarget.iOS -> BuildTarget.iPhone in AdjustEditor.cs for Unity 4     #
#####################################################################################
sed -e "s/BuildTarget.iOS/BuildTarget.iPhone/" ./Assets/Editor/AdjustEditor.cs > ./Assets/Editor/AdjustEditor.cs.temp
mv ./Assets/Editor/AdjustEditor.cs.temp ./Assets/Editor/AdjustEditor.cs
