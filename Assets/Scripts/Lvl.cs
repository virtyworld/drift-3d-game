using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lvl : MonoBehaviourPunCallbacks
{
    [SerializeField] private CarController[] _carControllerPrefabs;
    [SerializeField] public TextMeshProUGUI countdownText;
    [SerializeField] public TextMeshProUGUI pointText;
    [SerializeField] public Transform trackDir;
    [SerializeField] public GameObject scorePanel;
    [SerializeField] public GameObject[] maps;
    [SerializeField] public Button leaveButton;

    private float countdownTimer = 5f;
    private float gameTimer = 5f; 
    private bool gameStarted;
    private bool isControlEnabled;
    private CarController _carController;
    private GameObject map;
    
    public bool IsControlEnabled => isControlEnabled;

    void Start()
    {
        leaveButton.onClick.AddListener(LeaveRoom);
        InitMap();
        InitCar();
    }

    void FixedUpdate()
    {
        if (!gameStarted)
        {
            countdownTimer -= Time.deltaTime;
            int seconds = Mathf.FloorToInt(countdownTimer);

            if (seconds < 1) countdownText.text = "GO GO GO";
            else countdownText.text = seconds.ToString();

            if (countdownTimer <= 0f)
            {
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
                ShowScoreTable();
                countdownText.gameObject.SetActive(false);
            }
        }
    }


    private void InitMap()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int mapIndex = (int)PhotonNetwork.CurrentRoom.CustomProperties["MapIndex"];
            map = Instantiate(maps[mapIndex], trackDir);
        }
        else
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("MapIndex"))
            {
                int mapIndex = (int)PhotonNetwork.CurrentRoom.CustomProperties["MapIndex"];
                map = Instantiate(maps[mapIndex], trackDir);
            }
        }
    }

    private void InitCar()
    {
        int carIndex = PlayerPrefs.GetInt(PlayerPrefsVariables.playerChoosenCar.ToString(), 0);

        if (_carControllerPrefabs[carIndex] != null)
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                GameObject car = PhotonNetwork.Instantiate(this._carControllerPrefabs[carIndex].name,
                    new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                _carController = car.GetComponent<CarController>();
                _carController.Setup(this);
                SetPlayeCustomizeSettings();
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
        Debug.Log("OnLeftRoom");
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    private void SetPlayeCustomizeSettings()
    {
        SetCarColor();
        SetSpoiler();
    }

    private void SetCarColor()
    {
        int color = PlayerPrefs.GetInt(PlayerPrefsVariables.playerColorChoosen.ToString(), 0);
        switch (color)
        {
            case 0:
                _carController.frontLeftWheelMesh.GetComponent<MeshRenderer>().material.color = Color.red;
                _carController.frontRightWheelMesh.GetComponent<MeshRenderer>().material.color = Color.red;
                _carController.rearLeftWheelMesh.GetComponent<MeshRenderer>().material.color = Color.red;
                _carController.rearRightWheelMesh.GetComponent<MeshRenderer>().material.color = Color.red;
                break;
            case 1:
                _carController.frontLeftWheelMesh.GetComponent<MeshRenderer>().material.color = Color.black;
                _carController.frontRightWheelMesh.GetComponent<MeshRenderer>().material.color = Color.black;
                _carController.rearLeftWheelMesh.GetComponent<MeshRenderer>().material.color = Color.black;
                _carController.rearRightWheelMesh.GetComponent<MeshRenderer>().material.color = Color.black;
                break;
        }
    }

    private void SetSpoiler()
    {
        int spoilerChoose = PlayerPrefs.GetInt(PlayerPrefsVariables.playerSpoilerChoosen.ToString(), 0);

        if (spoilerChoose == 0)
        {
            _carController.spoiler.gameObject.SetActive(false);
        }
        else
        {
            _carController.spoiler.gameObject.SetActive(true);
        }
    }
    
    void OnApplicationPause(bool isPaused) {                 
        IronSource.Agent.onApplicationPause(isPaused);
    }
    
    private void ShowScoreTable()
    {
        scorePanel.SetActive(true);
    }

    public void MultiplyPoints()
    {
        pointText.text = (_carController.DriftPoints * 2f).ToString();
    }
}