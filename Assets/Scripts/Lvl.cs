using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lvl : MonoBehaviourPunCallbacks
{
    [SerializeField] private CarController[] _carControllerPrefabs;
    [SerializeField]  public TextMeshProUGUI countdownText;
    [SerializeField]  public TextMeshProUGUI pointText;
    [SerializeField]  public CameraFollow _cameraFollow;
    [SerializeField]  public Transform trackDir;
    [SerializeField]  public GameObject[] maps;
    
    private float countdownTimer = 5f; // Отсчёт в 5 секунд перед началом игры
    private float gameTimer = 120f; // 2 минуты игрового времени
    private bool gameStarted = false;
    private bool isControlEnabled = false;
    private CarController _carController;

    public bool IsControlEnabled => isControlEnabled;
    
    void Start()
    {
        InitMap();
        InitCar();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameStarted)
        {
            countdownTimer -= Time.deltaTime;
            int seconds = Mathf.FloorToInt(countdownTimer);
            
            if (seconds<1) countdownText.text = "GO GO GO";
            else  countdownText.text = seconds.ToString();
           

            if (countdownTimer <= 0f)
            {
                countdownText.gameObject.SetActive(false);
                gameStarted = true;
                _carController.enabled = true;
                isControlEnabled = true;
            }
        }
        else
        {
            gameTimer -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(gameTimer / 60);
            int seconds = Mathf.FloorToInt(gameTimer % 60);

            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            
            if (gameTimer <= 0f)
            {
                StopCarMovement();
                DisableCarControl();
            }
        }
    }

    private GameObject go;
    private void InitMap()
    {
       int map =  PlayerPrefs.GetInt(PlayerPrefsVariables.playerChoosedTrack.ToString(), 0);
       go = Instantiate(maps[map],trackDir);
       
    }
    private void InitCar()
    {
        int carIndex =  PlayerPrefs.GetInt(PlayerPrefsVariables.playerChoosedCar.ToString(), 0);
        
        if (_carControllerPrefabs[carIndex] == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
        }
        else
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
              GameObject car =  PhotonNetwork.Instantiate(this._carControllerPrefabs[carIndex].name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
              _carController = car.GetComponent<CarController>();
              _carController.Setup(this);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
       
    }
    private void StopCarMovement()
    {
        if (_carController != null)
        {
            _carController.rb.velocity = Vector3.zero; // Обнуляем скорость машины
            _carController.rb.angularVelocity = Vector3.zero; // Обнуляем угловую скорость машины (повороты)
        }
    }

    private void DisableCarControl()
    {
        isControlEnabled = false;
    }
    #region photon methods
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadArena();
        }
    }
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadArena();
        }
    }
    #endregion
}
