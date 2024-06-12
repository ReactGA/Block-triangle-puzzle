using System.Collections;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(Load),0.5f);
    }
    void Load(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
