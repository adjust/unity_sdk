using System;
using System.Collections.Generic;

namespace AdjustSdk
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

            this.Adid = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyAdid);
            this.Message = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyMessage);
            this.Timestamp = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyTimestamp);
            this.EventToken = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyEventToken);
            this.CallbackId = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyCallbackId);

            bool willRetry;
            if (bool.TryParse(AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyWillRetry), out willRetry))
            {
                this.WillRetry = willRetry;
            }

            string jsonResponseString = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyJsonResponse);
            var jsonResponseNode = JSON.Parse(jsonResponseString);
            if (jsonResponseNode != null && jsonResponseNode.AsObject != null)
            {
                this.JsonResponse = new Dictionary<string, object>();
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

            this.Adid = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyAdid);
            this.Message = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyMessage);
            this.Timestamp = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyTimestamp);
            this.EventToken = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyEventToken);
            this.CallbackId = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyCallbackId);
            this.WillRetry = Convert.ToBoolean(AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyWillRetry));

            var jsonResponseNode = jsonNode[AdjustUtils.KeyJsonResponse];
            if (jsonResponseNode == null)
            {
                return;
            }
            if (jsonResponseNode.AsObject == null)
            {
                return;
            }

            this.JsonResponse = new Dictionary<string, object>();
            AdjustUtils.WriteJsonResponseDictionary(jsonResponseNode.AsObject, JsonResponse);
        }

        public void BuildJsonResponseFromString(string jsonResponseString)
        {
            var jsonNode = JSON.Parse(jsonResponseString);
            if (jsonNode == null)
            {
                return;
            }

            this.JsonResponse = new Dictionary<string, object>();
            AdjustUtils.WriteJsonResponseDictionary(jsonNode.AsObject, JsonResponse);
        }

        public string GetJsonResponseAsString()
        {
            return AdjustUtils.GetJsonResponseCompact(JsonResponse);
        }
    }
}
