using UnityEngine;
#if UNITY_ANDROID
using Newtonsoft.Json;
#endif

namespace AdjustSdk.Test
{
    public class CommandListenerAndroid : AndroidJavaProxy
    {
#if UNITY_ANDROID
        private static CommandExecutor _commandExecutor;
#endif

        public CommandListenerAndroid(CommandExecutor commandExecutor) : base("com.adjust.test.ICommandRawJsonListener")
        {
#if UNITY_ANDROID
            _commandExecutor = commandExecutor;
#endif
        }

        public void executeCommand(string json) 
        {
            if (json == null)
            {
                return;
            }
#if UNITY_ANDROID
            Command command = JsonConvert.DeserializeObject<Command>(json);
            _commandExecutor.ExecuteCommand(command);
#endif
        }
    }
}
