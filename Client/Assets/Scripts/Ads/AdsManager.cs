using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleMobileAds.Api;
using System;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    bool isTestAds = true;

    private InterstitialAd ads_Interstitial_AfterGameOver; // After Game Over Insterstitial
    private RewardedAd ads_Reward_AfterGameOver; // After Game Over Rewarded

    #region -- Interstitial Ads (After Gameover) --

    string adUnitId_Interstitial_AfterGameOver;

    const string interstitialAdID_afterGameOver_Android = "ca-app-pub-1021255306046408/5062113930";
    const string interstitialAdID_afterGameOver_IOS = "ca-app-pub-1021255306046408/5992052226";

    const string TEST_interstitialAds_Android = "ca-app-pub-3940256099942544/1033173712";
    const string TEST_interstitialAds_IOS = "ca-app-pub-3940256099942544/4411468910";

    #endregion


    #region -- Reward Ads (After Gameover) --

    string adUnitId_Reward_afterGameOver;

    const string RewardAdId_AfterGameOver_Android = "ca-app-pub-1021255306046408/3534913849";
    const string RewardAdId_AfterGameOver_IOS = "ca-app-pub-1021255306046408/6795539993";

    const string TEST_RewardAdId_AfterGameOver_Android = "ca-app-pub-3940256099942544/5224354917";
    const string TEST_RewardAdId_AfterGameOver_IOS = "ca-app-pub-3940256099942544/1712485313";

    public static Action onEarnedByRewardAd;

    #endregion

    AdRequest request_Interstitial_afterGameOver;
    AdRequest request_Rewarded_afterGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {

        MobileAds.Initialize((InitializationStatus initStatus) => {
            Debug.Log("MoblieAds Init: " + initStatus);
        });

        CreateAndLoaded_InterstitialAds_AfterGameOver();
        CreateAndLoaded_RewardedAds_AfterGameOver();
    }


    void CreateAndLoaded_InterstitialAds_AfterGameOver()
    {

#if UNITY_ANDROID
        if (isTestAds)
            adUnitId_Interstitial_AfterGameOver = TEST_interstitialAds_Android;
        else
            adUnitId_Interstitial_AfterGameOver = interstitialAdID_afterGameOver_Android;

#elif UNITY_IOS
        if (isTestAds)
            adUnitId_Interstitial_AfterGameOver = TEST_interstitialAds_IOS;
        else
            adUnitId_Interstitial_AfterGameOver = interstitialAdID_afterGameOver_IOS;
#endif


        // Initialize an InterstitialAd. - After Game Over
        this.ads_Interstitial_AfterGameOver = new InterstitialAd(adUnitId_Interstitial_AfterGameOver);

        // Called when an ad request has successfully loaded.
        this.ads_Interstitial_AfterGameOver.OnAdLoaded += HandleOnAdLoaded_Interstitial_AfterGameOver;
        // Called when an ad request failed to load.
        this.ads_Interstitial_AfterGameOver.OnAdFailedToLoad += HandleOnAdFailedToLoad_Interstitial_AfterGameOver;
        // Called when an ad is shown.
        this.ads_Interstitial_AfterGameOver.OnAdOpening += HandleOnAdOpening_Interstitial_AfterGameOver;
        // Called when the ad is closed.
        this.ads_Interstitial_AfterGameOver.OnAdClosed += HandleOnAdClosed_Interstitial_AfterGameOver;

        // Create an empty ad request.
        request_Interstitial_afterGameOver = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.ads_Interstitial_AfterGameOver.LoadAd(request_Interstitial_afterGameOver);

    }

    void CreateAndLoaded_RewardedAds_AfterGameOver()
    {

#if UNITY_ANDROID
        if (isTestAds)
            adUnitId_Reward_afterGameOver = TEST_RewardAdId_AfterGameOver_Android;
        else
            adUnitId_Reward_afterGameOver = RewardAdId_AfterGameOver_Android;

#elif UNITY_IOS
        if (isTestAds)
            adUnitId_Reward_afterGameOver = TEST_RewardAdId_AfterGameOver_IOS;
        else
            adUnitId_Reward_afterGameOver = RewardAdId_AfterGameOver_IOS;
#endif

        this.ads_Reward_AfterGameOver = new RewardedAd(adUnitId_Reward_afterGameOver);

        // Called when an ad request has successfully loaded.
        this.ads_Reward_AfterGameOver.OnAdLoaded += HandleRewardedAdLoaded_AfterGameOver;
        // Called when an ad request failed to load.
        this.ads_Reward_AfterGameOver.OnAdFailedToLoad += HandleRewardedAdFailedToLoad_AfterGameOver;
        // Called when an ad is shown.
        this.ads_Reward_AfterGameOver.OnAdOpening += HandleRewardedAdOpening_AfterGameOver;
        // Called when an ad request failed to show.
        this.ads_Reward_AfterGameOver.OnAdFailedToShow += HandleRewardedAdFailedToShow_AfterGameOver;
        // Called when the user should be rewarded for interacting with the ad.
        this.ads_Reward_AfterGameOver.OnUserEarnedReward += HandleUserEarnedReward_AfterGameOver;
        // Called when the ad is closed.
        this.ads_Reward_AfterGameOver.OnAdClosed += HandleRewardedAdClosed;



        request_Rewarded_afterGameOver = new AdRequest.Builder().Build();
        this.ads_Reward_AfterGameOver.LoadAd(request_Rewarded_afterGameOver);
    }

    public void ShowAds_AfterGameOver()
    {
        if (this.ads_Interstitial_AfterGameOver.IsLoaded())
        {
            this.ads_Interstitial_AfterGameOver.Show();
        }
    }

    public void ShowAds_Rewarded_AfterGameOver()
    {
        if (this.ads_Reward_AfterGameOver.IsLoaded())
        {
            this.ads_Reward_AfterGameOver.Show();
        }
    }

    #region -- Interstitial AfterGameOver Implements --

    public void HandleOnAdLoaded_Interstitial_AfterGameOver(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad_Interstitial_AfterGameOver(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.LoadAdError.GetMessage());
    }

    public void HandleOnAdOpening_Interstitial_AfterGameOver(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpening event received");
    }

    public void HandleOnAdClosed_Interstitial_AfterGameOver(object sender, EventArgs args)
    {
        CreateAndLoaded_InterstitialAds_AfterGameOver();
        MonoBehaviour.print("HandleAdClosed event received");
    }

    #endregion


    #region -- Rewarded After GameOver Implements --

    private void HandleRewardedAdClosed(object sender, EventArgs e)
    {
        CreateAndLoaded_RewardedAds_AfterGameOver();
    }

    private void HandleUserEarnedReward_AfterGameOver(object sender, Reward reward)
    {
        string type = reward.Type;
        double amount = reward.Amount;

        onEarnedByRewardAd?.Invoke();
    }

    private void HandleRewardedAdFailedToShow_AfterGameOver(object sender, AdErrorEventArgs e)
    {

    }

    private void HandleRewardedAdOpening_AfterGameOver(object sender, EventArgs e)
    {

    }

    private void HandleRewardedAdFailedToLoad_AfterGameOver(object sender, AdFailedToLoadEventArgs e)
    {

    }

    private void HandleRewardedAdLoaded_AfterGameOver(object sender, EventArgs e)
    {

    }

    #endregion

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