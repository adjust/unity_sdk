import os, subprocess
from scripting_utils import *

def build(root_dir, android_submodule_dir, with_test_lib):
    # ------------------------------------------------------------------
    # paths
    sdk_adjust_dir = '{0}/sdk/Adjust'.format(android_submodule_dir)
    # jar_in_dir     = '{0}/target'.format(sdk_adjust_dir)
    jar_in_dir     = '{0}/adjust/build/intermediates/intermediate-jars/release'.format(sdk_adjust_dir)
    jar_out_dir    = '{0}/Assets/Adjust/Android'.format(root_dir)

    # ------------------------------------------------------------------
    # Running MVN clean & package
    debug_green('Running MVN clean & MVN package ...')
    os.chdir(sdk_adjust_dir)
    # subprocess.call(['mvn', 'clean'])
    # subprocess.call(['mvn', 'package'])
    subprocess.call(['./gradlew', 'clean', 'makeReleaseJar'])

    # ------------------------------------------------------------------
    # Previous SDK JAR cleanup and generating new one
    # debug_green('Previous SDK JAR cleanup and generating new one ...')
    # remove_files('adjust-android*', jar_out_dir)
    # copy_files('adjust-android-*.*.*.jar', jar_in_dir, jar_out_dir)
    # remove_files('*-javadoc.jar', jar_out_dir)
    # remove_files('*-sources.jar', jar_out_dir)
    # rename_file('adjust-android-*.*.*.jar', 'adjust-android.jar', jar_out_dir)
    copy_files('classes.jar', jar_in_dir, jar_out_dir)
    rename_file('classes.jar', 'adjust-android.jar', jar_out_dir)

    if with_test_lib:
        # ------------------------------------------------------------------
        # Test Library paths
        set_log_tag('ANROID-TEST-LIB-BUILD')
        waiting_animation(duration=4.0, step=0.025)
        debug_green('Building Test Library started ...')
        # test_jar_in_dir  = '{0}/testlibrary/build/outputs'.format(sdk_adjust_dir)
        test_jar_in_dir  = '{0}/testlibrary/build/intermediates/intermediate-jars/debug'.format(sdk_adjust_dir)
        test_jar_out_dir = '{0}/Assets/Adjust/Android/Test'.format(root_dir)

        # ------------------------------------------------------------------
        # Running Gradle tasks: clean :testlibrary:makeJar
        debug_green('Running Gradle tasks: clean :testlibrary:makeJar ...')
        subprocess.call(['./gradlew', 'clean', ':testlibrary:makeJar'])

        # ------------------------------------------------------------------
        # Moving test library JAR from ${JAR_IN_DIR} to ${JAR_OUT_DIR} ...
        debug_green('Copying test library JAR from {0} to {1} ...'.format(test_jar_in_dir, test_jar_out_dir))
        copy_files('classes.jar', test_jar_in_dir, test_jar_out_dir)
        rename_file('classes.jar', 'adjust-testing.jar', test_jar_out_dir)
        