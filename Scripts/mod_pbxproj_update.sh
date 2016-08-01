#!/usr/bin/env bash

########################################
# End script if one of the lines fails #
########################################
set -e

########################################################
# Go to mod_pbxproj repository and pull updates if any #
########################################################
cd ../ext/mod_pbxproj
git fetch -p
git rebase

####################################################
# Copy mod_pbxproj.py script to destination folder #
####################################################
cp mod_pbxproj/mod_pbxproj.py ../../Assets/Editor/mod_pbxproj.py
