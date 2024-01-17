using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    [Header("Connect screen")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private GameObject progressLabel;
    [Header("Create room")]
    [SerializeField] private TMP_InputField inputRoomName;
    [SerializeField] private TMP_InputField maxPlayersPerRoom;
    [SerializeField] private Button createRoomButton;
    [SerializeField] private LobbyRoomItem roomItem;
    [SerializeField] private GameObject createRoomDir;
    [SerializeField] private Transform roomItemContainer;
    [Header("room")]
    [SerializeField] private GameObject roomDir;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private TextMeshProUGUI joinedRoomName;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button leaveRoom;
    [SerializeField] private TextMeshProUGUI playerNamePrefab;
    [SerializeField] private Transform playerNameDir;

    private string gameVersion = "1";
    private bool gameStarted;
    private bool isReadyForRoomOperations;
    private TypedLobby customLobby = new TypedLobby("customLobby", LobbyType.Default);
    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        InitializeUI();
        ConnectButtonListeners();
    }

   
    private void InitializeUI()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void ConnectButtonListeners()
    {
        createRoomButton.onClick.AddListener(CreateRoom);
        startGameButton.onClick.AddListener(StartGameOnClick);
        leaveRoom.onClick.AddListener(LeaveRoom);
        loginButton.onClick.AddListener(ConnectButtonOnClick);
    }
   
    private void ConnectButtonOnClick()
    {
        string playerNickname = playerName.text;

        if (!string.IsNullOrEmpty(playerNickname))
        {
            PhotonNetwork.NickName = playerNickname;
            Connect();
        }
        else
        {
            Debug.Log("Please enter a player name.");
        }
    }

    public void Connect()
    {
        progressLabel.SetActive(true);
        loginPanel.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        progressLabel.SetActive(false);
        roomDir.SetActive(true);
        joinedRoomName.text = "Room: " + PhotonNetwork.CurrentRoom.Name;
        playerCountText.text = string.Format("Players: {0}/{1}", PhotonNetwork.CurrentRoom.Players.Count, PhotonNetwork.CurrentRoom.MaxPlayers);

        if (!PhotonNetwork.IsMasterClient) startGameButton.gameObject.SetActive(false);
        
        UpdatePlayerList();
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        progressLabel.SetActive(false);
        Debug.Log("A new player has joined the room!");
        UpdatePlayerCount();
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("A player has left the room!");
        UpdatePlayerCount();
        UpdatePlayerList();
    }
    public override void OnLeftRoom()
    {
        roomDir.SetActive(false);
        createRoomDir.SetActive(true);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created successfully!");
        SetRoomCustomProperties();
        UpdatePlayerList();
    }
    public override void OnJoinedLobby()
    {
        cachedRoomList.Clear();
        base.OnJoinedLobby();
        Debug.Log("Joined Lobby");
        progressLabel.SetActive(false);
        createRoomDir.SetActive(true);
        isReadyForRoomOperations = true;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);
        base.OnRoomListUpdate(roomList);
        UpdateRoomList();
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
        base.OnLeftLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        cachedRoomList.Clear();
        base.OnDisconnected(cause);
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    private void CreateRoom()
    {
        if (isReadyForRoomOperations)
        {
            ValidateRoomCreationInput();
        }
        else
        {
            Debug.Log("Not ready for room operations yet.");
        }
    }
    private void ValidateRoomCreationInput()
    {
        if (int.TryParse(maxPlayersPerRoom.text, out int maxPlayers) && maxPlayers > 0)
        {
            if (maxPlayers > 20) maxPlayers = 20;
           
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayers;

            if (!string.IsNullOrEmpty(inputRoomName.text))
            {
                PhotonNetwork.CreateRoom(inputRoomName.text, roomOptions, TypedLobby.Default);
                createRoomDir.SetActive(false);
                progressLabel.SetActive(true);
            }
            else
            {
                Debug.Log("Room name is empty or null. Please enter a valid room name.");
            }
        }
        else
        {
            Debug.Log("Invalid max players value. Please enter a valid number of players.");
        }
    }
    private void StartGameOnClick()
    {
        if (!gameStarted && PhotonNetwork.IsMasterClient)
        {
            gameStarted = true;
            Debug.Log("Starting the game!");
            HideRoomPanel();
            PhotonNetwork.LoadLevel("Game");
        }
    }
    private void SetRoomCustomProperties()
    {
        int mapIndex = PlayerPrefs.GetInt(PlayerPrefsVariables.playerChoosedTrack.ToString(), 0);
        Hashtable customRoomProperties = new Hashtable();
        customRoomProperties.Add("MapIndex", mapIndex); 
        PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);
    }
    private void LeaveRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ConfigureRoomForLeaving();
        }
        else
        {
            PhotonNetwork.LeaveRoom();
        }
    }
    private void ConfigureRoomForLeaving()
    {
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.RemovedFromList = true;
        PhotonNetwork.LeaveRoom();
    }
    private void UpdateRoomList()
    {
        foreach (Transform child in roomItemContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<string, RoomInfo> room in cachedRoomList)
        {
            LobbyRoomItem newRoomItem = Instantiate(roomItem, roomItemContainer);
            newRoomItem.roomName.text = room.Value.Name;
            newRoomItem.playerCount.text = room.Value.PlayerCount + "/" + room.Value.MaxPlayers;
            newRoomItem.joinRoomButton.onClick.AddListener(() => JoinRoom(room.Value.Name));
        }
    }
    private void JoinRoom(string roomName)
    {
        if (isReadyForRoomOperations)
        {
            PhotonNetwork.JoinRoom(roomName);
            progressLabel.SetActive(true);
            createRoomDir.SetActive(false);
        }
        else
        {
            Debug.Log("Not ready for room operations yet.");
        }
    }
    private void UpdatePlayerCount()
    {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
        Instantiate(playerNamePrefab, playerNameDir);
        playerCountText.text = string.Format("Players: {0}/{1}", playerCount, maxPlayers);
    }
    
    private void UpdatePlayerList()
    {
        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length; i++)
        {
            TextMeshProUGUI playerNameText;

            if (i >= playerNameDir.childCount) playerNameText = Instantiate(playerNamePrefab, playerNameDir);
            else playerNameText = playerNameDir.GetChild(i).GetComponent<TextMeshProUGUI>();

            playerNameText.text = players[i].NickName;
        }

        for (int i = players.Length; i < playerNameDir.childCount; i++)
        {
            playerNameDir.GetChild(i).GetComponent<TextMeshProUGUI>().text = string.Empty;
        }
    }
    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] = info;
            }
        }
    }
   
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("MapIndex"))
        {
          
        }
    }

    private void HideRoomPanel()
    {
        loginPanel.SetActive(false);
        createRoomDir.SetActive(false);
        roomDir.SetActive(false);
    }
}