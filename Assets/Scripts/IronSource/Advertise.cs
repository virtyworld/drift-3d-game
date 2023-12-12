using System;
using UnityEngine;
using UnityEngine.UI;

public class Advertise : MonoBehaviour
{
    // [SerializeField] private Lvl lvl;
    [SerializeField] private Button getX2Button;
    [SerializeField] private Button closeAdButton;
    
    private const string APP_KEY = "1cd38540d";
    private bool isInitialized;

    private void Awake()
    {
        IronSource.Agent.init (APP_KEY);
    }

    void Start()
    {
        EventImplement();
        
        IronSourceConfig.Instance.setClientSideCallbacks (true);
        string id = IronSource.Agent.getAdvertiserId ();
        Debug.Log ("unity-script: IronSource.Agent.getAdvertiserId : " + id);
        
        Debug.Log ("unity-script: IronSource.Agent.validateIntegration");
        IronSource.Agent.validateIntegration ();


        IronSource.Agent.loadRewardedVideo();
        getX2Button.onClick.AddListener(LoadAndShowBanner);
        closeAdButton.onClick.AddListener(HideBanner);
    }
    private void EventImplement()
    {
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;
    }
    void OnApplicationPause (bool isPaused)
    {
        Debug.Log ("unity-script: OnApplicationPause = " + isPaused);
        IronSource.Agent.onApplicationPause (isPaused);
    }
   
    
    private void LoadAndShowBanner()
    {
        Debug.Log ("unity-script: ShowRewardedVideoButtonClicked");
        if (IronSource.Agent.isRewardedVideoAvailable ()) {
            IronSource.Agent.showRewardedVideo ();
        } else {
            Debug.Log ("unity-script: IronSource.Agent.isRewardedVideoAvailable - False");
        }
        
        //lvl.MultiplyPoints();
    }
    public void OnRewardedVideoAdEnded()
    {
        // Здесь можно добавить код для награды игрока после просмотра видео
        Debug.Log("Награда за просмотр видео получена!");
        HideBanner();
    }
    private void HideBanner()
    {
        Debug.Log("HideBanner");
        IronSource.Agent.hideBanner();
    }
    
    private void OnDestroy()
    {
        HideBanner();
    }
    
    
/************* RewardedVideo AdInfo Delegates *************/
// Indicates that there’s an available ad.
// The adInfo object includes information about the ad that was loaded successfully
// This replaces the RewardedVideoAvailabilityChangedEvent(true) event
    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo) {
        bool available = IronSource.Agent.isRewardedVideoAvailable();
        Debug.Log("RewardedVideoOnAdAvailable "+available);
    }
// Indicates that no ads are available to be displayed
// This replaces the RewardedVideoAvailabilityChangedEvent(false) event
    void RewardedVideoOnAdUnavailable() {
    }
// The Rewarded Video ad view has opened. Your activity will loose focus.
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo){
    }
// The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo){
    }
// The user completed to watch the video, and should be rewarded.
// The placement parameter will include the reward data.
// When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo){
        Debug.Log("RewardedVideoOnAdRewardedEvent");
    }
// The rewarded video ad was failed to show.
    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo){
    }
// Invoked when the video ad was clicked.
// This callback is not supported by all networks, and we recommend using it only if
// it’s supported by all networks you included in your build.
    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo){
    }

}
