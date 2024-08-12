using System;
using Newtonsoft.Json;

namespace AdjustSdk.Test
{
    public class TestLibraryiOS : ITestLibrary
    {
#if UNITY_IOS
        private CommandExecutor _commandExecutor;

        public TestLibraryiOS(string overwriteUrl, string controlUrl)
        {
            _commandExecutor = new CommandExecutor(this, overwriteUrl);
            TestLibraryBridgeiOS.Initialize(overwriteUrl, controlUrl);
        }

        public void StartTestSession() 
        {
            TestApp.Log("TestLibrary -> StartTestSession()");
            Adjust.GetSdkVersion((sdkVersion) => {
                TestLibraryBridgeiOS.StartTestSession(sdkVersion);
            });
        }

        public void AddInfoToSend(string key, string paramValue) 
        {
            TestLibraryBridgeiOS.AddInfoToSend(key, paramValue);
        }

        public void SendInfoToServer(string basePath) 
        {
            TestLibraryBridgeiOS.SendInfoToServer(basePath);
        }

        public void ExecuteCommand(string commandJson)
        {
            Command command = JsonConvert.DeserializeObject<Command>(commandJson);
            _commandExecutor.ExecuteCommand(command);
        }

        public void AddTest(string testName)
        {
            TestLibraryBridgeiOS.AddTest(testName);
        }

        public void AddTestDirectory(string testDirectory)
        {
            TestLibraryBridgeiOS.AddTestDirectory(testDirectory);
        }
#else
        public void AddInfoToSend(string key, string paramValue)
        {
            throw new NotImplementedException();
        }

        public void AddTest(string testName)
        {
            throw new NotImplementedException();
        }

        public void AddTestDirectory(string testDirectory)
        {
            throw new NotImplementedException();
        }

        public void SendInfoToServer(string basePath)
        {
            throw new NotImplementedException();
        }

        public void StartTestSession()
        {
            throw new NotImplementedException();
        }
#endif
    }
}
