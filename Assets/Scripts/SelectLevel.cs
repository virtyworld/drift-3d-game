using UnityEngine;
using UnityEngine.UI;

public class SelectLevel : MonoBehaviour
{
    [SerializeField] private Toggle[] tracks;
    [SerializeField] private GameObject trackButtonPanel;

    private int currentTrack;

    private void Start()
    {
        currentTrack = PlayerPrefs.GetInt(PlayerPrefsVariables.playerChoosedTrack.ToString(), 0);
        Debug.Log(currentTrack);
        tracks[currentTrack].isOn = true;

        for (int i = 0; i < tracks.Length; i++)
        {
            int trackIndex = i;
            tracks[i].onValueChanged.AddListener((x) => { if (x) SelectTrack(trackIndex); });
            Debug.Log(i);
        }

        trackButtonPanel.SetActive(false);
    }

    private void SelectTrack(int value)
    {
        Debug.Log(value);
        PlayerPrefs.SetInt(PlayerPrefsVariables.playerChoosedTrack.ToString(), value);
        PlayerPrefs.Save();
    }
}