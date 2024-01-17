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
       
        
        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            
            InitCar();
            InitEntity();
        }

        private void InitEntity()
        {
            if (_carMono.photonViewScript.IsMine)
            {
                world.GetPool<Car>().Add(world.NewEntity());
            }
        }
        private void InitCar()
        {
            int carIndex = PlayerPrefs.GetInt(PlayerPrefsVariables.playerChoosenCar.ToString(), 0);
            
            if (gameData.Value.configuration.carSettings.carData[carIndex].prefab != null)
            {
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    GameObject car = PhotonNetwork.Instantiate(gameData.Value.configuration.carSettings.carData[carIndex].prefab.name,
                        new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                    _carMono = car.GetComponent<CarMono>();
                   
                    SetPlayeCustomizeSettings();
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }
        
        private void SetPlayeCustomizeSettings()
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
    }
}