using CarGame.MonoBehaviours;
using UnityEngine;

namespace CarGame
{
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "GameSettings/AudioSettings")]
    public class AudioSettings : ScriptableObject
    {
        public AudioData[] audioData;
        public GameObject audioPrefab;
    }
}