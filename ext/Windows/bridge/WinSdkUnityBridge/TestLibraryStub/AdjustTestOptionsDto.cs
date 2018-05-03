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
        public long? TimerIntervalInMilliseconds { get; set; }
        public long? TimerStartInMilliseconds { get; set; }
        public long? SessionIntervalInMilliseconds { get; set; }
        public long? SubsessionIntervalInMilliseconds { get; set; }
    }
}
