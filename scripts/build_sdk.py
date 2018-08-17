import os, subprocess, sys, shutil
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
windows_submodule_dir   = '{0}/ext/windows'.format(root_dir)

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
    # ------------------------------------------------------------------
    # paths
    devenv_dir              = 'C:/Program Files (x86)/Microsoft Visual Studio 14.0/Common7/IDE'
    nuget_dir               = 'C:/nuget'
    lib_out_dir             = '{0}/Assets/Adjust/Windows'.format(root_dir)
    bridge_dlls_dir         = '{0}/bridge/adjust-dlls'.format(windows_submodule_dir)
    bridge_release_dlls_dir = '{0}/bridge/release'.format(windows_submodule_dir)
    windows_sdk_dir         = '{0}/sdk'.format(windows_submodule_dir)

    if not os.path.isdir(bridge_dlls_dir):
        os.mkdir(bridge_dlls_dir)
    else:
        clear_dir(bridge_dlls_dir)

    # ------------------------------------------------------------------
    # Restoring Adjust SDK Nuget Packages
    debug_green('Restoring Adjust SDK Nuget Packages ...')
    subprocess.call(['{0}/nuget.exe'.format(nuget_dir), 'restore', '{0}/sdk/Adjust'.format(windows_submodule_dir)])

    # ------------------------------------------------------------------
    # Making the Adjust SDK DLLs
    debug_green('Making the Adjust SDK DLLs ...')
    subprocess.call(['{0}/devenv.exe'.format(devenv_dir), '{0}/sdk/Adjust/Adjust.sln'.format(windows_submodule_dir), '/build', 'Debug'])

    # ------------------------------------------------------------------
    # Copying needed Adjust SDK DLLs
    debug_green('Copying needed Adjust SDK DLLs ...')
    copy_file(windows_sdk_dir + '/WindowsPcl/bin/Debug/WindowsPcl.dll', bridge_dlls_dir + '/WindowsPcl.dll')
    copy_file(windows_sdk_dir + '/WindowsUap/bin/Debug/WindowsUap.dll', bridge_dlls_dir + '/WindowsUap.dll')
    copy_file(windows_sdk_dir + '/WindowsUAP10/bin/Debug/AdjustUAP10.dll', bridge_dlls_dir + '/AdjustUAP10.dll')
    copy_file(windows_sdk_dir + '/WindowsStore/bin/Debug/AdjustWS.dll', bridge_dlls_dir + '/AdjustWS.dll')
    copy_file(windows_sdk_dir + '/WindowsPhone81/bin/Debug/AdjustWP81.dll', bridge_dlls_dir + '/AdjustWP81.dll')
    copy_file(windows_sdk_dir + '/IntegrationTesting/TestLibrary/bin/Debug/TestLibrary.dll', bridge_dlls_dir + '/TestLibrary.dll')

    # ------------------------------------------------------------------
    # Restoring the bridge Nuget Packages
    debug_green('Restoring the bridge Nuget Packages ...')
    subprocess.call(['{0}/nuget.exe'.format(nuget_dir), 'restore', '{0}/bridge/WinSdkUnityBridge/WinSdkUnityBridge.sln'.format(windows_submodule_dir)])

    # ------------------------------------------------------------------
    # Making the bridge (stubs and interfaces)
    debug_green('Making the bridge (stubs and interfaces) ...')
    subprocess.call(['{0}/devenv.exe'.format(devenv_dir), '{0}/bridge/WinSdkUnityBridge/WinSdkUnityBridge.sln'.format(windows_submodule_dir), '/build', 'Release'])

    # ------------------------------------------------------------------
    # At this point, we have stubs and interfaces DLLs in ".\bridge\release" folder
    copy_file(bridge_release_dlls_dir + '/WindowsPcl.dll', lib_out_dir + '/WindowsPcl.dll')
    copy_file(bridge_release_dlls_dir + '/WindowsUap.dll', lib_out_dir + '/WindowsUap.dll')
    # stubs
    copy_file(bridge_release_dlls_dir + '/stubs/Win10Interface.dll', lib_out_dir + '/Stubs/Win10Interface.dll')
    copy_file(bridge_release_dlls_dir + '/stubs/Win81Interface.dll', lib_out_dir + '/Stubs/Win81Interface.dll')
    copy_file(bridge_release_dlls_dir + '/stubs/WinWsInterface.dll', lib_out_dir + '/Stubs/WinWsInterface.dll')
    copy_file(bridge_release_dlls_dir + '/stubs/TestLibraryInterface.dll', lib_out_dir + '/Stubs/TestLibraryInterface.dll')
    # WU10
    copy_file(bridge_release_dlls_dir + '/Win10Interface.dll', lib_out_dir + '/WU10/Win10Interface.dll')
    copy_file(bridge_release_dlls_dir + '/AdjustUAP10.dll', lib_out_dir + '/WU10/AdjustUAP10.dll')
    # WP 8.1
    copy_file(bridge_release_dlls_dir + '/Win81Interface.dll', lib_out_dir + '/W81/Win81Interface.dll')
    copy_file(bridge_release_dlls_dir + '/AdjustWP81.dll', lib_out_dir + '/W81/AdjustWP81.dll')
    # WP WS
    copy_file(bridge_release_dlls_dir + '/WinWsInterface.dll', lib_out_dir + '/WS/WinWsInterface.dll')
    copy_file(bridge_release_dlls_dir + '/AdjustWS.dll', lib_out_dir + '/WS/AdjustWS.dll')
    # Test Library
    copy_file(bridge_release_dlls_dir + '/TestLibrary.dll', lib_out_dir + '/Test/TestLibrary.dll')
    copy_file(bridge_release_dlls_dir + '/TestLibraryInterface.dll', lib_out_dir + '/Test/TestLibraryInterface.dll')

    # ------------------------------------------------------------------
    # Remove used unnecessary files
    shutil.rmtree(bridge_dlls_dir)
    shutil.rmtree(bridge_release_dlls_dir)

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

    if not os.listdir('{0}/sdk/'.format(windows_submodule_dir)):
        error('Windows submodule folder empty. Did you forget to run >> git submodule update --init --recursive << ?')
        exit()
    build_for_windows()

# ------------------------------------------------------------------
# Script completed
debug_green('Script completed!')
