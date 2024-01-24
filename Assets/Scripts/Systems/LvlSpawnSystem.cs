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
        private EcsWorld world;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            ref Lvl lvl = ref world.GetPool<Lvl>().Add(world.NewEntity());
            lvl.countdownTimer = 5f;

            InitMap();
            SpawnLvlMono(ref lvl);
        }

        private void InitMap()
        {
            int mapIndex = (int) PhotonNetwork.CurrentRoom.CustomProperties["MapIndex"];

            map = Object.Instantiate(gameData.Value.configuration.mapSettings.mapData[mapIndex].prefab,
                new Vector3(0, 0, 0), Quaternion.identity);
            map.transform.SetParent(gameData.Value.trackDir);
        }

        private void SpawnLvlMono(ref Lvl lvl)
        {
            GameObject go = Object.Instantiate(gameData.Value.configuration.lvlMomoPrefab);
            LvlMono lvlMono = go.GetComponent<LvlMono>();
            lvl.LvlMonoRef = lvlMono;
        }
    }
}