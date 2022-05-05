namespace com.adjust.sdk
{
    [System.Serializable]
    public enum AdjustUrlStrategy
    {
        Default,
        DataResidencyEU,
        DataResidencyTK,
        DataResidencyUS,
        India,
        China,
    }

    public static class AdjustUrlStrategyExtension
    {
        public static string ToLowerCaseString(this AdjustUrlStrategy strategy)
        {
            switch (strategy)
            {
                case AdjustUrlStrategy.India: return "india";
                case AdjustUrlStrategy.China: return "china";
                case AdjustUrlStrategy.DataResidencyEU: return "data-residency-eu";
                case AdjustUrlStrategy.DataResidencyTK: return "data-residency-tr";
                case AdjustUrlStrategy.DataResidencyUS: return "data-residency-us";
                default: return string.Empty;
            }
        }
    }
}

