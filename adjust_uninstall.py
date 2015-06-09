#!/usr/bin/env python

import os
import re
import sys
import shutil

def main():
    assets_path = os.getcwd() + "/Assets";

    re_adjust_files = re.compile(r"AI.*\.m|.*\+AI.*\.m|.*\+AI.*\.h|AI.*\.h|ADJ.*\.m|ADJ.*\.h|Adjust\.*|adjust\.*|ExampleGUI\.*|.*AdjustPostBuild.*|mod_pbxproj\.*|IAdjust\.*|ResponseData\.*|SimpleJSON\.*")
    re_adjust_folders = re.compile(r"Adjust|3rd Party|ExampleGUI|adjust")

    for path, subdirs, files in os.walk(assets_path):
        for name in files:
            adjust_file_match = re_adjust_files.match(name if name else "")

            if (adjust_file_match):
                file_path = os.path.join(path, name)

                if (os.path.exists(file_path)):
                    print "Removing file: " + file_path
                    os.remove(file_path)

    for path, subdirs, files in os.walk(assets_path):
        for name in subdirs:
            adjust_folder_match = re_adjust_folders.match(name if name else "")

            if (adjust_folder_match):
                folder_path = os.path.join(path, name)

                if (os.path.exists(folder_path)):
                    print "Removing folder: " + folder_path
                    shutil.rmtree(folder_path)

    print "Adjust successfully removed from your project"

    sys.exit(0)

if __name__ == "__main__":
    main()