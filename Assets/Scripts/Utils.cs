using System;
using UnityEngine;
using System.Collections;

public class Utils : MonoBehaviour
{
    static Utils Instance;
    private void Awake() {
        Instance = this;
    }
    public static void Invoke(float delay,Action method){
        Instance.StartCoroutine(Instance.PerformInvoke(delay,method));
    }
    IEnumerator PerformInvoke(float delay,Action method){
        yield return new WaitForSeconds(delay);
        method?.Invoke();
    }
}
