using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using UnityEngine;

public interface IAdUnit
{
    void Load();
    void Show();
    void Hide();
    void Destroy();
}

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }

    [SerializeField] bool m_adsEnabledInBuild = true; // Dev/Debug Kill Switch
    [SerializeField] string m_bannerId = "ca-app-pub-3940256099942544/6300978111";
    [SerializeField] string m_interstitialId = "ca-app-pub-3940256099942544/1033173712";
    [SerializeField] string m_rewardedId = "ca-app-pub-3940256099942544/5224354917";

    const string NO_ADS_KEY = "NoAds";
    BannerAdUnit m_banner;
    InterstitialAdUnit m_interstitial;
    RewardedAdUnit m_rewarded;
    bool m_initialised;

    bool m_bannerWanted;

    public bool IsAdsEnabled => m_adsEnabledInBuild && PlayerPrefs.GetInt(NO_ADS_KEY, 0) == 0;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (!IsAdsEnabled) return; // SDK never even starts 
        MobileAds.Initialize(_ =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                m_banner = new BannerAdUnit(m_bannerId);
                m_interstitial = new InterstitialAdUnit(m_interstitialId);
                m_rewarded = new RewardedAdUnit(m_rewardedId);

                m_banner.Load();
                m_interstitial.Load();
                m_rewarded.Load();

                m_initialised = true;

                // Show Banner if it was requested before the SDK finished initializing
                if (m_bannerWanted)
                {
                    m_banner.Show();
                }
            });
        });
    }

    public void ShowBanner()
    {
        m_bannerWanted = true;
        if (IsAdsEnabled && m_initialised)
        {
            m_banner.Show();
        }
    }

    public void HideBanner()
    {
        m_bannerWanted = false;
        if (m_initialised)
        { 
            m_banner.Hide();
        }
    }

    public void ShowInterstitial()
    {
        if (IsAdsEnabled && m_initialised)
        {
            m_interstitial.Show();
        }
    }

    public void ShowRewarded(Action onReward)
    {
        if (IsAdsEnabled && m_initialised && m_rewarded.IsReady)
        {
            m_rewarded.Show(onReward);
        }
    }

    // Call this from a "Remove Ads" purchase or settings toggle
    public void DisableAds()
    {
        // Save NO ADS state to PlayerPrefs
        PlayerPrefs.SetInt(NO_ADS_KEY, 1);
        PlayerPrefs.Save();

        if (m_initialised)
        {
            m_banner.Destroy();
            m_interstitial.Destroy();
        }
    }

    void OnDestroy()
    {
        m_banner?.Destroy();
        m_interstitial?.Destroy();
    }
}