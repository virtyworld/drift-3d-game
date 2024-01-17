using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CarGame
{
    sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] private Configuration configuration;
        [SerializeField] private Camera mainCamera;
        [SerializeField] public Transform trackDir;
        [SerializeField] public Transform carsDir;
        
        private EcsWorld ecsWorld;
        private IEcsSystems initSystems;
        private IEcsSystems updateSystems;
        private IEcsSystems fixedUpdateSystems;
        private IEcsSystems lateUpdateSystems;


        private void Start()
        {
            ecsWorld = new EcsWorld();
            GameData gameData = new GameData();
            gameData.trackDir = trackDir;
            gameData.configuration = configuration;

            initSystems = new EcsSystems(ecsWorld)
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Add(new LvlSpawnSystem())
                .Add(new CarSpawnSystem())
                .Add(new CarMoveSystem())
                .Inject(gameData);

            initSystems.Init();

            // updateSystems = new EcsSystems(ecsWorld)
            //     .Inject(gameData);

            // updateSystems.Init();

            fixedUpdateSystems = new EcsSystems(ecsWorld)
                .Add(new CarSystem())
                .Inject(gameData);


            fixedUpdateSystems.Init();

            // lateUpdateSystems = new EcsSystems(ecsWorld)
            //     .Add(new TestEventsDestructionSystem())
            //     .Inject(eventBus);


            // lateUpdateSystems.Init();
        }

        // private void Update()
        // {
        //     updateSystems.Run();
        // }

        private void FixedUpdate()
        {
            fixedUpdateSystems.Run();
        }

        // private void LateUpdate()
        // {
        //     lateUpdateSystems.Run();
        // }

        private void OnDestroy()
        {
            initSystems.Destroy();
            updateSystems.Destroy();
            fixedUpdateSystems.Destroy();
            // lateUpdateSystems.Destroy();
            ecsWorld.Destroy();
        }
    }
}