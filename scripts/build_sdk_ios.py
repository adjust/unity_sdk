from scripting_utils import *

def build(root_dir, ios_submodule_dir, with_test_lib):
    # ------------------------------------------------------------------
    # Paths.
    src_dir               = '{0}/sdk'.format(ios_submodule_dir)
    lib_out_dir           = '{0}/Assets/Adjust/iOS'.format(root_dir)
    lib_out_dir_test      = '{0}/Assets/Adjust/iOS/Test'.format(root_dir)
    sdk_static_framework  = '{0}/Frameworks/Static/AdjustSdk.framework'.format(src_dir)
    
    # ------------------------------------------------------------------
    # Build AdjustStatic framework target.
    debug_green('Building AdjustStatic framework target ...')
    change_dir(src_dir)
    xcode_build_release('AdjustStatic')
    copy_file(sdk_static_framework + '/Versions/A/AdjustSdk', lib_out_dir + '/AdjustSdk.a')
    copy_files('*', sdk_static_framework + '/Versions/A/Headers/', lib_out_dir)

    if with_test_lib:
        # ------------------------------------------------------------------
        # Paths.
        test_static_framework = '{0}/Frameworks/Static/AdjustTestLibrary.framework'.format(src_dir)

        # ------------------------------------------------------------------
        # Build AdjustTestLibraryStatic framework target.
        set_log_tag('IOS-TEST-LIB-BUILD')
        debug_green('Building Test Library started ...')
        change_dir('{0}/AdjustTests/AdjustTestLibrary'.format(src_dir))
        xcode_build_debug('AdjustTestLibraryStatic')
        copy_file(test_static_framework + '/Versions/A/AdjustTestLibrary', lib_out_dir_test + '/AdjustTestLibrary.a')
        copy_files('*', test_static_framework + '/Versions/A/Headers/', lib_out_dir_test)
