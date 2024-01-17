using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CarShop : MonoBehaviour
{
    [SerializeField] private CarSettings carSettings;
    [SerializeField] private Transform carDirectoryToSpawn;
    [SerializeField] private Button redCarButton;
    [SerializeField] private Button blueCarButton;
    [SerializeField] private Button grayCarButton;
    [SerializeField] private Button purpleCarButton;

    private GameObject currectCar;

    public GameObject CurrentCar => currectCar;
    private void Start()
    {
        redCarButton.onClick.AddListener(() => SpawnCar(CarName.redCar));
        blueCarButton.onClick.AddListener(() => SpawnCar(CarName.blueCar));
        grayCarButton.onClick.AddListener(() => SpawnCar(CarName.grayCar));
        purpleCarButton.onClick.AddListener(() => SpawnCar(CarName.purpleCar));

        int selectedCarIndex = PlayerPrefs.GetInt(PlayerPrefsVariables.playerChoosenCar.ToString(), 1);
        SpawnCar((CarName)selectedCarIndex);
    }

    private void SpawnCar(CarName carName)
    {
        ClearCarDir();
        SpawnNewCar(carName);
        SavePlayerChoose((int)carName);
    }

    private void ClearCarDir()
    {
        foreach (Transform child in carDirectoryToSpawn) {
            Destroy(child.gameObject);
        }
    }
    
    private void SpawnNewCar(CarName name)
    {
        currectCar =  Instantiate(GetPrefab(name), carDirectoryToSpawn, true);
        currectCar.transform.position += new Vector3(0,1,0);
    }
    private GameObject GetPrefab(CarName name)
    {
        return carSettings.carData.FirstOrDefault(x => x.name == name).prefab;
    }

    private void SavePlayerChoose(int carIndex)
    {
        PlayerPrefs.SetInt(PlayerPrefsVariables.playerChoosenCar.ToString(), carIndex);
        PlayerPrefs.Save();
    }
}