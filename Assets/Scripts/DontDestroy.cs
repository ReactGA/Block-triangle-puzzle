using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy Instance;
    public bool Started;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }else
            Destroy(this);
    }
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        Started = true;
    }
}
