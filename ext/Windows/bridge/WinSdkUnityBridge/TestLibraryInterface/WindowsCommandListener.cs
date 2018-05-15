using System.Collections.Generic;
using TestLibrary;

namespace TestLibraryInterface
{
    internal class WindowsCommandListener : ICommandListener
    {
        private IAdjustCommandExecutor _adjustCommandExecutor;

        public WindowsCommandListener(IAdjustCommandExecutor adjustCommandExecutor)
        {
            _adjustCommandExecutor = adjustCommandExecutor;
        }

        public void ExecuteCommand(string className, string methodName, Dictionary<string, List<string>> parameters)
        {
            switch (className.ToLower())
            {
                case "adjust":
                    _adjustCommandExecutor.ExecuteCommand(className, methodName, parameters);
                    break;
                default:
                    Log.Debug("Could not find {0} class to execute", className);
                    break;
            }
        }
    }
}
