using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Photon.Pun;
using UnityEngine;

namespace CarGame
{
    sealed class CarSpawnSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<GameData> gameData = default;
        private CarMono _carMono;
        private EcsWorld world;
        private EcsFilter lvlFilter;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            lvlFilter = world.Filter<Lvl>().End();
            
            InitCar();
        }


        private void InitCar()
        {
            int carIndex = PlayerPrefs.GetInt(PlayerPrefsVariables.playerChoosenCar.ToString(), 0);

            if (gameData.Value.configuration.carSettings.carData[carIndex].prefab != null)
            {
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    GameObject car = PhotonNetwork.Instantiate(
                        gameData.Value.configuration.carSettings.carData[carIndex].prefab.name,
                        new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                    _carMono = car.GetComponent<CarMono>();

                    InitEntity(_carMono);
                    SetPlayerCustomizeSettings();
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        private void SetPlayerCustomizeSettings()
        {
            SetCarColor();
            SetSpoiler();
        }

        private void SetCarColor()
        {
            int color = PlayerPrefs.GetInt(PlayerPrefsVariables.playerColorChoosen.ToString(), 0);
            switch (color)
            {
                case 0:
                    _carMono.frontLeftWheelMesh.GetComponent<MeshRenderer>().material.color = Color.red;
                    _carMono.frontRightWheelMesh.GetComponent<MeshRenderer>().material.color = Color.red;
                    _carMono.rearLeftWheelMesh.GetComponent<MeshRenderer>().material.color = Color.red;
                    _carMono.rearRightWheelMesh.GetComponent<MeshRenderer>().material.color = Color.red;
                    break;
                case 1:
                    _carMono.frontLeftWheelMesh.GetComponent<MeshRenderer>().material.color = Color.black;
                    _carMono.frontRightWheelMesh.GetComponent<MeshRenderer>().material.color = Color.black;
                    _carMono.rearLeftWheelMesh.GetComponent<MeshRenderer>().material.color = Color.black;
                    _carMono.rearRightWheelMesh.GetComponent<MeshRenderer>().material.color = Color.black;
                    break;
            }
        }

        private void SetSpoiler()
        {
            int spoilerChoose = PlayerPrefs.GetInt(PlayerPrefsVariables.playerSpoilerChoosen.ToString(), 0);

            if (spoilerChoose == 0)
            {
                _carMono.spoiler.gameObject.SetActive(false);
            }
            else
            {
                _carMono.spoiler.gameObject.SetActive(true);
            }
        }

        private void InitEntity(CarMono _carMono)
        {
            if (_carMono.photonViewScript.IsMine)
            {
                ref Car car = ref world.GetPool<Car>().Add(world.NewEntity());
                car.lvlEntity = world.PackEntity(GetLvlEntity());
                car.carMonoRef = _carMono;
            }
        }

        private int GetLvlEntity()
        {
            foreach (int lvlEntity in lvlFilter)
            {
                return lvlEntity;
            }

            return 0;
        }
    }
}