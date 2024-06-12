using UnityEngine;
using TMPro;

public class PurchaseDialogue : MonoBehaviour
{
    static PurchaseDialogue Instance;
    [SerializeField]
    GameObject DialogueObject, PurchaseRequestPage,
    ResumeRequestPage, ErrorPage, LoadingIndicator;
    [SerializeField] float tweenTime;
    private void Awake()
    {
        Instance = this;
    }
    void SetDialogue(GameObject dialogue)
    {
        DialogueObject.gameObject.SetActive(true);
        foreach (Transform t in DialogueObject.transform)
        {
            // LeanTween.moveLocalX(t.gameObject, 1000, tweenTime).setEaseInCirc().setOnComplete(() => t.gameObject.SetActive(false));
            t.gameObject.SetActive(false);
        }
        dialogue.SetActive(true);
        dialogue.transform.localPosition = new Vector3(1000, dialogue.transform.localPosition.y, 0);
        LeanTween.moveLocalX(dialogue, 0, tweenTime).setEaseInCirc();
    }

    public static void ShowPurchaseRequest(string message)
    {
        Instance.PurchaseRequest(message);
    }
    void PurchaseRequest(string message)
    {
        SetDialogue(PurchaseRequestPage);
        PurchaseRequestPage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
    }
    public static void ShowRequestError()
    {
        Instance.SetDialogue(Instance.ErrorPage);
    }
    public static void ShowResumeRequest()
    {
        Instance.SetDialogue(Instance.ResumeRequestPage);
    }

    public static void ShowLoading()
    {
        Instance.SetDialogue(Instance.LoadingIndicator);
    }

    public static void ClosePurchaseDialogue()
    {
        Instance.DialogueObject.SetActive(false);
    }
}
