using AdjustSdk.Pcl.IntegrationTesting;

namespace TestLibraryInterface
{
    public class AdjustTestOptionsDto
    {
        public string BaseUrl { get; set; }
        public string BasePath { get; set; }
        public string GdprUrl { get; set; }
        public string GdprPath { get; set; }
        public bool? Teardown { get; set; }
        public bool? DeleteState { get; set; }

        // default value => Constants.ONE_MINUTE;
        public long? TimerIntervalInMilliseconds { get; set; }
        // default value => Constants.ONE_MINUTE;
        public long? TimerStartInMilliseconds { get; set; }
        // default value => Constants.THIRTY_MINUTES;
        public long? SessionIntervalInMilliseconds { get; set; }
        // default value => Constants.ONE_SECOND;
        public long? SubsessionIntervalInMilliseconds { get; set; }

        public AdjustTestOptions ToAdjustTestOptions()
        {
            return new AdjustTestOptions
            {
                BasePath = this.BasePath,
                GdprPath = this.GdprPath,
                BaseUrl = this.BaseUrl,
                GdprUrl = this.GdprUrl,
                DeleteState = this.DeleteState,
                SessionIntervalInMilliseconds = this.SessionIntervalInMilliseconds,
                SubsessionIntervalInMilliseconds = this.SubsessionIntervalInMilliseconds,
                Teardown = this.Teardown,
                TimerIntervalInMilliseconds = this.TimerIntervalInMilliseconds,
                TimerStartInMilliseconds = this.TimerStartInMilliseconds
            };
        }
    }
}
