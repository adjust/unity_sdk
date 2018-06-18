using System.Collections.Generic;

namespace TestLibraryInterface
{
    public interface IAdjustCommandExecutor
    {
        void ExecuteCommand(string className, string methodName, Dictionary<string, List<string>> parameters);
    }
}
