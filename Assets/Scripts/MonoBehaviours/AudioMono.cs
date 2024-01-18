using UnityEngine;

public class AudioMono : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        audioSource.volume =  PlayerPrefs.GetFloat(PlayerPrefsVariables.voume.ToString(), 0.395f);
    }
}