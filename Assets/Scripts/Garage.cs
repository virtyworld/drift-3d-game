using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Garage : MonoBehaviour
{
    [Header("Button-Panel Pairs")]
    [SerializeField] private List<ButtonPanelPair> buttonPanelPairs;
    [SerializeField] private CarShop carShop;
    
    private Dictionary<Button, GameObject> buttonPanelMap = new Dictionary<Button, GameObject>();

    public CarShop CarShop => carShop;
    
    private void Start()
    {
        foreach (var pair in buttonPanelPairs)
        {
            buttonPanelMap[pair.button] = pair.panel;
            pair.button.onClick.AddListener(() => OnButtonClick(pair.button));
        }
    }

    private void OnButtonClick(Button clickedButton)
    {
        foreach (var pair in buttonPanelMap)
        {
            GameObject panel = pair.Value;
            panel.SetActive(pair.Key == clickedButton);
        }
        
        Camera.main.transform.position = new Vector3(-2f, 1.3f, 2.85f);
        Camera.main.transform.rotation = Quaternion.Euler(13.32f, 128.5f, 0f);
    }

    public void Exit()
    {
        Application.Quit();
    }
}


