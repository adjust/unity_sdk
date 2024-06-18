using System.Collections.Generic;

namespace AdjustSdk
{
    public class AdjustPurchaseVerificationResult
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string VerificationStatus { get; set; }

        public AdjustPurchaseVerificationResult() {}

        public AdjustPurchaseVerificationResult(string jsonString)
        {
            var jsonNode = JSON.Parse(jsonString);
            if (jsonNode == null)
            {
                return;
            }

            string strCode = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyCode);
            this.Code = int.Parse(strCode);
            this.Message = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyMessage);
            this.VerificationStatus = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyVerificationStatus);
        }
    }
}
