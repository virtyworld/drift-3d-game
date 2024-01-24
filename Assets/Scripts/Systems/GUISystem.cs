using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;

namespace CarGame
{
    sealed class GUISystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<EcsUguiClickEvent> _clickEventsPool;
        private EcsFilter _clickEvents;
        private EcsFilter lvlFilter;

        private EcsWorld worldUgui;
        private EcsWorld world;

        public void Init(IEcsSystems systems)
        {
            worldUgui = systems.GetWorld("ugui-events");
            world = systems.GetWorld();
        }

        public void Run(IEcsSystems systems)
        {
            _clickEventsPool = worldUgui.GetPool<EcsUguiClickEvent>();
            _clickEvents = worldUgui.Filter<EcsUguiClickEvent>().End();
            lvlFilter = world.Filter<Lvl>().End();

            foreach (var entity in _clickEvents)
            {
                ref EcsUguiClickEvent data = ref _clickEventsPool.Get(entity);

                if (data.WidgetName.Equals(ButtonName.LeaveFromRace.ToString()))
                {
                    LeaveFromRace();
                }
            }
        }

        private void LeaveFromRace()
        {
            foreach (int entity in lvlFilter)
            {
                ref Lvl lvl = ref world.GetPool<Lvl>().Get(entity);
                lvl.LvlMonoRef.LeaveRoom();
            }
        }
    }
}