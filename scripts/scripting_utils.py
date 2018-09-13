##
##  Various util python methods which can be utilized and shared among different scripts
##
import os, shutil, glob, time, sys, platform, subprocess

def set_log_tag(t):
    global TAG
    TAG = t

############################################################
### colors for terminal (does not work in Windows... of course)

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

############################################################
### file system util methods

def copy_file(sourceFile, destFile):
    debug('copying: {0} -> {1}'.format(sourceFile, destFile))
    shutil.copyfile(sourceFile, destFile)

def copy_files(fileNamePattern, sourceDir, destDir):
    for file in glob.glob(sourceDir + '/' + fileNamePattern):
        debug('copying: {0} -> {1}'.format(file, destDir))
        shutil.copy(file, destDir)

def remove_files(fileNamePattern, sourceDir, log=True):
    for file in glob.glob(sourceDir + '/' + fileNamePattern):
        if log:
            debug('deleting: ' + file)
        os.remove(file)

def rename_file(fileNamePattern, newFileName, sourceDir):
    for file in glob.glob(sourceDir + '/' + fileNamePattern):
        debug('rename: {0} -> {1}'.format(file, newFileName))
        os.rename(file, sourceDir + '/' + newFileName)

def clear_dir(dir):
    shutil.rmtree(dir)
    os.mkdir(dir)    

############################################################
### debug messages util methods

def debug(msg):
    if not is_windows():
        print(('{0}* [{1}][INFO]:{2} {3}').format(CBOLD, TAG, CEND, msg))
    else:
        print(('* [{0}][INFO]: {1}').format(TAG, msg))

def debug_green(msg):
    if not is_windows():
        print(('{0}* [{1}][INFO]:{2} {3}{4}{5}').format(CBOLD, TAG, CEND, CGREEN, msg, CEND))
    else:
        print(('* [{0}][INFO]: {1}').format(TAG, msg))

def debug_blue(msg):
    if not is_windows():
        print(('{0}* [{1}][INFO]:{2} {3}{4}{5}').format(CBOLD, TAG, CEND, CBLUE, msg, CEND))
    else:
        print(('* [{0}][INFO]: {1}').format(TAG, msg))

def error(msg):
    if not is_windows():
        print(('{0}* [{1}][ERROR]:{2} {3}{4}{5}').format(CBOLD, TAG, CEND, CRED, msg, CEND))
    else:
        print(('* [{0}][ERROR]: {1}').format(TAG, msg))

############################################################
### util

def execute_command(cmd_params, log=True):
    if log:
        debug_blue('Executing: ' + str(cmd_params))
    subprocess.call(cmd_params)

def change_dir(dir):
    os.chdir(dir)

def check_submodule_dir(platform, submodule_dir):
    if not os.path.isdir(submodule_dir) or not os.listdir(submodule_dir):
        error('Submodule [{0}] folder empty. Did you forget to run >> git submodule update --init --recursive << ?'.format(platform))
        exit()

def is_windows():
    return platform.system().lower() == 'windows';

def xcode_build(target, configuration='Release'):
    execute_command(['xcodebuild', '-target', target, '-configuration', configuration, 'clean', 'build'])

def gradle_make_release_jar():
    execute_command(['./gradlew', 'clean', 'makeReleaseJar'])

def gradle_make_testlib_jar():
    execute_command(['./gradlew', 'clean', ':testlibrary:makeJar'])

############################################################
### nonsense, eyecandy and such

def waiting_animation(duration, step):
    if(duration <= step):
        return

    line = '-'
    line_killer = '\b'
    while duration >= 0:
        duration -= step
        sys.stdout.write(line)
        sys.stdout.flush()
        sys.stdout.write(line_killer)
        line += '-'
        line_killer += '\b'
        if len(line) > 65:
            line = '-'
        time.sleep(step)
