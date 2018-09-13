from scripting_utils import *

def build(root_dir, ios_submodule_dir, with_test_lib):
    # ------------------------------------------------------------------
    # paths
    srcdir                = '{0}/sdk'.format(ios_submodule_dir)
    lib_out_dir           = '{0}/Assets/Adjust/iOS'.format(root_dir)
    lib_out_dir_test      = '{0}/Assets/Adjust/iOS/Test'.format(root_dir)
    sdk_static_framework  = '{0}/Frameworks/Static/AdjustSdk.framework'.format(srcdir)
    
    # ------------------------------------------------------------------
    # Building AdjustStatic framework target
    debug_green('Building AdjustStatic framework target ...')
    change_dir(srcdir)
    xcode_build('AdjustStatic')
    copy_file(sdk_static_framework + '/Versions/A/AdjustSdk', lib_out_dir + '/AdjustSdk.a')
    copy_files('*', sdk_static_framework + '/Versions/A/Headers/', lib_out_dir)

    if with_test_lib:
        # ------------------------------------------------------------------
        # Test Library paths
        set_log_tag('IOS-TEST-LIB-BUILD')
        debug_green('Building Test Library started ...')
        waiting_animation(duration=4.0, step=0.025)
        test_static_framework = '{0}/Frameworks/Static/AdjustTestLibrary.framework'.format(srcdir)

        change_dir('{0}/AdjustTests/AdjustTestLibrary'.format(srcdir))
        xcode_build('AdjustTestLibraryStatic')
        copy_file(test_static_framework + '/Versions/A/AdjustTestLibrary', lib_out_dir_test + '/AdjustTestLibrary.a')
        copy_files('*', test_static_framework + '/Versions/A/Headers/', lib_out_dir_test)
