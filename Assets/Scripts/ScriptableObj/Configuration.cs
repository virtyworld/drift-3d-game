using UnityEngine;

namespace CarGame
{
    [CreateAssetMenu(fileName = "Configuration", menuName = "GameSettings/Configuration")]
    public class Configuration : ScriptableObject
    {
        public MapSettings mapSettings;
        public CarSettings carSettings;
    }
}