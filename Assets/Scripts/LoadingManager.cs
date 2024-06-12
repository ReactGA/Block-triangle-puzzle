using System;
using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    static LoadingManager Instance;
    [SerializeField] GameObject LoadPanel;
    float loadTimer;
    Action onClosedLoading;
    private void Awake()
    {
        Instance = this;
    }
    public static void ShowLoading(float loadingTimer = 0, Action onClose = null)
    {
        Instance.LoadPanel.SetActive(true);
        Instance.onClosedLoading = onClose;
        if (loadingTimer > 0)
            Instance.Invoke(nameof(CloseLoading), loadingTimer);
    }
    public static void CloseLoading()
    {
        Instance.onClosedLoading?.Invoke();
        Instance.LoadPanel.SetActive(false);
    }
}
