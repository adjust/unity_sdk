namespace AdjustSdk
{
    [System.Serializable]
    public enum AdjustEnvironment
    {
        Sandbox,
        Production
    }

    public static class AdjustEnvironmentExtension
    {
        public static string ToLowercaseString(this AdjustEnvironment adjustEnvironment)
        {
            switch (adjustEnvironment)
            {
                case AdjustEnvironment.Sandbox:
                    return "sandbox";
                case AdjustEnvironment.Production:
                    return "production";
                default:
                    return "unknown";
            }
        }
    }
}
