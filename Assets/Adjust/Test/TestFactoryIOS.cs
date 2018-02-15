using System;
using Newtonsoft.Json;

namespace com.adjust.sdk.test
{
    public class TestFactoryIOS : ITestFactory
    {
		private CommandExecutor _commandExecutor;

        public TestFactoryIOS (string baseUrl)
        {
			_commandExecutor = new CommandExecutor(this, baseUrl);
			TestLibraryBridgeiOS.Initialize(baseUrl);
        }

        public void StartTestSession(string testNames = null) 
        {
            TestApp.Log ("TestFactory -> StartTestSession()");
			TestLibraryBridgeiOS.StartTestSession (TestApp.CLIENT_SDK);
        }

        public void AddInfoToSend(string key, string paramValue) 
        {
			TestLibraryBridgeiOS.AddInfoToSend (key, paramValue);
        }

        public void SendInfoToServer(string basePath) 
        {
			TestLibraryBridgeiOS.SendInfoToServer (basePath);
        }

		public void ExecuteCommand(string commandJson)
		{
			Command command = JsonConvert.DeserializeObject<Command> (commandJson);
			_commandExecutor.ExecuteCommand (command);
		}
    }
}

