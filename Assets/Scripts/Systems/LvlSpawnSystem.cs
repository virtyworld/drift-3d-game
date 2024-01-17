using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Photon.Pun;
using UnityEngine;

namespace CarGame
{
    sealed class LvlSpawnSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<GameData> gameData = default;
        private GameObject map;

        public void Init(IEcsSystems systems)
        {
            InitMap();
        }

        private void InitMap()
        {
            int mapIndex = (int) PhotonNetwork.CurrentRoom.CustomProperties["MapIndex"];

            map = Object.Instantiate(gameData.Value.configuration.mapSettings.mapData[mapIndex].prefab,
                new Vector3(0, 0, 0), Quaternion.identity);
            map.transform.SetParent(gameData.Value.trackDir);
        }
    }
}