using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleMobileAds.Api;
using System;

public class AdsManager : MonoBehaviour
{
    bool isTestAds = true;

    private InterstitialAd ads_AfterGameOver; // After Game Over

    string adUnitId_afterGameOver;

    const string afterGameOver_ADID_Android = "ca-app-pub-1021255306046408/5062113930";
    const string afterGameOver_ADID_IOS = "ca-app-pub-1021255306046408/5992052226";

    const string TEST_interstitialAds_Android = "ca-app-pub-3940256099942544/1033173712";
    const string TEST_interstitialAds_IOS = "ca-app-pub-3940256099942544/4411468910";

    AdRequest request_afterGameOver;

    void Start()
    {
#if UNITY_ANDROID
        if (isTestAds) adUnitId_afterGameOver = TEST_interstitialAds_Android;
        else adUnitId_afterGameOver = afterGameOver_ADID_Android;
#elif UNITY_IOS
        if (isTestAds) adUnitId_afterGameOver = TEST_interstitialAds_IOS;
        else adUnitId_afterGameOver = afterGameOver_ADID_IOS;
#endif

        MobileAds.Initialize((InitializationStatus initStatus) => {
            Debug.Log("MoblieAds Init: " + initStatus);
        });


        // Initialize an InterstitialAd. - After Game Over
        this.ads_AfterGameOver = new InterstitialAd(adUnitId_afterGameOver);

        // Called when an ad request has successfully loaded.
        this.ads_AfterGameOver.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.ads_AfterGameOver.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.ads_AfterGameOver.OnAdOpening += HandleOnAdOpening;
        // Called when the ad is closed.
        this.ads_AfterGameOver.OnAdClosed += HandleOnAdClosed;

        // Create an empty ad request.
        request_afterGameOver = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.ads_AfterGameOver.LoadAd(request_afterGameOver);
    }

    public void ShowAds_AfterGameOver()
    {
        if (this.ads_AfterGameOver.IsLoaded())
        {
            this.ads_AfterGameOver.Show();
        }
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.LoadAdError.GetMessage());
    }

    public void HandleOnAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpening event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        this.ads_AfterGameOver.LoadAd(request_afterGameOver);
        MonoBehaviour.print("HandleAdClosed event received");
    }
}


// Test Ads IDs
// - Android
// 앱 오프닝 광고	ca-app-pub-3940256099942544/3419835294
// 배너 광고	    ca-app-pub-3940256099942544/6300978111
// 전면 광고	    ca-app-pub-3940256099942544/1033173712
// 보상형 광고	    ca-app-pub-3940256099942544/5224354917
// 보상형 전면 광고	ca-app-pub-3940256099942544/5354046379
// 네이티브 광고	ca-app-pub-3940256099942544/2247696110

// - IOS
// 앱 오프닝 광고	ca-app-pub-3940256099942544/5662855259
// 배너 광고	    ca-app-pub-3940256099942544/2934735716
// 전면 광고	    ca-app-pub-3940256099942544/4411468910
// 보상형 광고	    ca-app-pub-3940256099942544/1712485313
// 보상형 전면 광고	ca-app-pub-3940256099942544/6978759866
// 네이티브 광고	ca-app-pub-3940256099942544/3986624511