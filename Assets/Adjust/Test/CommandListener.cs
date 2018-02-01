using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using com.adjust.sdk;
using Newtonsoft.Json;

namespace com.adjust.sdk.test
{
	public class CommandListener : AndroidJavaProxy
	{
		private static CommandExecutor _commandExecutor;

		public CommandListener(CommandExecutor commandExecutor) : base("com.adjust.testlibrary.ICommandRawJsonListener")
		{
			_commandExecutor = commandExecutor;
		}

		public void executeCommand(string json) {
			if (json == null) {
				return;
			}
				
			Command command = JsonConvert.DeserializeObject<Command> (json);
			_commandExecutor.ExecuteCommand(command);
		}
	}
}

