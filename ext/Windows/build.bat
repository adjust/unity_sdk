@echo off

title Make Unity SDK for Windows...
set DEVENV_LOCATION=C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE
set NUGET_LOCATION=C:\nuget

set PATH=%PATH%;%DEVENV_LOCATION%;%NUGET_LOCATION%

echo.
echo ============== Restoring Adjust SDK Nuget Packages ...
nuget restore "sdk\Adjust\Adjust.sln"

echo.
echo ============== Making the Adjust SDK DLLs ...
devenv "sdk\Adjust\Adjust.sln" /build Release

echo.
echo ============== Copying needed Adjust SDK DLLs ...
echo f | xcopy /f /y sdk\WindowsPcl\bin\Release\WindowsPcl.dll bridge\adjust-dlls\WindowsPcl.dll
echo f | xcopy /f /y sdk\WindowsUap\bin\Release\WindowsUap.dll bridge\adjust-dlls\WindowsUap.dll
echo f | xcopy /f /y sdk\WindowsUAP10\bin\Release\AdjustUAP10.dll bridge\adjust-dlls\AdjustUAP10.dll
echo f | xcopy /f /y sdk\WindowsStore\bin\Release\AdjustWS.dll bridge\adjust-dlls\AdjustWS.dll
echo f | xcopy /f /y sdk\WindowsPhone81\bin\Release\AdjustWP81.dll bridge\adjust-dlls\AdjustWP81.dll

echo.
echo ============== Making the bridge (stubs and interfaces) ...
devenv ".\bridge\WinSdkUnityBridge\WinSdkUnityBridge.sln" /build Release

REM ------------------------------------------------------------------------------------------
REM ------------- At this point, we have stubs and interfaces DLLs in ".\bridge\release" folder
REM ------------- root dir
echo f | xcopy /f /y bridge\release\WindowsPcl.dll ..\..\Assets\Adjust\Windows\WindowsPcl.dll
echo f | xcopy /f /y bridge\release\WindowsUap.dll ..\..\Assets\Adjust\Windows\WindowsUap.dll

REM ------------- STUBS
echo f | xcopy /f /y bridge\release\stubs\Win10Interface.dll ..\..\Assets\Adjust\Windows\stubs\Win10Interface.dll
echo f | xcopy /f /y bridge\release\stubs\Win81Interface.dll ..\..\Assets\Adjust\Windows\stubs\Win81Interface.dll
echo f | xcopy /f /y bridge\release\stubs\WinWsInterface.dll ..\..\Assets\Adjust\Windows\stubs\WinWsInterface.dll

REM ------------- WU10
echo f | xcopy /f /y bridge\release\Win10Interface.dll ..\..\Assets\Adjust\Windows\WU10\Win10Interface.dll
echo f | xcopy /f /y bridge\release\AdjustUAP10.dll ..\..\Assets\Adjust\Windows\WU10\AdjustUAP10.dll

REM ------------- WP 8.1
echo f | xcopy /f /y bridge\release\Win81Interface.dll ..\..\Assets\Adjust\Windows\W81\Win81Interface.dll
echo f | xcopy /f /y bridge\release\AdjustWP81.dll ..\..\Assets\Adjust\Windows\W81\AdjustWP81.dll

REM ------------- WS
echo f | xcopy /f /y bridge\release\WinWsInterface.dll ..\..\Assets\Adjust\Windows\WS\WinWsInterface.dll
echo f | xcopy /f /y bridge\release\AdjustWS.dll ..\..\Assets\Adjust\Windows\WS\AdjustWS.dll

REM ------------------------------------------------------------------------------------------
REM ------------- Remove used unnecessary files
echo ============== Removing unnecessary files ...
rmdir /s /q bridge\adjust-dlls
rmdir /s /q bridge\release
echo ============== All done!