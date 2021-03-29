##
##  Various utility methods.
##

import os, shutil, glob, time, sys, platform, subprocess

# ------------------------------------------------------------------
# Windows specific paths.
nuget_dir  = 'C:/nuget'
devenv_dir = 'C:/Program Files (x86)/Microsoft Visual Studio 14.0/Common7/IDE'

def set_log_tag(t):
    global TAG
    TAG = t

# ------------------------------------------------------------------
# Colors for terminal (does not work in Windows).

CEND = '\033[0m'

CBOLD     = '\33[1m'
CITALIC   = '\33[3m'
CURL      = '\33[4m'
CBLINK    = '\33[5m'
CBLINK2   = '\33[6m'
CSELECTED = '\33[7m'

CBLACK  = '\33[30m'
CRED    = '\33[31m'
CGREEN  = '\33[32m'
CYELLOW = '\33[33m'
CBLUE   = '\33[34m'
CVIOLET = '\33[35m'
CBEIGE  = '\33[36m'
CWHITE  = '\33[37m'

CBLACKBG  = '\33[40m'
CREDBG    = '\33[41m'
CGREENBG  = '\33[42m'
CYELLOWBG = '\33[43m'
CBLUEBG   = '\33[44m'
CVIOLETBG = '\33[45m'
CBEIGEBG  = '\33[46m'
CWHITEBG  = '\33[47m'

CGREY    = '\33[90m'
CRED2    = '\33[91m'
CGREEN2  = '\33[92m'
CYELLOW2 = '\33[93m'
CBLUE2   = '\33[94m'
CVIOLET2 = '\33[95m'
CBEIGE2  = '\33[96m'
CWHITE2  = '\33[97m'

CGREYBG    = '\33[100m'
CREDBG2    = '\33[101m'
CGREENBG2  = '\33[102m'
CYELLOWBG2 = '\33[103m'
CBLUEBG2   = '\33[104m'
CVIOLETBG2 = '\33[105m'
CBEIGEBG2  = '\33[106m'
CWHITEBG2  = '\33[107m'

# ------------------------------------------------------------------
# File system methods.

def copy_file(sourceFile, destFile):
    debug('Copying from {0} to {1}'.format(sourceFile, destFile))
    shutil.copyfile(sourceFile, destFile)

def copy_files(fileNamePattern, sourceDir, destDir):
    for file in glob.glob(sourceDir + '/' + fileNamePattern):
        debug('Copying from {0} to {1}'.format(file, destDir))
        shutil.copy(file, destDir)

def remove_files(fileNamePattern, sourceDir, log=True):
    for file in glob.glob(sourceDir + '/' + fileNamePattern):
        if log:
            debug('Deleting ' + file)
        os.remove(file)

def rename_file(fileNamePattern, newFileName, sourceDir):
    for file in glob.glob(sourceDir + '/' + fileNamePattern):
        debug('Renaming file {0} to {1}'.format(file, newFileName))
        os.rename(file, sourceDir + '/' + newFileName)

def clear_dir(dir):
    shutil.rmtree(dir)
    os.mkdir(dir)

def recreate_dir(dir):
    if os.path.exists(dir):
        shutil.rmtree(dir)
    os.mkdir(dir)

def remove_dir_if_exists(path):
    shutil.rmtree(path)

def change_dir(dir):
    os.chdir(dir)

def check_submodule_dir(platform, submodule_dir):
    if not os.path.isdir(submodule_dir) or not os.listdir(submodule_dir):
        error('Submodule [{0}] folder empty.')
        error('Did you forget to run \'git submodule update --init --recursive\' ?'.format(platform))
        exit()

# ------------------------------------------------------------------
# Debug messages methods.

def debug(msg):
    if not is_windows():
        print(('{0}[{1}][INFO]:{2} {3}').format(CBOLD, TAG, CEND, msg))
    else:
        print(('[{0}][INFO]: {1}').format(TAG, msg))

def debug_green(msg):
    if not is_windows():
        print(('{0}[{1}][INFO]:{2} {3}{4}{5}').format(CBOLD, TAG, CEND, CGREEN, msg, CEND))
    else:
        print(('[{0}][INFO]: {1}').format(TAG, msg))

def debug_blue(msg):
    if not is_windows():
        print(('{0}[{1}][INFO]:{2} {3}{4}{5}').format(CBOLD, TAG, CEND, CBLUE, msg, CEND))
    else:
        print(('[{0}][INFO]: {1}').format(TAG, msg))

def error(msg):
    if not is_windows():
        print(('{0}[{1}][ERROR]:{2} {3}{4}{5}').format(CBOLD, TAG, CEND, CRED, msg, CEND))
    else:
        print(('[{0}][ERROR]: {1}').format(TAG, msg))

# ------------------------------------------------------------------
# Execution and platform methods.

def is_windows():
    return platform.system().lower() == 'windows';

def execute_command(cmd_params, log=True):
    if log:
        debug_blue('Executing ' + str(cmd_params))
    subprocess.call(cmd_params)

def xcode_build_debug(target):
    execute_command(['xcodebuild', '-target', target, '-configuration', 'Debug', '-UseModernBuildSystem=NO' 'clean', 'build'])

def xcode_build_release(target):
    execute_command(['xcodebuild', '-target', target, '-configuration', 'Release', '-UseModernBuildSystem=NO' 'clean', 'build'])

def gradle_make_sdk_jar_debug():
    execute_command(['./gradlew', 'clean', 'adjustCoreJarDebug'])

def gradle_make_sdk_jar_release():
    execute_command(['./gradlew', 'clean', 'adjustCoreJarRelease'])

def gradle_make_test_jar_debug():
    execute_command(['./gradlew', 'clean', ':test-library:adjustTestLibraryJarDebug'])

def gradle_make_test_jar_release():
    execute_command(['./gradlew', 'clean', ':test-library:adjustTestLibraryJarRelease'])

def gradle_make_test_library_aar_debug():
    execute_command(['./gradlew', 'clean', ':test-library:assembleDebug'])

def gradle_make_test_library_aar_release():
    execute_command(['./gradlew', 'clean', ':test-library:assembleRelease'])

def gradle_make_test_options_aar_debug():
    execute_command(['./gradlew', 'clean', ':test-options:assembleDebug'])

def gradle_make_test_options_aar_release():
    execute_command(['./gradlew', 'clean', ':test-options:assembleRelease'])

def gradle_make_oaid_jar_release():
    execute_command(['./gradlew', 'clean', ':sdk-plugin-oaid:adjustOaidAndroidJar'])

def nuget_restore(project_path):
    execute_command(['{0}/nuget.exe'.format(nuget_dir), 'restore', project_path])

def devenv_build(solution_path, configuration='Release'):
    execute_command(['{0}/devenv.exe'.format(devenv_dir), solution_path, '/build', configuration])    

