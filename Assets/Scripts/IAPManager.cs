using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    public static IAPManager Instance { get; private set; }

    public const string RemoveAdsId = "remove_ads"; // must match Play Console

    public event Action OnAdsRemoved; // UI listens to this
    public event Action<bool> OnRestoreFinished; // true = remove_ads found
    bool m_restoreRequested;
    public bool StoreReady { get; private set; }

    StoreController m_storeController;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    async void Start()
    {
        // Attempt to initialize Unity Services
        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("UnityServices initialised");
        }
        catch (Exception e)
        {
            Debug.Log("UnityServices failed: " + e);
            return;
        }

        await InitStore();
    }

    async System.Threading.Tasks.Task InitStore()
    {
        m_storeController = UnityIAPServices.StoreController();

        // Add event listeners for store events
        m_storeController.OnStoreDisconnected += OnStoreDisconnected;

        m_storeController.OnProductsFetched += OnProductsFetched;
        m_storeController.OnProductsFetchFailed += OnProductsFetchFailed;

        m_storeController.OnPurchasesFetched += OnPurchasesFetched;
        m_storeController.OnPurchasesFetchFailed += OnPurchasesFetchFailed;

        m_storeController.OnPurchasePending += OnPurchasePending;
        m_storeController.OnPurchaseConfirmed += OnPurchaseConfirmed;
        m_storeController.OnPurchaseFailed += OnPurchaseFailed;

        // Connect to the Store
        await m_storeController.Connect();

        var catalogProvider = new CatalogProvider();
        catalogProvider.AddProduct(RemoveAdsId, ProductType.NonConsumable);
        catalogProvider.FetchProducts(list => m_storeController.FetchProducts(list));
    }

    // Called by the Settings button
    public void BuyRemoveAds()
    {
        if (AdManager.Instance != null && !AdManager.Instance.IsAdsEnabled) return; // already removed
        if (!StoreReady)
        {
            Debug.Log("Store not ready yet");
            return;
        }

        m_storeController.PurchaseProduct(RemoveAdsId);
    }

    // Callbacks
    void OnProductsFetched(List<Product> products)
    {
        Debug.Log($"Products fetched: {products.Count}");
        StoreReady = true;
        m_storeController.FetchPurchases(); // restore previous purchases
    }

    void OnProductsFetchFailed(ProductFetchFailed failure)
        => Debug.Log($"Products fetch failed for {failure.FailedFetchProducts.Count} products: {failure.FailureReason}");

    void OnPurchasePending(PendingOrder order)
    {
        var product = GetFirstProductInOrder(order);
        if (product == null)
        {
            Debug.Log("Could not find product in order.");
            return;
        }

        if (product.definition.id == RemoveAdsId)
        {
            GrantRemoveAds();
        }

        Debug.Log($"Purchase complete - Product: {product.definition.id}");
        m_storeController.ConfirmPurchase(order);
    }

    void OnPurchaseConfirmed(Order order)
    {
        switch (order)
        {
            case ConfirmedOrder confirmed:
                Debug.Log($"Purchase confirmed: {GetFirstProductInOrder(confirmed)?.definition.id}");
                break;
            case FailedOrder failed:
                Debug.Log($"Confirmation failed: {failed.FailureReason} / {failed.Details}");
                break;
            default:
                Debug.Log("Unknown OnPurchaseConfirmed result.");
                break;
        }
    }

    void OnPurchaseFailed(FailedOrder order)
    {
        var product = GetFirstProductInOrder(order);
        Debug.Log($"Purchase failed - Product: '{product?.definition.id}', " +
                  $"Reason: {order.FailureReason}, Details: {order.Details}");
    }

    void OnPurchasesFetched(Orders orders)
    {
        bool found = false;

        foreach (var confirmed in orders.ConfirmedOrders)
        {
            if (GetFirstProductInOrder(confirmed)?.definition.id == RemoveAdsId)
            {
                found = true;
                GrantRemoveAds();
            }
        }

        if (m_restoreRequested)
        {
            m_restoreRequested = false;
            OnRestoreFinished?.Invoke(found);
        }
    }

    void OnPurchasesFetchFailed(PurchasesFetchFailureDescription failure)
    {
        Debug.Log("Fetch purchases failed: " + failure.Message);

        if (m_restoreRequested)
        {
            m_restoreRequested = false;
            OnRestoreFinished?.Invoke(false);
        }
    }

    void OnStoreDisconnected(StoreConnectionFailureDescription failure)
    {
        StoreReady = false;
        Debug.Log("Store disconnected: " + failure.Message);
    }

    // Helpers
    void GrantRemoveAds()
    {
        AdManager.Instance?.DisableAds();
        OnAdsRemoved?.Invoke();
    }

    Product GetFirstProductInOrder(Order order) => order.CartOrdered.Items().First()?.Product;

    public void RestorePurchases()
    {
        if (!StoreReady)
        {
            Debug.Log("Store not ready, cannot restore yet");
            OnRestoreFinished?.Invoke(false);
            return;
        }

        m_restoreRequested = true;
        m_storeController.FetchPurchases();
    }
}