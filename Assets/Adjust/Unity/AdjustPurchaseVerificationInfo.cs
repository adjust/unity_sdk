using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
    public class AdjustPurchaseVerificationInfo
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string VerificationStatus { get; set; }

        public AdjustPurchaseVerificationInfo() {}

        public AdjustPurchaseVerificationInfo(string jsonString)
        {
            var jsonNode = JSON.Parse(jsonString);
            if (jsonNode == null)
            {
                return;
            }

            string strCode = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyCode);
            Code = Int32.Parse(strCode);
            Message = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyMessage);
            VerificationStatus = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyVerificationStatus);
        }
    }
}
