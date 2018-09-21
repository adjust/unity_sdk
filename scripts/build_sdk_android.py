from scripting_utils import *

def build(root_dir, android_submodule_dir, with_test_lib):
    # ------------------------------------------------------------------
    # paths
    sdk_adjust_dir = '{0}/sdk/Adjust'.format(android_submodule_dir)
    jar_in_dir     = '{0}/adjust/build/intermediates/intermediate-jars/release'.format(sdk_adjust_dir)
    jar_out_dir    = '{0}/Assets/Adjust/Android'.format(root_dir)

    # ------------------------------------------------------------------
    # Gradle: making release JAR
    debug_green('Gradle: making release JAR ...')
    change_dir(sdk_adjust_dir)
    gradle_make_release_jar()

    # ------------------------------------------------------------------
    # Copy new JAR
    debug_green('Copy new JAR ...')
    copy_file('{0}/classes.jar'.format(jar_in_dir), '{0}/adjust-android.jar'.format(jar_out_dir))

    if with_test_lib:
        # ------------------------------------------------------------------
        # Test Library paths
        set_log_tag('ANROID-TEST-LIB-BUILD')
        waiting_animation(duration=4.0, step=0.025)
        debug_green('Building Test Library started ...')
        test_jar_in_dir  = '{0}/testlibrary/build/intermediates/intermediate-jars/debug'.format(sdk_adjust_dir)
        test_jar_out_dir = '{0}/Assets/Adjust/Android/Test'.format(root_dir)

        # ------------------------------------------------------------------
        # Running Gradle tasks: clean :testlibrary:makeJar
        debug_green('Running Gradle tasks: clean :testlibrary:makeJar ...')
        gradle_make_testlib_jar()

        # ------------------------------------------------------------------
        # Moving test library JAR from ${JAR_IN_DIR} to ${JAR_OUT_DIR} ...
        debug_green('Copying test library JAR from {0} to {1} ...'.format(test_jar_in_dir, test_jar_out_dir))
        copy_file('{0}/classes.jar'.format(test_jar_in_dir), '{0}/adjust-testing.jar'.format(test_jar_out_dir))
