using UnityEngine;

[CreateAssetMenu(fileName = "CarSettings", menuName = "GameSettings/CarSettings")]
public class CarSettings : ScriptableObject
{
  public CarData[] carData;
}
