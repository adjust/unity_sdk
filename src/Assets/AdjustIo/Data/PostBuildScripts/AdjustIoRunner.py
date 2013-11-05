#!/usr/bin/python

import sys
import shutil
import os

from mod_pbxproj import XcodeProject

f = open('AdjustIoBuildLogFile.txt','a')
f.write('Start of python script\n')
f.close()

projectPath = sys.argv[1]

f = open('AdjustIoBuildLogFile.txt','a')
f.write('project path:\n')
f.write(projectPath)
f.write('\n')
f.close()

XcodePath = sys.argv[3]
XcodeFrameworksPath = XcodePath + '/System/Library/Frameworks/'

f = open('FiksuBuildLogFile.txt','a')
f.write('Framework path:\n')
f.write(XcodeFrameworksPath)
f.write('\n')
f.close()

project = XcodeProject.Load(projectPath + '/Unity-iPhone.xcodeproj/project.pbxproj')

f = open('AdjustIoBuildLogFile.txt','a')
f.write('project loaded:\n')
f.close()

f = open('AdjustIoBuildLogFile.txt','a')
f.write('Starting to copy AdjustIO SDK\n')
f.close()

shutil.rmtree(projectPath + '/Classes/AdjustIo', True);
shutil.copytree(sys.argv[2] + 'SDK/iOS/AdjustIo' , projectPath + '/Classes/AdjustIo',ignore = shutil.ignore_patterns('*.meta'));
shutil.copyfile(sys.argv[2] + 'iOSWrapper/AdjustIoIosUnityWrapper.mm', projectPath + '/Classes/AdjustIo/AdjustIoIosUnityWrapper.mm');

f = open('AdjustIoBuildLogFile.txt','a')
f.write('Done copying AdjustIO SDK\n')
f.close()

f = open('AdjustIoBuildLogFile.txt','a')
f.write('Adding files to project\n')
f.close()

adjustIoGroup = project.get_or_create_group('AdjustIo')

for root, dirs, files in os.walk(projectPath + '/Classes/AdjustIo'):
	for f in files:
		results = project.add_file_if_doesnt_exist(os.path.join(root,f), parent=adjustIoGroup )
		for pbxfile in results:
			if pbxfile.get('isa') == 'PBXBuildFile':
				pbxfile.add_compiler_flag('-fobjc-arc')

f = open('AdjustIoBuildLogFile.txt','a')
f.write('Files added to project\n')
f.close()

f = open('AdjustIoBuildLogFile.txt','a')
f.write('Adding adSupport framework\n')
f.close()

project.add_file_if_doesnt_exist(XcodeFrameworksPath + 'AdSupport.framework', tree='SDKROOT',create_build_files=True,weak=True);

f = open('AdjustIoBuildLogFile.txt','a')
f.write('Support framework added\n')
f.close()


project.saveFormat3_2()
f = open('AdjustIoBuildLogFile.txt','a')
f.write('Saved project:\n')
f.close()
