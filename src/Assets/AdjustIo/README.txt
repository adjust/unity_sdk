-----------------------------
----- Use of the Plugin -----
-----------------------------
1) Import all files into your project.
2) Read this file completely.
3) Add the prefab Assets/AdjustIo/AdjustIo.prefab to the first scene of the app (only to the first scene!).
4) Change the app token on the AdjustIo object in your hierachy. You can find your app token on the AdjustIo
	dashboard.
5) Change the log level, environment and buffering settings to the desired settings. For more info on these
	settings, go to https://github.com/adeven/adjust_android_sdk/ or https://github.com/adeven/adjust_ios_sdk/ .
6) If you already have an AndroidManifest.xml file in your project, please adapt the manifest according to
	instructions found on https://github.com/adeven/adjust_android_sdk/ .
7) Add the desired tracking functions to your code.
	- Warning: AdjustIo is started in the Awake event. This means that all functions below may not work when
	called in the Awake event of the first scene.
	- Below you can find a list of all availabe functions (these are all static functions in the AdjustIo class):
		AdjustIo.TrackEvent(string eventToken);
		AdjustIo.TrackEvent(string eventToken, Dictionary<string,string> parameters);
		AdjustIo.TrackRevenue(double cents);
		AdjustIo.TrackRevenue(double cents, string eventToken);
		AdjustIo.TrackRevenue(double cents, string eventToken, Dictionary<string,string> parameters);
	
--------------------------------
----- Use of Example Scene -----
--------------------------------
1) Open the scene Assets/AdjustIo/Example/ExampleScene.
2) You will see the AdjustIo object in the Hiearchy tab. Change the app token parameter of this object
to your app token. You can find your app token on the AdjustIo dashboard.
3) In the Hierarchy tab you will also find the ExampleUI object. Change the event token parameter of this
	object to an event token of an event you have created for this app. This information can be found on the
	AdjustIo dashboard. The example will use this event token when tracking events or when tracking revenue.
4) You can now run the example in the editor. You should see some buttons which will send events. While running
	in the Unity Editor the events will not be sent to the AdjustIo server (see below for more information).
5) Build the example scene to an Android or iOS device so you can test if you receive any events on the dashboard.
	Be sure read the build instructions below first.

--------------------------------------------------------
----- Notes on Running the App in the Unity Editor ----- 
--------------------------------------------------------
The AdjustIo will not throw any errors when testing the app in the Editor. However most of the
code will not be called while testing in the Editor. This means that the AdjustIo server will
not receive any messages while testing in the Unity Editor.

--------------------------------------
----- Android Build Instructions ----- 
--------------------------------------
1) Open the scene containing the AdjustIo object.
2) Check the app token, log level, environment and buffering settings.
3) Go to the menu and select AdjustIo > Check AndroidManifest.xml
	This will update the AndroidManifest to use the app token, log level, environment and 
	buffering settings chosen on the AdjustIo object.
4) You are now ready to build using the 'Build' or 'Build And Run' options from the 'Build Settings' menu.

--------------------------------------
----- iOS Build Instructions ----- 
--------------------------------------
1) Open the scene containing the AdjustIo object.
2) Check the app token, log level, environment and buffering settings. 
3) You are now ready to build using the Build or Build And Run from the Build Settings menu.
	Warning: when you use File > Build and Run, the plugin may not be included correctly.
4) When you notice that the plugin is not added to the Xcode project, be sure to check the file permissions on the file Assets/AdjustIo/Data/PostBuildScripts/PostBuildAdjustIoScript. You should have the execute permission on this file.


License notes
-------------

The file mod_pbxproj.py is licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
