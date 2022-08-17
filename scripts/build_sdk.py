#!/usr/bin/python

import os, sys
from scripting_utils import *
import build_sdk_android    as android
import build_sdk_ios        as ios
import build_sdk_windows    as windows

set_log_tag('BUILD-SDK')

if __name__ != "__main__":
    error('Error. Do not import this script, but run it explicitly.')
    exit()

# ------------------------------------------------------------------
# get arguments
usage_message = 'Usage: python build_sdk.py [ios | android | windows] [optional, to build test library too: --with-testlib | -tl]\n';
if len(sys.argv) < 2:
    error('Error. Platform not provided.')
    debug(usage_message)
    exit()

platform = sys.argv[1]
if platform != 'ios' and platform != 'android' and platform != 'windows':
    error('Error. Unknown platform provided: [{0}]'.format(platform))
    debug(usage_message)
    exit()

with_test_lib = False
if len(sys.argv) == 3 and (sys.argv[2] == '--with-testlib' or sys.argv[2] == '-tl'):
    with_test_lib = True
elif len(sys.argv) == 3:
    error('Unknown 2nd parameter.')
    debug(usage_message)
    exit()

debug_green('Script start. Platform=[{0}]. With Test Library=[{1}]. Build Adjust Unity SDK ...'.format(platform, with_test_lib))

# ------------------------------------------------------------------
# Paths
script_dir              = os.path.dirname(os.path.realpath(__file__))
root_dir                = os.path.dirname(os.path.normpath(script_dir))
android_submodule_dir   = '{0}/ext/android'.format(root_dir)
ios_submodule_dir       = '{0}/ext/ios'.format(root_dir)
windows_submodule_dir   = '{0}/ext/windows'.format(root_dir)

# ------------------------------------------------------------------
# Call platform specific build method.
if platform == 'ios':
    set_log_tag('IOS-SDK-BUILD')
    check_submodule_dir('iOS', ios_submodule_dir + '/sdk')
    ios.build(root_dir, ios_submodule_dir, with_test_lib)
elif platform == 'android':
    set_log_tag('ANROID-SDK-BUILD')
    check_submodule_dir('Android', android_submodule_dir + '/sdk')
    android.build(root_dir, android_submodule_dir, with_test_lib)
else:
    set_log_tag('WINDOWS-SDK-BUILD')
    check_submodule_dir('Windows', windows_submodule_dir + '/sdk')
    windows.build(root_dir, windows_submodule_dir)

remove_files('*.pyc', script_dir, log=False)

# ------------------------------------------------------------------
# Script completed.
debug_green('Script completed!')
