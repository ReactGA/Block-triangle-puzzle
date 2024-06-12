using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] SolutionGenerator solution;
    [SerializeField] UIManager manager;
    [SerializeField] AdsCaller adsCaller;
    [SerializeField] LevelsGenerator levelGen;
    [SerializeField] TextMeshProUGUI HintText, levelText;
    [SerializeField] Button HintBtn;
    [SerializeField] Transform SoundBtn, CompletedPanel, RatePanel, brush;
    public Transform gridTilesBG, TileBackground, gridBackground;
    bool ShowAds, JustStarted, UsedHint;
    int currentlevel;
    Vector2 CompletedPanelPos;
    [SerializeField] GameObject BGselectorPanel, GameBGsParents;
    [SerializeField] float maxlevel = 100;
    [SerializeField] GameObject diamondUIPrefabs;
    [SerializeField] Transform diamondUICounterPos;
    [SerializeField] Camera UICam;
    [SerializeField] RectTransform mCanvas;
    [SerializeField]GameObject HintGetDialgue;
    private void Awake()
    {
        Instance = this;
        if (PlayerPrefs.GetInt("AwardedHint") == 0)
        {
            PlayerPrefs.SetInt("HintCount", 3);
            PlayerPrefs.SetInt("AwardedHint", 1);
        }
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt("Restart") > 0)
        {
            manager.PlayGame();
            manager.SelectLevel(PlayerPrefs.GetInt("Restart"));
            PlayerPrefs.SetInt("Restart", 0);
        }
        if (PlayerPrefs.GetInt("Selection") > 0)
        {
            manager.PlayGame();
            PlayerPrefs.SetInt("Selection", 0);
        }

        SetSoundVisual(PlayerPrefs.GetInt("Sound"));
        HintText.text = (PlayerPrefs.GetInt("HintCount")).ToString();
        // diamondUICounterPos.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetInt("DiamonEarned").ToString();

        ShowAds = (PlayerPrefs.GetInt("Ads") == 0);
        CompletedPanelPos = CompletedPanel.transform.localPosition;
        Selectbackground(PlayerPrefs.GetInt("Background"));

        // AdsManager.ON_REWARDED_ADS_FINISHED += OnHintPurchased;
    }
    void OnEnable()
    {
        adsCaller.RewardGive += OnHintPurchased;
    }
    void Disable()
    {
        adsCaller.RewardGive -= OnHintPurchased;
    }
    void SetSoundVisual(int On = 0)
    {
        SoundBtn.GetChild(0).gameObject.SetActive(On == 0);
        SoundBtn.GetChild(1).gameObject.SetActive(On == 1);
    }
    public void SoundToogle()
    {
        AudioManager.PlaySound(Sounds.click);
        var value = AudioManager.SoundToogle();
        SetSoundVisual(value);
    }
    public void HintClicked()
    {
        if (PlayerPrefs.GetInt("HintCount") > 0)
        {
            UseHint();
            PlayerPrefs.SetInt("HintCount", PlayerPrefs.GetInt("HintCount") - 1);
            HintText.text = (PlayerPrefs.GetInt("HintCount")).ToString();
            HintBtn.interactable = false;
            UsedHint = true;
        }
        else
            PurchaseHint();
    }
    void HintDialogue()
    {
        var str = "Do you want use a hint point?";
        // Purchase.Instance.ShowPurchaseDialogue(OnHintPurchased, str,PaymentDescription.Null);
    }
    void UseHint()
    {
        var center = solution.GetHintCenter(currentlevel);
        var Tile = solution.GetHintTile(currentlevel);
        Destroy(Tile.gameObject.GetComponent<TileInstance>());
        Tile.transform.parent = gridBackground;
        Tile.transform.position = center;
        foreach (Transform tile in Tile.transform)
        {
            Destroy(tile.gameObject.GetComponent<TilePiece>());
            foreach (Transform t in tile.transform)
            {
                if (t.gameObject.activeInHierarchy)
                {
                    var col = t.GetComponent<SpriteRenderer>().color;
                    LeanTween.color(t.gameObject,
                     new Color(col.r, col.g, col.b, 0.6f), 0.65f).setLoopPingPong(5).setOnComplete(() =>
                        Destroy(Tile)
                    );
                }
            }
        }
    }
    void PurchaseHint()
    {
        HintGetDialgue.SetActive(true);
        HintGetDialgue.transform.localScale = Vector3.zero;
        LeanTween.scale(HintGetDialgue,Vector3.one,0.5f).setEaseOutBack();
        /* var str = "Out if Hints, Do you want to get 5 hintPoints via sms?";
        Purchase.Instance.ShowPurchaseDialogue(OnHintPurchased, str, PaymentDescription.HintPurchase);
        // AppFlyerRef.LogHintPurchaseClick(currentlevel, true); */
    }
    public void AcceptHintGet(){
        HintGetDialgue.SetActive(false);
        adsCaller.ShowRewardedAds();
    }
    public void OnHintPurchased()
    {
        PlayerPrefs.SetInt("HintCount", 3);
        HintText.text = (PlayerPrefs.GetInt("HintCount")).ToString();
        /*  Purchase.Instance.OnPaymentSucess -= OnHintPurchased;

        // AppFlyerRef.LogHintPurchase(currentlevel, true); */
    }
    public void GotoSelection()
    {
        // manager.PlayGame();
        PlayerPrefs.SetInt("Selection", 1);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void GotoMenu()
    {
        // manager.GotoMenu();
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void PlayLevel(int levelToPlay)
    {
        currentlevel = levelToPlay;
        LoadLevel();
    }
    public void OpenCloseBgSelector(float Pos)
    {
        LeanTween.moveLocalY(BGselectorPanel, Pos, 1).setEaseInBack();
    }
    public void Selectbackground(int i)
    {
        foreach (Transform t in GameBGsParents.transform)
        {
            int res;
            if (int.TryParse(t.name, out res))
                t.gameObject.SetActive(false);
        }
        GameBGsParents.transform.GetChild(i).gameObject.SetActive(true);
        PlayerPrefs.SetInt("Background", i);
    }
    bool completed;
    public void CompletedLevel()
    {
        if (!completed)
        {


            AudioManager.PlaySound(Sounds.blockexplode);
            // AppFlyerRef.LogHintUsage(currentlevel, UsedHint);
            HintBtn.gameObject.SetActive(false);
            Utils.Invoke(0.7f, () =>
            {
                StartCoroutine(SetCompletedActions());
            });
            completed = true;
        }

    }
    IEnumerator SetCompletedActions()
    {
        EffectManager.PlayEffect(EffectType.electric);
        AudioManager.PlaySound(Sounds.electric);

        foreach (Transform grid in gridTilesBG)
        {
            foreach (Transform t in grid)
            {
                Destroy(t.GetComponent<Collider2D>());
            }
        }
        foreach (Transform t in gridBackground)
        {
            Destroy(t.GetComponent<Collider2D>());
            // LeanTween.move(g,UICam.ViewportToWorldPoint(diamondUI.position),Random.Range(1,2.75f)).setOnComplete(()=> Destroy(g));
        }


        if (currentlevel > PlayerPrefs.GetInt("Completed"))
        {
            Utils.Invoke(0.5f, () => SpawnDiamond());
            Utils.Invoke(1.5f, () => SpawnDiamond());
            Utils.Invoke(2f, () => SpawnDiamond());
            Utils.Invoke(2.5f, () => SpawnDiamond());
        }

        if (PlayerPrefs.GetInt("Completed") < currentlevel)
            PlayerPrefs.SetInt("Completed", currentlevel);


        yield return new WaitForSeconds(3f);
        AudioManager.PlaySound(Sounds.Glassbreak);
        EffectManager.PlayEffect(EffectType.Glassbreak);


        if (gridTilesBG.childCount > 0)
        { foreach (Transform t in gridTilesBG) { Destroy(t.gameObject); } }
        if (gridBackground.childCount > 0)
            foreach (Transform t in gridBackground) { Destroy(t.gameObject); }

        yield return new WaitForSeconds(1f);
        EffectManager.PlayEffect(EffectType.Confetti);
        LeanTween.moveLocalX(CompletedPanel.gameObject, 0, 0.7f).setEaseInBack();
        AudioManager.PlaySound(Sounds.fireworks);
    }
    void SpawnDiamond()
    {
        var RandPos = new Vector3(Screen.width / 2 + Random.Range(-10.75f, 10.75f),
        -335 + Random.Range(-5.25f, 5.25f), 0);

        var g = Instantiate(diamondUIPrefabs, Vector3.one, Quaternion.identity/* Euler(0, 0, Random.Range(0, 360)) */, diamondUICounterPos);
        g.transform.localPosition = RandPos;

        LeanTween.move(g,
            diamondUICounterPos.position, 0.7f).setOnComplete(() =>
            {
                PlayerPrefs.SetInt("DiamonEarned", PlayerPrefs.GetInt("DiamonEarned") + 1);
                diamondUICounterPos.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetInt("DiamonEarned").ToString();
                LeanTween.scale(diamondUICounterPos.gameObject, diamondUICounterPos.localScale * 1.1f, 0.3f).setLoopPingPong(1).setOnComplete(() =>
                    diamondUICounterPos.localScale = Vector3.one);
                Destroy(g);
            });
    }
    Vector3 WorldToUI(Vector3 pos)
    {
        Vector2 adjustedPosition = UICam.WorldToScreenPoint(pos);

        adjustedPosition.x *= mCanvas.rect.width / (float)UICam.pixelWidth;
        adjustedPosition.y *= mCanvas.rect.height / (float)UICam.pixelHeight;

        adjustedPosition = adjustedPosition - mCanvas.sizeDelta / 2f;
        return adjustedPosition;
    }
    public void Restart()
    {
        JustStarted = false;
        completed = false;
        UsedHint = false;
        LoadLevel();
    }
    public void RestartCurrentLevel()
    {
        PlayerPrefs.SetInt("Restart", currentlevel);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void NextLevel()
    {
        completed = false;
        EffectManager.StopEffect(EffectType.Confetti);
        // var Rect = CompletedPanel.GetComponent<RectTransform>();
        CompletedPanel.transform.localPosition = CompletedPanelPos;//new Vector2(Screen.width * 2, Rect.position.y);
        if (currentlevel == maxlevel || (currentlevel == 50 && PlayerPrefs.GetInt("Morelevels") == 0))
        {
            GotoMenu();
            return;
        }
        currentlevel++;
        adsCaller.ShowInterstitialAds();
        LoadLevel();
        UsedHint = false;
    }
    public void LoadLevel()
    {
        HintBtn.interactable = true;
        HintBtn.gameObject.SetActive(true);
        if (!JustStarted) JustStarted = true;
        else
        {
            /* if ((currentlevel - 1) % 3 == 0 && PlayerPrefs.GetInt("Rate") == 0)
                LeanTween.moveLocalX(RatePanel.gameObject, 0, 0.7f).setEaseInBack();
            else
            {
                if (PlayerPrefs.GetInt("Ads") == 0 && currentlevel > 3) AdMob.ShowAds();
            }  */
            if ((currentlevel - 1) % 3 == 0 ){
                AdsManager.ShowMidWareAds();
            }
        }

        levelText.text = "Level " + currentlevel;
        if (gridTilesBG.childCount > 0)
            foreach (Transform t in gridTilesBG) { Destroy(t.gameObject); }

        levelGen.ShowNextLevel(currentlevel);
        solution.ShowNextLevel(currentlevel);
    }

    public void GetHintWithDiamond()
    {
        if (!HintBtn.gameObject.activeInHierarchy) return;
        if (PlayerPrefs.GetInt("DiamonEarned") > 10)
        {
            PlayerPrefs.SetInt("DiamonEarned", PlayerPrefs.GetInt("DiamonEarned") - 10);
            PlayerPrefs.SetInt("HintCount", PlayerPrefs.GetInt("HintCount") + 1);
            HintText.text = (PlayerPrefs.GetInt("HintCount")).ToString();
            diamondUICounterPos.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetInt("DiamonEarned").ToString();
            LeanTween.scale(HintBtn.gameObject, HintBtn.transform.localScale * 1.1f, 0.1f).setLoopPingPong(1);
        }
    }
    public void RatingClick(int val)
    {
        if (val == 0) RatePanel.transform.localPosition = CompletedPanelPos;
        else
        {
            PlayerPrefs.SetInt("Rate", 1);
            RatePanel.transform.localPosition = CompletedPanelPos;
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.gamsole");
        }
    }

    void LogEvent()
    {
        /* Dictionary<string, string> purchaseEvent = new Dictionary<string, string>();
        purchaseEvent.Add(AFInAppEvents.CURRENCY, "NGN");
        purchaseEvent.Add(AFInAppEvents.REVENUE, "100");
        purchaseEvent.Add(AFInAppEvents.QUANTITY, "1");
        AppFlyerRef.SendEvent("Hint_Purchase", purchaseEvent); */
    }
}
