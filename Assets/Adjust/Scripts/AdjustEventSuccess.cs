using System;
using System.Collections.Generic;

namespace AdjustSdk
{
    public class AdjustEventSuccess
    {
        public string Adid { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
        public string EventToken { get; set; }
        public string CallbackId { get; set; }
        public Dictionary<string, object> JsonResponse { get; set; }

        public AdjustEventSuccess() {}

        public AdjustEventSuccess(Dictionary<string, string> eventSuccessDataMap)
        {
            if (eventSuccessDataMap == null)
            {
                return;
            }

            this.Adid = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyAdid);
            this.Message = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyMessage);
            this.Timestamp = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyTimestamp);
            this.EventToken = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyEventToken);
            this.CallbackId = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyCallbackId);

            string jsonResponseString = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyJsonResponse);
            var jsonResponseNode = JSON.Parse(jsonResponseString);
            if (jsonResponseNode != null && jsonResponseNode.AsObject != null)
            {
                this.JsonResponse = new Dictionary<string, object>();
                AdjustUtils.WriteJsonResponseDictionary(jsonResponseNode.AsObject, JsonResponse);
            }
        }

        public AdjustEventSuccess(string jsonString)
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
