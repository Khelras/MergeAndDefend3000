using UnityEngine;
using UnityEngine.UI;

public class RemoveAdsButton : MonoBehaviour
{
    [SerializeField] Button m_button;
    [SerializeField] GameObject m_removeAdsSection;

    void OnEnable()
    {
        Refresh();
        if (IAPManager.Instance != null) IAPManager.Instance.OnAdsRemoved += Refresh;
    }

    void OnDisable()
    {
        if (IAPManager.Instance != null) IAPManager.Instance.OnAdsRemoved -= Refresh;
    }

    void Refresh()
    {
        bool adsOn = AdManager.Instance != null && AdManager.Instance.IsAdsEnabled;
        m_removeAdsSection.gameObject.SetActive(adsOn); // Hides once ads are removed
    }

    public void OnClicked() => IAPManager.Instance?.BuyRemoveAds();
}