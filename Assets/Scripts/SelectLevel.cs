using ExitGames.Client.Photon;
using Photon.Pun;
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
        tracks[currentTrack].isOn = true;

        for (int i = 0; i < tracks.Length; i++)
        {
            int trackIndex = i;
            tracks[i].onValueChanged.AddListener((x) => { if (x) SelectTrack(trackIndex); });
        }

        trackButtonPanel.SetActive(false);
    }

    private void SelectTrack(int value)
    {
        PlayerPrefs.SetInt(PlayerPrefsVariables.playerChoosedTrack.ToString(), value);
        PlayerPrefs.Save();
        SetRoomCustomProperties(value);
    }
    private void SetRoomCustomProperties(int index)
    {
        Hashtable customRoomProperties = new Hashtable();
        customRoomProperties.Add("MapIndex", index);
        if (PhotonNetwork.CurrentRoom!=null)PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);
    }
}