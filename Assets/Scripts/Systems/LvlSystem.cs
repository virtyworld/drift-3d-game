using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Photon.Pun;
using UnityEngine;

namespace CarGame
{
    sealed class LvlSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<GameData> gameData = default;
        private GameObject map;
        private EcsPool<Lvl> lvlPool;
        private EcsWorld world;
        private EcsFilter lvlFilter;
        private float countdownTimer;

        public void Init(IEcsSystems systems)
        {
            InitMap();
            world = systems.GetWorld();
            world.GetPool<Lvl>().Add(world.NewEntity());
            lvlFilter = world.Filter<Lvl>().End();
        }

        public void Run(IEcsSystems systems)
        {
            Timer();
        }

        private void InitMap()
        {
            int mapIndex = (int) PhotonNetwork.CurrentRoom.CustomProperties["MapIndex"];

            map = Object.Instantiate(gameData.Value.configuration.mapSettings.mapData[mapIndex].prefab,
                new Vector3(0, 0, 0), Quaternion.identity);
            map.transform.SetParent(gameData.Value.trackDir);
        }

        private void Timer()
        {
            foreach (int entity in lvlFilter)
            {
                ref Lvl lvl = ref lvlPool.Get(entity);

                if (!lvl.gameStarted)
                {
                    countdownTimer -= Time.deltaTime;

                    if (countdownTimer <= 0f)
                    {
                        lvl.gameStarted = true;
                        lvl.isControlEnabled = true;
                    }
                }
            }
        }
    }
}