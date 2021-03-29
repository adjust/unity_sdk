from scripting_utils import *

def build(root_dir, android_submodule_dir, with_test_lib):
    # ------------------------------------------------------------------
    # Paths.
    src_dir             = '{0}/sdk/Adjust'.format(android_submodule_dir)
    jar_in_dir          = '{0}/sdk-core/build/libs'.format(src_dir)
    jar_in_dir_oaid     = '{0}/sdk-plugin-oaid/build/libs'.format(src_dir)
    jar_out_dir         = '{0}/Assets/Adjust/Android'.format(root_dir)
    jar_out_dir_oaid    = '{0}/Assets/AdjustOaid/Android'.format(root_dir)

    # ------------------------------------------------------------------
    # Building Android SDK JAR in release mode.
    debug_green('Building Android SDK JAR in release mode ...')
    change_dir(src_dir)
    gradle_make_sdk_jar_release()

    # ------------------------------------------------------------------
    # Copy Android SDK JAR to destination.
    debug_green('Copying generated Android SDK JAR to destination ...')
    copy_file('{0}/adjust-sdk-release.jar'.format(jar_in_dir), '{0}/adjust-android.jar'.format(jar_out_dir))

    # ------------------------------------------------------------------
    # Building Android SDK JAR in release mode.
    debug_green('Building Android OAID plugin JAR in release mode ...')
    gradle_make_oaid_jar_release()

    # ------------------------------------------------------------------
    # Copy Android OAID plugin JAR to destination.
    debug_green('Copying generated Android OAID plugin JAR to destination ...')
    copy_file('{0}/sdk-plugin-oaid.jar'.format(jar_in_dir_oaid), '{0}/adjust-android-oaid.jar'.format(jar_out_dir_oaid))

    if with_test_lib:
        # ------------------------------------------------------------------
        # Paths.
        set_log_tag('ANDROID-TEST-LIB-BUILD')
        debug_green('Building Test Library started ...')
        test_library_in_dir  = '{0}/test-library/build/outputs/aar'.format(src_dir)
        test_options_in_dir  = '{0}/test-options/build/outputs/aar'.format(src_dir)
        test_unity_out_dir = '{0}/Assets/Adjust/Android/Test'.format(root_dir)

        # ------------------------------------------------------------------
        # Building Android test library AAR in debug mode.
        debug_green('Building Adjust test library AAR in debug mode ...')
        gradle_make_test_library_aar_debug()

        # ------------------------------------------------------------------
        # Copy Android test library AAR from to destination.
        debug_green('Copying generated Android test library AAR from {0} to {1} ...'.format(test_library_in_dir, test_unity_out_dir))
        copy_file('{0}/test-library-debug.aar'.format(test_library_in_dir), '{0}/adjust-test-library.aar'.format(test_unity_out_dir))

        # ------------------------------------------------------------------
        # Building Android test options AAR in debug mode.
        debug_green('Building Adjust test options AAR in debug mode ...')
        gradle_make_test_options_aar_debug()

        # ------------------------------------------------------------------
        # Copy Android test options AAR from to destination.
        debug_green('Copying generated Android test options AAR from {0} to {1} ...'.format(test_options_in_dir, test_unity_out_dir))
        copy_file('{0}/test-options-debug.aar'.format(test_options_in_dir), '{0}/adjust-test-options.aar'.format(test_unity_out_dir))
