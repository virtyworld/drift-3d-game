using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CarGame
{
    sealed class AudioSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<GameData> gameData = default;
        private  EcsWorld world;
        private EcsPool<Audio> audioPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            audioPool = world.GetPool<Audio>();
            
            ref Audio audio = ref audioPool.Add(world.NewEntity());
          
            SpawnAudio(ref audio);
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in world.Filter<Audio>().End())
            {
                ref Audio audio = ref audioPool.Get(entity);
                
                if (!audio.audioMonoRef.audioSource.isPlaying)
                {
                    PlayRandomSound(ref audio);
                }
            }
        }

        private void SpawnAudio(ref Audio audio)
        {
            GameObject go = Object.Instantiate(gameData.Value.configuration.audioSettings.audioPrefab);
            AudioMono audioMono = go.GetComponent<AudioMono>();
            audio.audioMonoRef = audioMono;
        }
        
        private void PlayRandomSound(ref Audio audio)
        {
            List<AudioClip> raceClips =
                new List<AudioClip>(gameData.Value.configuration.audioSettings.audioData[0].clips);//race clips
            audio.audioMonoRef.audioSource.clip = raceClips[Random.Range(0, raceClips.Count)];
            audio.audioMonoRef.audioSource.Play();
        }
    }
}