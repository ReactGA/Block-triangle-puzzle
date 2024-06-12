using UnityEngine;

public enum EffectType { Confetti, electric, Glassbreak }
public class EffectManager : MonoBehaviour
{
    static EffectManager Instance;
    void Awake()
    {
        Instance = this;
    }

    public static void PlayEffect(EffectType e)
    {
        foreach (Transform t in Instance.transform) { t.gameObject.SetActive(false); }

        Instance.transform.GetChild((int)e).gameObject.SetActive(true);
    }
    public static void StopEffect(EffectType? e = null)
    {
        if (e != null)
            Instance.transform.GetChild((int)e.Value).gameObject.SetActive(false);
        else
            foreach (Transform t in Instance.transform) { t.gameObject.SetActive(false); }
    }
}
