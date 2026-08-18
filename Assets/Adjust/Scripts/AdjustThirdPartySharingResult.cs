namespace AdjustSdk
{
    public class AdjustThirdPartySharingResult
    {
        public string ThirdPartySharingSettingsJson { get; set; }

        public AdjustThirdPartySharingResult() {}

        public AdjustThirdPartySharingResult(string thirdPartySharingSettingsJson)
        {
            this.ThirdPartySharingSettingsJson = thirdPartySharingSettingsJson;
        }
    }
}
