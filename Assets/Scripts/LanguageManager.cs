using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class LanguageManager : MonoBehaviour
{
    static LanguageManager instance;
    public Transform Selector;
    public GameObject LanguagePanel;
    [SerializeField] List<TranslationSetter> UITextSetter;
    Dictionary<string, TranslationModel> dictModel = new Dictionary<string, TranslationModel>();

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt("SetLanguage") == 0)
        {
            LanguagePanel.SetActive(true);
        }
        var index = PlayerPrefs.GetInt("LanguageIndex");
        Transform t = Selector.parent.GetChild(index + 1);
        Selector.transform.position = new Vector3(Selector.transform.position.x,
        t.position.y, Selector.transform.position.z);

        for (int i = 0; i < UITextSetter.Count; i++)
        {
            if(!UITextSetter[i].UITextRef) continue;
            UITextSetter[i].englishRef = UITextSetter[i].UITextRef.text;
            dictModel.Add(UITextSetter[i].englishRef,UITextSetter[i].model);
        }

        ChangeAppLang();
    }
    public void ChangeLanguage(Transform t)
    {
        t.TryGetComponent(out LanguageModel model);
        if (!model) return;
        PlayerPrefs.SetInt("LanguageIndex", model.languageModel.Index);
        Selector.transform.position = new Vector3(Selector.transform.position.x,
        t.position.y, Selector.transform.position.z);
        ChangeAppLang();
    }
    public void ContinueGame()
    {
        LanguagePanel.SetActive(false);
        if (PlayerPrefs.GetInt("SetLanguage") == 0)
            PlayerPrefs.SetInt("SetLanguage", 1);
    }
    void ChangeAppLang()
    {
        for (int i = 0; i < UITextSetter.Count; i++)
        {
            if(!UITextSetter[i].UITextRef) continue;
            UITextSetter[i].UITextRef.text = GetWordInAppLang(UITextSetter[i].englishRef);
        }
    }

    public static string GetWordInAppLang(string engRef, int index = -1)
    {
        if (index == -1) index = PlayerPrefs.GetInt("LanguageIndex");
        if (index == 0) return engRef;
        else return instance.TranslationString(engRef, index);
    }

    string TranslationString(string engRef, int index)
    {
        if (dictModel.Count == 0) return engRef;
        if (dictModel.ContainsKey(engRef))
        {
            if (index == 1) return dictModel[engRef].FrenchWord;
            if (index == 2) return dictModel[engRef].GermanWord;
            if (index == 3) return dictModel[engRef].ItalianWord;
            if (index == 4) return dictModel[engRef].PortugueseWord;
            if (index == 5) return dictModel[engRef].RussianWord;
            if (index == 6) return dictModel[engRef].SpanishWord;
        }
        return engRef;

    }
}

[System.Serializable]
public class TranslationModel
{
    public string EnglishWord;
    public string FrenchWord;
    public string GermanWord;
    public string ItalianWord;
    public string PortugueseWord;
    public string RussianWord;
    public string SpanishWord;
}
[System.Serializable]
public class TranslationSetter
{
    public TextMeshProUGUI UITextRef;
    [HideInInspector] public string englishRef;
    public TranslationModel model;
}
