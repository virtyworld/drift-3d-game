using UnityEngine;

namespace CarGame
{
    [CreateAssetMenu(fileName = "MapSettings", menuName = "GameSettings/MapSettings")]
    public class MapSettings : ScriptableObject
    {
        public MapData[] mapData;
    }
}