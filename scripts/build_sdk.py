import os
import subprocess
import sys
from scripting_utils import *

set_log_tag('BUILD-SDK')

# ------------------------------------------------------------------
# get arguments
usage_message = 'Usage >> python build_sdk.py [ios | android | windows] [otpional, to build test library too: --with-testlib | -tl]\n';
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
# common paths
script_dir              = os.path.dirname(os.path.realpath(__file__))
root_dir                = os.path.dirname(os.path.normpath(script_dir))
android_submodule_dir   = '{0}/ext/Android'.format(root_dir)
ios_submodule_dir       = '{0}/ext/iOS'.format(root_dir)
windows_submodule_dir   = '{0}/ext/Windows'.format(root_dir)

def build_for_android():
    # ------------------------------------------------------------------
    # paths
    sdk_adjust_dir = '{0}/sdk/Adjust'.format(android_submodule_dir)
    jar_in_dir     = '{0}/target'.format(sdk_adjust_dir)
    jar_out_dir    = '{0}/Assets/Adjust/Android'.format(root_dir)

    # ------------------------------------------------------------------
    # Running MVN clean & package
    debug_green('Running MVN clean & MVN package ...')
    os.chdir(sdk_adjust_dir)
    subprocess.call(['mvn', 'clean'])
    subprocess.call(['mvn', 'package'])

    # ------------------------------------------------------------------
    # Previous SDK JAR cleanup and generating new one
    debug_green('Previous SDK JAR cleanup and generating new one ...')
    remove_files('adjust-android*', jar_out_dir)
    copy_files('adjust-android-*.*.*.jar', jar_in_dir, jar_out_dir)
    remove_files('*-javadoc.jar', jar_out_dir)
    remove_files('*-sources.jar', jar_out_dir)
    rename_file('adjust-android-*.*.*.jar', 'adjust-android.jar', jar_out_dir)

    if with_test_lib:
        # ------------------------------------------------------------------
        # Test Library paths
        set_log_tag('ANROID-TEST-LIB-BUILD')
        waiting_animation(duration=4.0, step=0.025)
        debug_green('Building Test Library started ...')
        test_jar_in_dir  = '{0}/testlibrary/build/outputs'.format(sdk_adjust_dir)
        test_jar_out_dir = '{0}/Assets/Adjust/Android'.format(root_dir)

        # ------------------------------------------------------------------
        # Running Gradle tasks: clean :testlibrary:makeJar
        debug_green('Running Gradle tasks: clean :testlibrary:makeJar ...')
        subprocess.call(['./gradlew', 'clean', ':testlibrary:makeJar'])

        # ------------------------------------------------------------------
        # Moving test library JAR from ${JAR_IN_DIR} to ${JAR_OUT_DIR} ...
        debug_green('Moving test library JAR from {0} to {1} ...'.format(test_jar_in_dir, test_jar_out_dir))
        copy_files('*.jar', test_jar_in_dir, test_jar_out_dir)

def build_for_ios():
    # ------------------------------------------------------------------
    # paths
    srcdir                = '{0}/sdk'.format(ios_submodule_dir)
    lib_out_dir           = '{0}/Assets/Adjust/iOS'.format(root_dir)
    sdk_static_framework  = '{0}/Frameworks/Static/AdjustSdk.framework'.format(srcdir)
    
    # ------------------------------------------------------------------
    # Building AdjustStatic framework target
    debug_green('Building AdjustStatic framework target ...')
    os.chdir(srcdir)
    subprocess.call(['xcodebuild', '-target', 'AdjustStatic', '-configuration', 'Release', 'clean', 'build'])
    copy_file(sdk_static_framework + '/Versions/A/AdjustSdk', lib_out_dir + '/AdjustSdk.a')
    copy_files('*', sdk_static_framework + '/Versions/A/Headers/', lib_out_dir)

    if with_test_lib:
        # ------------------------------------------------------------------
        # Test Library paths
        set_log_tag('IOS-TEST-LIB-BUILD')
        debug_green('Building Test Library started ...')
        waiting_animation(duration=4.0, step=0.025)
        test_static_framework = '{0}/Frameworks/Static/AdjustTestLibrary.framework'.format(srcdir)

        os.chdir('{0}/AdjustTests/AdjustTestLibrary'.format(srcdir))
        subprocess.call(['xcodebuild', '-target', 'AdjustTestLibraryStatic', '-configuration', 'Release', 'clean', 'build'])
        copy_file(test_static_framework + '/Versions/A/AdjustTestLibrary', lib_out_dir + '/AdjustTestLibrary.a')
        copy_files('*', test_static_framework + '/Versions/A/Headers/', lib_out_dir)

def build_for_windows():
    ### TODO: complete windows part on Win Machine ...


    # ------------------------------------------------------------------
    # Script completed
    debug_green('Script completed!')

# ------------------------------------------------------------------
# call platform specific build method
if platform == 'ios':
    set_log_tag('IOS-SDK-BUILD')
    if not os.listdir('{0}/sdk'.format(ios_submodule_dir)):
        error('iOS submodule folder empty. Did you forget to run >git submodule update --init --recursive< ?')
        exit()
    build_for_ios()
elif platform == 'android':
    set_log_tag('ANROID-SDK-BUILD')
    if not os.listdir('{0}/sdk'.format(android_submodule_dir)):
        error('Android submodule folder empty. Did you forget to run >git submodule update --init --recursive< ?')
        exit()
    build_for_android()
else:
    set_log_tag('WINDOWS-SDK-BUILD')
    if not os.listdir('{0}/sdk'.format(windows_submodule_dir)):
        error('Windows submodule folder empty. Did you forget to run >git submodule update --init --recursive< ?')
        exit()
    build_for_windows()

# ------------------------------------------------------------------
# Script completed
debug_green('Script completed!')
