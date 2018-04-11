using System;
using Newtonsoft.Json;

namespace com.adjust.sdk.test
{
    public class TestFactoryIOS : ITestFactory
    {
#if UNITY_IOS
        private CommandExecutor _commandExecutor;

        public TestFactoryIOS(string baseUrl)
        {
            _commandExecutor = new CommandExecutor(this, baseUrl);
            TestLibraryBridgeiOS.Initialize(baseUrl);
        }

        public void StartTestSession() 
        {
            TestApp.Log("TestFactory -> StartTestSession()");
            TestLibraryBridgeiOS.StartTestSession(TestApp.CLIENT_SDK);
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
