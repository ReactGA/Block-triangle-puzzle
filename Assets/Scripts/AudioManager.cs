using UnityEngine;

public enum Sounds { click, scrub, fireworks, blockPlace, 
blockWrong, blockexplode,electric,Glassbreak }
public class AudioManager : MonoBehaviour
{
    static AudioManager instance;
    [SerializeField] AudioSource BackgroundSource;
    [SerializeField] AudioSource GeneralSoundSource;
    // [SerializeField] AudioClip click, scrub, fireworks, blokPlace, blockWrong, blockexplode;
    [SerializeField] AudioClip[] audios;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        TryPlayBackgroundClip();
    }
    public static void PlaySound(Sounds s)
    {
        if (PlayerPrefs.GetInt("Sound") == 1) return;
        if(!instance) return;
        instance.GeneralSoundSource.clip = instance.audios[(int)s];
        instance.GeneralSoundSource.Play();
    }
    public static void TryPlayBackgroundClip()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            instance.BackgroundSource.Stop();
            return;
        }

        instance.BackgroundSource.Play();
    }

    public static int SoundToogle()
    {
        PlayerPrefs.SetInt("Sound", 1 - PlayerPrefs.GetInt("Sound"));
        TryPlayBackgroundClip();
        return PlayerPrefs.GetInt("Sound");
    }
}
