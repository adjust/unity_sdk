using AdjustSdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AdjustUnityWP
{
    public class AdjustWP
    {
        public static void AppDidLaunch(string appToken, string sEnvironment, string sdkPrefix, string sLogLevel, bool eventBuffering)
        {
            Adjust.AppDidLaunch(appToken);

            if ("sandbox" == sEnvironment)
                Adjust.SetEnvironment(AdjustEnvironment.Sandbox);
            if ("production" == sEnvironment)
                Adjust.SetEnvironment(AdjustEnvironment.Production);

            Adjust.SetSdkPrefix(sdkPrefix);

            var logLevel = AdjustSdk.ActivityKindUtil.FromString(sLogLevel);
            Adjust.SetLogLevel((LogLevel)logLevel);

            Adjust.SetEventBufferingEnabled(eventBuffering);
        }

        public static void AppDidActivate()
        {
            Adjust.AppDidActivate();
        }

        public static void AppDidDeactivate()
        {
            Adjust.AppDidDeactivate();
        }

        public static void TrackEvent(string eventToken, Dictionary<string, string> callbackParameters = null)
        {
            Adjust.TrackEvent(eventToken, callbackParameters);
        }

        public static void TrackRevenue(double amountInCents, string eventToken = null, Dictionary<string, string> callbackParameters = null)
        {
            Adjust.TrackRevenue(amountInCents, eventToken, callbackParameters);
        }

        public static void SetEnabled(bool enabled)
        {
            Adjust.SetEnabled(enabled);
        }

        public static bool IsEnabled()
        {
            return Adjust.IsEnabled();
        }

        /// <summary>
        /// Sets the response delegate that takes a ResponseData from a response delegate that takes a Json string
        /// </summary>
        /// <param name="responseDelegate">Response delegate that takes a Json string</param>
        public static void SetResponseDelegateString(Action<string> responseDelegate)
        {
            // function to convert a ResponseData object to a Json String
            Func<ResponseData, string> convertResponseDataToString = (responseData => JsonConvert.SerializeObject(responseData.ToDic()));

            // Convert the response delegate from taking a string, to taking a ResponseData using a function that converts from ResponseData to string
            Action<ResponseData> responseDelegateConverted = (responseData => responseDelegate(convertResponseDataToString(responseData)));

            // Sets the converted response delegate that takes the ResponseData
            Adjust.SetResponseDelegate(responseDelegateConverted);
        }
    }
}