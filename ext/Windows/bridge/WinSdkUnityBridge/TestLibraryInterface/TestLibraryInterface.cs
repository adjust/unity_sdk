using System;
using AdjustSdk.Pcl;
using Windows.Networking.Connectivity;
using AdjustSdk;

namespace TestLibraryInterface
{
    public class TestLibraryInterface
    {
        private TestLibrary.TestLibrary _testLibrary;
        private Action<string> _logDelegate;

        public void Init(IAdjustCommandExecutor adjustCommandExecutor, string baseUrl, Action<string> logDelegate = null)
        {
            _logDelegate = logDelegate;
            Log("Initializing...");

            var localIp = GetLocalIp();
            Log("Local IP: " + localIp);

            var commandListener = new WindowsCommandListener(adjustCommandExecutor);

            Log("Setting AdjustFactory.BaseUrl to: " + baseUrl);
            AdjustFactory.BaseUrl = baseUrl;

            _testLibrary = new TestLibrary.TestLibrary(baseUrl, commandListener, localIp, _logDelegate);
            Log("Init finished.");
        }

        public static void SetTestOptions(AdjustTestOptionsDto adjustTestOptionsDto)
        {
            Adjust.SetTestOptions(adjustTestOptionsDto.ToAdjustTestOptions());
        }

        public void StartTestSession(string clientSdk, string testNames = null)
        {
            if (_testLibrary == null) { return; }

            Log("Starting Test Session...");

            if(!string.IsNullOrEmpty(testNames))
            {
                _testLibrary.SetTests(testNames);
            }

            _testLibrary.StartTestSession(clientSdk);
        }
        
        public void AddInfoToSend(string key, string paramValue)
        {
            if (_testLibrary == null) { return; }
            _testLibrary.AddInfoToSend(key, paramValue);
        }

        public void SendInfoToServer(string basePath)
        {
            if (_testLibrary == null) { return; }
            _testLibrary.SendInfoToServer(basePath);
        }

        private string GetLocalIp()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();

            if (icp?.NetworkAdapter == null) return null;

            var hostNames = NetworkInformation.GetHostNames();
            foreach (var hn in hostNames)
            {
                if (hn.IPInformation?.NetworkAdapter == null)
                    continue;

                if (hn.IPInformation.NetworkAdapter.NetworkAdapterId == icp.NetworkAdapter.NetworkAdapterId)
                    return hn.CanonicalName;
            }

            return string.Empty;
        }

        private void Log(string message)
        {
            if (_logDelegate == null) { return; }
            _logDelegate("[TestLibraryInterface]: " + message);
        }
    }
}
