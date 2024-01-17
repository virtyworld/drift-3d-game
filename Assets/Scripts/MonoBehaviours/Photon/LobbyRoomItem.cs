using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyRoomItem : MonoBehaviourPunCallbacks
{
    [SerializeField] public TextMeshProUGUI roomName;
    [SerializeField] public TextMeshProUGUI playerCount;
    [SerializeField] public Button joinRoomButton;
}
