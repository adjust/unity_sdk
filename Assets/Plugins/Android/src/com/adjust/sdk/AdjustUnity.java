package com.adjust.sdk;

import java.util.Map;

import org.json.JSONObject;

import com.unity3d.player.UnityPlayer;

public class AdjustUnity {

    public static void setResponseDelegate(final String sceneName)
    {
        Adjust.setOnFinishedListener(new OnFinishedListener() {

            @Override
            public void onFinishedTracking(ResponseData responseData) {
                Map<String, String> responseDataMap = responseData.toDic();
                JSONObject responseDataJson = new JSONObject(responseDataMap);
                UnityPlayer.UnitySendMessage(sceneName, "getNativeMessage", responseDataJson.toString());
            }
        });
    }
}
