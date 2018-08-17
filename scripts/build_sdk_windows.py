import os, subprocess, shutil
from scripting_utils import *

def build(root_dir, windows_submodule_dir):
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
