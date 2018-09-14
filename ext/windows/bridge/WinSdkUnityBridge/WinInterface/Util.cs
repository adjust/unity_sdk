using AdjustSdk;
using Newtonsoft.Json;
using System.Collections.Generic;

#if WIN_INTERFACE_10
namespace Win10Interface
#elif WIN_INTERFACE_81
namespace Win81Interface
#elif WIN_INTERFACE_WS
namespace WinWsInterface
#else
namespace WinInterface
#endif
{
    public class Util
    {
        private const string MESSAGE = "message";
        private const string TIMESTAMP = "timestamp";
        private const string ADID = "adid";
        private const string WILL_RETRY = "willRetry";
        private const string JSON_RESPONSE = "jsonResponse";
        private const string EVENT_TOKEN = "eventToken";
        private const string CALLBACK_ID = "callbackId";

        public static Dictionary<string, string> ToDictionary(AdjustEventSuccess adjustEvent)
        {
            var jsonResp = JsonConvert.SerializeObject(adjustEvent.JsonResponse);
            return new Dictionary<string, string>
            {
                {MESSAGE, adjustEvent.Message},
                {TIMESTAMP, adjustEvent.Timestamp},
                {ADID, adjustEvent.Adid},
                {EVENT_TOKEN, adjustEvent.EventToken},
                {JSON_RESPONSE, jsonResp},
                {CALLBACK_ID, adjustEvent.CallbackId }
            };
        }

        public static Dictionary<string, string> ToDictionary(AdjustEventFailure adjustEvent)
        {
            var jsonResp = JsonConvert.SerializeObject(adjustEvent.JsonResponse);
            return new Dictionary<string, string>
            {
                {MESSAGE, adjustEvent.Message},
                {TIMESTAMP, adjustEvent.Timestamp},
                {ADID, adjustEvent.Adid},
                {EVENT_TOKEN, adjustEvent.EventToken},
                {WILL_RETRY, adjustEvent.WillRetry.ToString()},
                {JSON_RESPONSE, jsonResp},
                {CALLBACK_ID, adjustEvent.CallbackId }
            };
        }

        public static Dictionary<string, string> ToDictionary(AdjustSessionSuccess adjustSession)
        {
            var jsonResp = JsonConvert.SerializeObject(adjustSession.JsonResponse);
            return new Dictionary<string, string>
            {
                {MESSAGE, adjustSession.Message},
                {TIMESTAMP, adjustSession.Timestamp},
                {ADID, adjustSession.Adid},
                {JSON_RESPONSE, jsonResp}
            };
        }

        public static Dictionary<string, string> ToDictionary(AdjustSessionFailure adjustSession)
        {
            var jsonResp = JsonConvert.SerializeObject(adjustSession.JsonResponse);
            return new Dictionary<string, string>
            {
                {MESSAGE, adjustSession.Message},
                {TIMESTAMP, adjustSession.Timestamp},
                {ADID, adjustSession.Adid},
                {WILL_RETRY, adjustSession.WillRetry.ToString()},
                {JSON_RESPONSE, jsonResp}
            };
        }
    }
}
