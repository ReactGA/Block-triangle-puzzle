using UnityEngine;
using DentedPixel;
public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject MenuBackground, GameBackground;
    [SerializeField] GameObject Menu, Stage, Game;//Pages
    [SerializeField] GameObject AdsButton,Startbutton;
    [SerializeField] Transform SoundBtn;
    [SerializeField] float tweenTime = 0.5f;
    private void Awake()
    {
        GotoMenu();
    }
    private void Start()
    {
        SetSoundVisual(PlayerPrefs.GetInt("Sound"));
        AdsButton.SetActive(PlayerPrefs.GetInt("Ads") == 0);
        LeanTween.scale(Startbutton,Startbutton.transform.localScale * 1.1f,0.5f).setLoopPingPong();
    }
    void SetSoundVisual(int On = 0)
    {
        SoundBtn.GetChild(0).gameObject.SetActive(On == 0);
        SoundBtn.GetChild(1).gameObject.SetActive(On == 1);
    }
    public void SoundToogle()
    {
        PlayClick();
        var value = AudioManager.SoundToogle();
        SetSoundVisual(value);
    }
    public void RemoveAds()
    {
        PlayClick();
        var str = "Proceed to remove ads with sms?";
        /* Purchase.Instance.ShowPurchaseDialogue(OnPurchasedRemoveAds,str,PaymentDescription.AdsRemoval);
        AppFlyerRef.LogRemoveAdsClick(); */
    }
    public void GotoMenu()
    {
        PlayClick();
        GameBackground.SetActive(false);
        Stage.SetActive(false);
        Game.SetActive(false);
        MenuBackground.SetActive(true);
        Menu.SetActive(true);
    }
    public void PlayGame()
    {
        // AppFlyerRef.InstallLog();
        PlayClick();
        GameBackground.SetActive(false);
        Menu.SetActive(false);
        Game.SetActive(false);
        MenuBackground.SetActive(true);
        Stage.SetActive(true);
    }
    public void SelectLevel(Transform t)
    {
        //Check level locked
        if (t.GetChild(1).gameObject.activeInHierarchy) return;
        PlayClick();
        MenuBackground.SetActive(false);
        Stage.SetActive(false);
        Game.SetActive(true);
        GameBackground.SetActive(true);
        GameManager.Instance.PlayLevel(int.Parse(t.name));
    }
    public void SelectLevel(int t)
    {
        // PlayClick();
        MenuBackground.SetActive(false);
        Stage.SetActive(false);
        Game.SetActive(true);
        GameBackground.SetActive(true);
        GameManager.Instance.PlayLevel(t);
    }

    #region Helpers

    public void OnPurchasedRemoveAds()
    {
        PlayerPrefs.SetInt("Ads", 1);
        AdsButton.SetActive(false);
        /* AppFlyerRef.LogPurchasedRemoveAds();
        Purchase.Instance.OnPaymentSucess -= OnPurchasedRemoveAds; */
    }

    void PlayClick()
    {
        AudioManager.PlaySound(Sounds.click);
    }

    #endregion
}
