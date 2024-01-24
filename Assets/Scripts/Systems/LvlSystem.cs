using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CarGame
{
    sealed class LvlSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<Lvl> lvlPool;
        private EcsWorld world;
        private EcsFilter lvlFilter;
        private float countdownTimer;
        private readonly EcsCustomInject<GameData> gameData = default;
        private float timeToHideText = 2f;
        private bool gameStarted;
        private bool isTextHidden;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            lvlFilter = world.Filter<Lvl>().End();
        }

        public void Run(IEcsSystems systems)
        {
            if (!gameStarted) Timer();
            if (!isTextHidden && gameStarted) HideText();
        }

        private void Timer()
        {
            lvlPool = world.GetPool<Lvl>();

            foreach (int entity in lvlFilter)
            {
                ref Lvl lvl = ref lvlPool.Get(entity);

                if (!lvl.gameStarted)
                {
                    lvl.countdownTimer -= Time.deltaTime;
                    gameData.Value.textTimer.text = lvl.countdownTimer.ToString("0.00");
                    

                    if (lvl.countdownTimer <= 0f)
                    {
                        lvl.gameStarted = true;
                        gameStarted = true;
                        lvl.isControlEnabled = true;
                        gameData.Value.textTimer.text = "GO GO GO";
                    }
                }
            }
        }

        private void HideText()
        {
            timeToHideText -= Time.deltaTime;

            if (timeToHideText <= 0f)
            {
                gameData.Value.textTimer.gameObject.SetActive(false);
                isTextHidden = true;
            }
        }
    }
}