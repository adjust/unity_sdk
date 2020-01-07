using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
    public class AdjustEventFailure
    {
        public string Adid { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
        public string EventToken { get; set; }
        public string CallbackId { get; set; }
        public bool WillRetry { get; set; }
        public Dictionary<string, object> JsonResponse { get; set; }

        public AdjustEventFailure() {}

        public AdjustEventFailure(Dictionary<string, string> eventFailureDataMap)
        {
            if (eventFailureDataMap == null)
            {
                return;
            }

            Adid = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyAdid);
            Message = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyMessage);
            Timestamp = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyTimestamp);
            EventToken = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyEventToken);
            CallbackId = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyCallbackId);

            bool willRetry;
            if (bool.TryParse(AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyWillRetry), out willRetry))
            {
                WillRetry = willRetry;
            }

            string jsonResponseString = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyJsonResponse);
            var jsonResponseNode = JSON.Parse(jsonResponseString);
            if (jsonResponseNode != null && jsonResponseNode.AsObject != null)
            {
                JsonResponse = new Dictionary<string, object>();
                AdjustUtils.WriteJsonResponseDictionary(jsonResponseNode.AsObject, JsonResponse);
            }
        }

        public AdjustEventFailure(string jsonString)
        {
            var jsonNode = JSON.Parse(jsonString);
            if (jsonNode == null)
            {
                return;
            }

            Adid = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyAdid);
            Message = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyMessage);
            Timestamp = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyTimestamp);
            EventToken = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyEventToken);
            CallbackId = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyCallbackId);
            WillRetry = Convert.ToBoolean(AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyWillRetry));

            var jsonResponseNode = jsonNode[AdjustUtils.KeyJsonResponse];
            if (jsonResponseNode == null)
            {
                return;
            }
            if (jsonResponseNode.AsObject == null)
            {
                return;
            }

            JsonResponse = new Dictionary<string, object>();
            AdjustUtils.WriteJsonResponseDictionary(jsonResponseNode.AsObject, JsonResponse);
        }

        public void BuildJsonResponseFromString(string jsonResponseString)
        {
            var jsonNode = JSON.Parse(jsonResponseString);
            if (jsonNode == null)
            {
                return;
            }

            JsonResponse = new Dictionary<string, object>();
            AdjustUtils.WriteJsonResponseDictionary(jsonNode.AsObject, JsonResponse);
        }

        public string GetJsonResponse()
        {
            return AdjustUtils.GetJsonResponseCompact(JsonResponse);
        }
    }
}
