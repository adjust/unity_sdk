namespace com.adjust.sdk
{
    public enum UrlStrategy
    {
        Default,
        DataResidencyEU,
        DataResidencyTK,
        DataResidencyUS,
        India,
        China,
    }

    public static class UrlStrategyExtension
    {
        public static string ToLowerCaseString(this UrlStrategy strategy)
        {
            switch (strategy)
            {
                case UrlStrategy.India: return "india";
                case UrlStrategy.China: return "china";
                case UrlStrategy.DataResidencyEU: return "data-residency-eu";
                case UrlStrategy.DataResidencyTK: return "data-residency-tr";
                case UrlStrategy.DataResidencyUS: return "data-residency-us";
                default: return string.Empty;
            }
        }
    }
}

