using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AudioMenu : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Slider slider;
    

    private void Start()
    {
        audioSource.volume =  PlayerPrefs.GetFloat(PlayerPrefsVariables.voume.ToString(), 0.395f);
       
        if (slider!=null) slider.value = audioSource.volume ;
    }

    private void FixedUpdate()
    {
        if (!audioSource.isPlaying)
        {
            PlayRandomSound();
        }
        
            ChangeVolume();

    }
    private void PlayRandomSound()
    {
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.Play();
    }

    private void ChangeVolume()
    {
        if (slider!=null && audioSource.volume!=slider.value)
        {
            audioSource.volume = slider.value;
            SaveSettings();
        }
        
    }

    private void SaveSettings()
    {
       PlayerPrefs.SetFloat(PlayerPrefsVariables.voume.ToString(),slider.value); 
       PlayerPrefs.Save();
    }
}
