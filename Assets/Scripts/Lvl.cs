using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lvl : MonoBehaviour
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
    private bool isControlEnabled = true;
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
                gameStarted = true;
                _carController.enabled = true;
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

    private void InitMap()
    {
       int map =  PlayerPrefs.GetInt(PlayerPrefsVariables.playerChoosedTrack.ToString(), 0);
       Instantiate(maps[map],trackDir);
    }
    private void InitCar()
    {
        int car =  PlayerPrefs.GetInt(PlayerPrefsVariables.playerChoosedCar.ToString(), 0);
        _carController =  Instantiate(_carControllerPrefabs[car],trackDir);
        _carController.Setup(this);
        _carController.enabled = false;
        _cameraFollow.target = _carController.transform;
    }
    private void StopCarMovement()
    {
        if (_carController != null)
        {
            _carController.rb.velocity = Vector3.zero; // Обнуляем скорость машины
            _carController.rb.angularVelocity = Vector3.zero; // Обнуляем угловую скорость машины (повороты)
        }
    }

    // Отключить управление машиной
    private void DisableCarControl()
    {
        isControlEnabled = false; // Помечаем, что управление отключено
        // Здесь можно добавить логику отключения управления машиной,
        // например, отключение скриптов, отвечающих за управление
    }
}
