using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    int CompletedLevel;
    bool Morelevel;
    [SerializeField] float MorebuttonSpacing = 120, MoreXoffSet = 5;
    [SerializeField] Transform scrollRect;
    [SerializeField] GameObject levelbtnPrefab, MorelevelsBtn;
    [SerializeField] UIManager uiManager;
    private void OnEnable()
    {
        SetLevels();
    }
    void SetLevels()
    {
        CompletedLevel = PlayerPrefs.GetInt("Completed");
        // Morelevel = PlayerPrefs.GetInt("Morelevels") == 1;
        PopulateButtons();
    }

    void PopulateButtons()
    {
        MorelevelsBtn.SetActive(false);
        var Rect = scrollRect.GetComponent<ScrollRect>();
        var dynamic = scrollRect.GetComponent<DynamicScrollRect>();
        if (Rect.content.childCount > 0)
        {
            foreach (Transform t in Rect.content)
            {
                Destroy(t.gameObject);
            }
            Rect.content.sizeDelta = new Vector2(Rect.content.sizeDelta.x, 50);
        }
        for (int i = 0; i < 50; i++)
        {
            var g = dynamic.AddMorebuttonsGrid(Rect, levelbtnPrefab, 5);
            g.transform.name = (i + 1).ToString();
            g.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
            g.transform.GetChild(1).gameObject.SetActive((i + 1) > (CompletedLevel + 1));
            g.GetComponent<Button>().onClick.AddListener(() => uiManager.SelectLevel(g.transform));

        }
        /* if (!Morelevel)
        {
            MorelevelsBtn.SetActive(true);
            ScrollPage();
        } 
        else
        { */
        for (int i = 50; i < 100; i++)
        {
            var g = dynamic.AddMorebuttonsGrid(Rect, levelbtnPrefab, 5);
            g.transform.name = (i + 1).ToString();
            g.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
            g.transform.GetChild(1).gameObject.SetActive((i + 1) > (CompletedLevel + 1));
            g.GetComponent<Button>().onClick.AddListener(() => uiManager.SelectLevel(g.transform));
        }

        Rect.content.sizeDelta -= new Vector2(0, 100);
        // }
    }

    public void ScrollPage()
    {
        if (Morelevel) return;
        var scroll = scrollRect.GetComponent<ScrollRect>().content;
        var child = scroll.GetChild(scroll.childCount - 2).position;
        MorelevelsBtn.transform.position = new Vector3(child.x - MoreXoffSet, child.y - MorebuttonSpacing, child.z);
    }

    public void UnlockMoreLevels()
    {
        var str = "Proceed to unlock more levels with sms?";
        // Purchase.Instance.ShowPurchaseDialogue(OnMoreLevelsUnlocked, str,PaymentDescription.UnlockMoreLevel);
        // AppFlyerRef.LogPurchaseMoreLevelsClick();
    }
    public void OnMoreLevelsUnlocked()
    {
        PlayerPrefs.SetInt("Morelevels", 1);
        SetLevels();
        // AppFlyerRef.LogPurchasedMoreLevels();
        // Purchase.Instance.OnPaymentSucess -= OnMoreLevelsUnlocked;
    }
}