using UnityEngine;
using UnityEngine.UI;

public class Advertise : MonoBehaviour
{
    // [SerializeField] private Lvl lvl;
    [SerializeField] private Button getX2Button;
    [SerializeField] private Button closeAdButton;
    
    private const string APP_KEY = "1cd38540d";
    private bool isInitialized;

    void Start()
    {
        EventImplement();
        IronSource.Agent.validateIntegration();
        IronSource.Agent.init(APP_KEY);

        IronSourceConfig.Instance.setClientSideCallbacks (true);
        string id = IronSource.Agent.getAdvertiserId ();
        Debug.Log ("unity-script: IronSource.Agent.getAdvertiserId : " + id);

        getX2Button.onClick.AddListener(LoadAndShowBanner);
        closeAdButton.onClick.AddListener(HideBanner);
    }
    private void EventImplement()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
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

    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo) {
        bool available = IronSource.Agent.isRewardedVideoAvailable();
        Debug.Log("RewardedVideoOnAdAvailable "+available);
    }

    void RewardedVideoOnAdUnavailable() {
    }

    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo){
    }

    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo){
    }

    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo){
        Debug.Log("RewardedVideoOnAdRewardedEvent");
    }

    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo){
    }

    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo){
    }

    private void SdkInitializationCompletedEvent()
    {
        IronSource.Agent.loadRewardedVideo();
        Debug.Log ("unity-script: IronSource.Agent.validateIntegration");
        Debug.Log("SdkInitializationCompletedEvent");
    }
}
//   TODO erroe when android resolve
// Gradle failed to fetch dependencies.
//
// Failed to run 'C:\Users\dima\unityProject\Drift 3D game\Temp\PlayServicesResolverGradle\gradlew.bat --no-daemon -b "C:\Users\dima\unityProject\Drift 3D game\Temp\PlayServicesResolverGradle\PlayServicesResolver.scripts.download_artifacts.gradle" "-PANDROID_HOME=C:/Program Files/Unity/Hub/Editor/2022.3.12f1/Editor/Data/PlaybackEngines/AndroidPlayer\SDK" "-PTARGET_DIR=C:\Users\dima\unityProject\Drift 3D game\Assets\Plugins\Android" "-PMAVEN_REPOS=https://android-sdk.is.com/;https://maven.google.com/" "-PPACKAGES_TO_COPY=com.ironsource.sdk:mediationsdk:7.6.0;com.google.android.gms:play-services-ads-identifier:18.0.1;com.google.android.gms:play-services-basement:18.1.0;com.ironsource.adapters:unityadsadapter:4.3.34;com.unity3d.ads:unity-ads:4.9.2" "-PUSE_JETIFIER=1" "-PDATA_BINDING_VERSION=7.1.2"'
// stdout:
//
// > Configure project :
// ANDROID_HOME: C:/Program Files/Unity/Hub/Editor/2022.3.12f1/Editor/Data/PlaybackEngines/AndroidPlayer\SDK
// MAVEN_REPOS: name=Google url=https://dl.google.com/dl/android/maven2/
// MAVEN_REPOS: name=maven url=https://dl.google.com/dl/android/maven2/
// MAVEN_REPOS: name=maven2 url=https://maven.google.com/
// MAVEN_REPOS: name=maven3 url=https://android-sdk.is.com/
// MAVEN_REPOS: name=MavenLocal url=file:/C:/Users/dima/.m2/repository/
// MAVEN_REPOS: name=BintrayJCenter url=https://jcenter.bintray.com/
// MAVEN_REPOS: name=MavenRepo url=https://repo.maven.apache.org/maven2/
// PACKAGES_TO_COPY: com.ironsource.sdk:mediationsdk:7.6.0
// PACKAGES_TO_COPY: com.google.android.gms:play-services-ads-identifier:18.0.1
// PACKAGES_TO_COPY: com.google.android.gms:play-services-basement:18.1.0
// PACKAGES_TO_COPY: com.ironsource.adapters:unityadsadapter:4.3.34
// PACKAGES_TO_COPY: com.unity3d.ads:unity-ads:4.9.2
// TARGET_DIR: C:\Users\dima\unityProject\Drift 3D game\Assets\Plugins\Android
// Resolution attempt 1: packages [com.google.android.gms:play-services-ads-identifier:18.0.1, com.google.android.gms:play-services-basement:18.1.0, com.unity3d.ads:unity-ads:4.9.2, com.ironsource.adapters:unityadsadapter:4.3.34, com.ironsource.sdk:mediationsdk:7.6.0]
// androidx.asynclayoutinflater:asynclayoutinflater conflicting due to package(s):
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.asynclayoutinflater:asynclayoutinflater:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.asynclayoutinflater:asynclayoutinflater:1.0.0
// - com.unity3d.ads:unity-ads:4.9.2/com.google.android.gms:play-services-cronet:18.0.1/com.google.android.gms:play-services-base:18.0.1/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.asynclayoutinflater:asynclayoutinflater:1.0.0
// - com.unity3d.ads:unity-ads:4.9.2/com.google.android.gms:play-services-cronet:18.0.1/com.google.android.gms:play-services-base:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.asynclayoutinflater:asynclayoutinflater:1.0.0
// - com.unity3d.ads:unity-ads:4.9.2/com.google.android.gms:play-services-cronet:18.0.1/com.google.android.gms:play-services-base:18.0.1/com.google.android.gms:play-services-tasks:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.asynclayoutinflater:asynclayoutinflater:1.0.0
// - com.unity3d.ads:unity-ads:4.9.2/com.google.android.gms:play-services-cronet:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.asynclayoutinflater:asynclayoutinflater:1.0.0
// - com.unity3d.ads:unity-ads:4.9.2/com.google.android.gms:play-services-cronet:18.0.1/com.google.android.gms:play-services-tasks:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.asynclayoutinflater:asynclayoutinflater:1.0.0
// androidx.coordinatorlayout:coordinatorlayout conflicting due to package(s):
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.coordinatorlayout:coordinatorlayout:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.coordinatorlayout:coordinatorlayout:1.0.0
// - com.unity3d.ads:unity-ads:4.9.2/com.google.android.gms:play-services-cronet:18.0.1/com.google.android.gms:play-services-base:18.0.1/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.coordinatorlayout:coordinatorlayout:1.0.0
// - com.unity3d.ads:unity-ads:4.9.2/com.google.android.gms:play-services-cronet:18.0.1/com.google.android.gms:play-services-base:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.coordinatorlayout:coordinatorlayout:1.0.0
// - com.unity3d.ads:unity-ads:4.9.2/com.google.android.gms:play-services-cronet:18.0.1/com.google.android.gms:play-services-base:18.0.1/com.google.android.gms:play-services-tasks:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.coordinatorlayout:coordinatorlayout:1.0.0
// - com.unity3d.ads:unity-ads:4.9.2/com.google.android.gms:play-services-cronet:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.coordinatorlayout:coordinatorlayout:1.0.0
// - com.unity3d.ads:unity-ads:4.9.2/com.google.android.gms:play-services-cronet:18.0.1/com.google.android.gms:play-services-tasks:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.coordinatorlayout:coordinatorlayout:1.0.0
// androidx.core:core conflicting due to package(s):
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.core:core:1.2.0
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.asynclayoutinflater:asynclayoutinflater:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.coordinatorlayout:coordinatorlayout:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.coordinatorlayout:coordinatorlayout:1.0.0/androidx.customview:customview:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.customview:customview:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.drawerlayout:drawerlayout:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.drawerlayout:drawerlayout:1.0.0/androidx.customview:customview:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.legacy:legacy-support-core-utils:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.legacy:legacy-support-core-utils:1.0.0/androidx.loader:loader:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.slidingpanelayout:slidingpanelayout:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.slidingpanelayout:slidingpanelayout:1.0.0/androidx.customview:customview:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.swiperefreshlayout:swiperefreshlayout:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.viewpager:viewpager:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.viewpager:viewpager:1.0.0/androidx.customview:customview:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-utils:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-utils:1.0.0/androidx.loader:loader:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-ads-identifier:18.0.1/com.google.android.gms:play-services-basement:18.0.0/androidx.fragment:fragment:1.0.0/androidx.loader:loader:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.core:core:1.2.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.asynclayoutinflater:asynclayoutinflater:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.coordinatorlayout:coordinatorlayout:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.coordinatorlayout:coordinatorlayout:1.0.0/androidx.customview:customview:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.customview:customview:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.drawerlayout:drawerlayout:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.drawerlayout:drawerlayout:1.0.0/androidx.customview:customview:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.legacy:legacy-support-core-utils:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.legacy:legacy-support-core-utils:1.0.0/androidx.loader:loader:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.slidingpanelayout:slidingpanelayout:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.slidingpanelayout:slidingpanelayout:1.0.0/androidx.customview:customview:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.swiperefreshlayout:swiperefreshlayout:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.viewpager:viewpager:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.viewpager:viewpager:1.0.0/androidx.customview:customview:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-utils:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-utils:1.0.0/androidx.loader:loader:1.0.0/androidx.core:core:1.0.0
// - com.google.android.gms:play-services-basement:18.1.0/androidx.fragment:fragment:1.0.0/androidx.loader:loader:1.0.0/androidx.core:core:1.0.0
// - com.unity3d.ads:unity-ads:4.9.2/androidx.activity:activity-ktx:1.7.1/androidx.activity:activity:[1.7.1]/androidx.core:core:1.8.0
// - com.unity3d.ads:unity-ads:4.9.2/androidx.activity:activity-ktx:1.7.1/androidx.activity:activity:[1.7.1]/androidx.lifecycle:lifecycle-viewmodel-savedstate:2.6.1/androidx.core:core-ktx:1.2.0/androidx.core:core:1.9.0
// - com.unity3d.ads:unity-ads:4.9.2/androidx.activity:activity-ktx:1.7.1/androidx.core:core-ktx:1.1.0/androidx.core:core:1.9.0
// - com.unity3d.ads:unity-ads:4.9.2/androidx.core:core-ktx:1.9.0/androidx.core:core:1.9.0
// - com.unity3d.ads:unity-ads:4.9.2/androidx.webkit:webkit:1.6.1/androidx.core:core:1.1.0
// - com.unity3d.ads:unity-ads:4.9.2/androidx.work:work-runtime-ktx:2.7.0/androidx.work:work-runtime:[2.7.0]/androidx.core:core:1.6.0
// - com.unity3d.ads:unity-ads:4.9.2/com.google.android.gms:play-services-cronet:18.0.1/com.google.android.gms:play-services-base:18.0.1/androidx.core:core:1.2.0
// - com.unity3d.ads:unity-ads:4.9.2/com.google.android.gms:play-services-cronet:18.0.1/com.google.android.gms:play-services-base:18.0.1/androidx.fragment:fragment:1.0.0/androidx.core:core:1.0.0
// - com.unity3d.ads:unity-ads:4.9.2/com.google.android.gms:play-services-cronet:18.0.1/com.google.android.gms:play-services-base:18.0.1/androidx.fragment:fragment:1.0.0/androidx.legacy:legacy-support-core-ui:1.0.0/androidx.asynclayoutinflater:asynclayoutinflater:1.0.0/androidx.core:core:1.0.0
// - com.unity3d.ads:unity-ads:4.9.2/com.google.android.gms:play-services-cronet:18.0.1/com.g<message truncated>