@ECHO off

REM ------------- load submodule
REM ECHO ============== Loading windows SDK submodule ...
REM CALL git submodule update --init --recursive

IF NOT EXIST sdk\Adjust (
	ECHO Error: Adjust Windows SDK submodule not loaded!
	EXIT /B 1
)

TITLE Make Unity SDK for Windows...
SET DEVENV_LOCATION=C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE
SET NUGET_LOCATION=C:\nuget

SET PATH=%PATH%;%DEVENV_LOCATION%;%NUGET_LOCATION%

ECHO.
ECHO ============== Restoring Adjust SDK Nuget Packages ...
nuget restore "sdk\Adjust\Adjust.sln"

ECHO.
ECHO ============== Making the Adjust SDK DLLs ...
devenv "sdk\Adjust\Adjust.sln" /build Debug

ECHO.
ECHO ============== Copying needed Adjust SDK DLLs ...
ECHO f | xcopy /f /y sdk\WindowsPcl\bin\Debug\WindowsPcl.dll bridge\adjust-dlls\WindowsPcl.dll
ECHO f | xcopy /f /y sdk\WindowsUap\bin\Debug\WindowsUap.dll bridge\adjust-dlls\WindowsUap.dll
ECHO f | xcopy /f /y sdk\WindowsUAP10\bin\Debug\AdjustUAP10.dll bridge\adjust-dlls\AdjustUAP10.dll
ECHO f | xcopy /f /y sdk\WindowsStore\bin\Debug\AdjustWS.dll bridge\adjust-dlls\AdjustWS.dll
ECHO f | xcopy /f /y sdk\WindowsPhone81\bin\Debug\AdjustWP81.dll bridge\adjust-dlls\AdjustWP81.dll
REM ---- Integration Testing dll - Test Library
ECHO f | xcopy /f /y sdk\IntegrationTesting\TestLibrary\bin\Debug\TestLibrary.dll bridge\adjust-dlls\TestLibrary.dll

ECHO.
ECHO ============== Restoring the bridge Nuget Packages ...
nuget restore "bridge\WinSdkUnityBridge\WinSdkUnityBridge.sln"

ECHO.
ECHO ============== Making the bridge (stubs and interfaces) ...
devenv "bridge\WinSdkUnityBridge\WinSdkUnityBridge.sln" /build Release

REM ------------------------------------------------------------------------------------------
REM ------------- At this point, we have stubs and interfaces DLLs in ".\bridge\release" folder
REM ------------- root dir
ECHO f | xcopy /f /y bridge\release\WindowsPcl.dll ..\..\Assets\Adjust\Windows\WindowsPcl.dll
ECHO f | xcopy /f /y bridge\release\WindowsUap.dll ..\..\Assets\Adjust\Windows\WindowsUap.dll

REM ------------- STUBS
ECHO f | xcopy /f /y bridge\release\stubs\Win10Interface.dll ..\..\Assets\Adjust\Windows\stubs\Win10Interface.dll
ECHO f | xcopy /f /y bridge\release\stubs\Win81Interface.dll ..\..\Assets\Adjust\Windows\stubs\Win81Interface.dll
ECHO f | xcopy /f /y bridge\release\stubs\WinWsInterface.dll ..\..\Assets\Adjust\Windows\stubs\WinWsInterface.dll
ECHO f | xcopy /f /y bridge\release\stubs\TestLibraryInterface.dll ..\..\Assets\Adjust\Windows\stubs\TestLibraryInterface.dll

REM ------------- WU10
ECHO f | xcopy /f /y bridge\release\Win10Interface.dll ..\..\Assets\Adjust\Windows\WU10\Win10Interface.dll
ECHO f | xcopy /f /y bridge\release\AdjustUAP10.dll ..\..\Assets\Adjust\Windows\WU10\AdjustUAP10.dll

REM ------------- WP 8.1
ECHO f | xcopy /f /y bridge\release\Win81Interface.dll ..\..\Assets\Adjust\Windows\W81\Win81Interface.dll
ECHO f | xcopy /f /y bridge\release\AdjustWP81.dll ..\..\Assets\Adjust\Windows\W81\AdjustWP81.dll

REM ------------- WS
ECHO f | xcopy /f /y bridge\release\WinWsInterface.dll ..\..\Assets\Adjust\Windows\WS\WinWsInterface.dll
ECHO f | xcopy /f /y bridge\release\AdjustWS.dll ..\..\Assets\Adjust\Windows\WS\AdjustWS.dll

REM ------------- Test Library
ECHO f | xcopy /f /y bridge\release\TestLibrary.dll ..\..\Assets\Adjust\Windows\Test\TestLibrary.dll
ECHO f | xcopy /f /y bridge\release\TestLibraryInterface.dll ..\..\Assets\Adjust\Windows\Test\TestLibraryInterface.dll

REM ------------------------------------------------------------------------------------------
REM ------------- Remove used unnecessary files
ECHO ============== Removing unnecessary files ...
RMDIR /s /q bridge\adjust-dlls
RMDIR /s /q bridge\release
ECHO ============== All done!
