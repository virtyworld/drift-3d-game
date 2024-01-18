using System;
using UnityEngine;

namespace CarGame.MonoBehaviours
{
    [Serializable]
    public struct AudioData
    {
        public AudioType name;
        public AudioClip[] clips;
    }
}

public enum AudioType
{
    race,car,menu
}