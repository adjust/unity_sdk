using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UrlStrategy
{
    Default,
    DataResidencyEU,
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
            default: return string.Empty;
        }
    }
} 

