namespace AdjustSdk
{
    public class AdjustRemoteTrigger
    {
        public string Label { get; set; }
        public string Payload { get; set; }

        public AdjustRemoteTrigger()
        {
            this.Payload = "{}";
        }

        public AdjustRemoteTrigger(string jsonString)
            : this()
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                return;
            }

            var jsonNode = JSON.Parse(jsonString);
            if (jsonNode == null)
            {
                return;
            }

            this.Label = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyLabel);

            string payload = AdjustUtils.GetJsonString(jsonNode, AdjustUtils.KeyPayload);
            if (payload != null)
            {
                this.Payload = payload;
                return;
            }

            var payloadNode = jsonNode[AdjustUtils.KeyPayload];
            if (payloadNode != null)
            {
                this.Payload = payloadNode.ToString();
            }
        }

        public AdjustRemoteTrigger(string label, string payload)
            : this()
        {
            this.Label = label;
            this.Payload = string.IsNullOrEmpty(payload) ? "{}" : payload;
        }
    }
}
