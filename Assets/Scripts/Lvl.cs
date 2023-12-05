using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lvl : MonoBehaviour
{
    [SerializeField] private CarController _carControllerPrefab;
    [SerializeField]  public TextMeshProUGUI countdownText;
    [SerializeField]  public CameraFollow _cameraFollow;
    
    private float countdownTimer = 5f; // Отсчёт в 5 секунд перед началом игры
    private float gameTimer = 120f; // 2 минуты игрового времени
    private bool gameStarted = false;

    private CarController _carController;
    void Start()
    {
        _carController =  Instantiate(_carControllerPrefab);
        _carController.enabled = false;
        _cameraFollow.target = _carController.transform;
    }

    // Update is called once per frame
    void Update()
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
                // Время вышло, выполните необходимые действия
            }
        }
    }
}
