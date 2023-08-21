using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
    public class AdjustPurchaseVerificationInfo
    {
        #region Properties
        public int code { get; set; }
        public string message { get; set; }
        public string verificationStatus { get; set; }
        #endregion

        #region Constructors
        public AdjustPurchaseVerificationInfo()
        {
        }

        public AdjustPurchaseVerificationInfo(string jsonString)
        {
            var jsonNode = JSON.Parse(jsonString);

            if (jsonNode == null)
            {
                return;
            }

            string stringCode = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyCode);
            code = Int32.Parse(stringCode);
            message = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyMessage);
            verificationStatus = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyVerificationStatus);
        }
        #endregion
    }
}
