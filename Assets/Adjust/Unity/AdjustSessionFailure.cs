using System;
using System.Collections.Generic;

namespace com.adjust.sdk {
    public class AdjustSessionFailure {
        #region Properties
        public string Adid { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }

        public bool WillRetry { get; set; }
        public Dictionary<string, object> JsonResponse { get; set; }
        #endregion

        #region Constructors
        public AdjustSessionFailure() {}
        
        public AdjustSessionFailure(string jsonString) {
            var jsonNode = JSON.Parse(jsonString);
            
            if (jsonNode == null) {
                return;
            }

            Adid = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyAdid);
            Message = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyMessage);
            Timestamp = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyTimestamp);
            WillRetry = Convert.ToBoolean(AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyWillRetry));
            
            var jsonResponseNode = jsonNode[AdjustUtils.KeyJsonResponse];

            if (jsonResponseNode == null) {
                return;
            }

            if (jsonResponseNode.AsObject == null) {
                return;
            }

            JsonResponse = new Dictionary<string, object>();
            AdjustUtils.WriteJsonResponseDictionary(jsonResponseNode.AsObject, JsonResponse);
        }
        #endregion

        #region Public methods
        public void BuildJsonResponseFromString(string jsonResponseString) {
            var jsonNode = JSON.Parse(jsonResponseString);
            
            if (jsonNode == null) {
                return;
            }

            JsonResponse = new Dictionary<string, object>();
            AdjustUtils.WriteJsonResponseDictionary(jsonNode.AsObject, JsonResponse);
        }
        
        public string GetJsonResponse() {
            return AdjustUtils.GetJsonResponseCompact(JsonResponse);
        }
        #endregion
    }
}
