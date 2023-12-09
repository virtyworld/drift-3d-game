using System;
using UnityEngine;
using UnityEngine.UI;

public class Customization : MonoBehaviour
{
   [SerializeField] private Button redColorButton;
   [SerializeField] private Button blackColorButton;
   [SerializeField] private Button spoilerButton;
   [SerializeField] private Garage garage;

   private Vector3 cameraPos;
   private Quaternion cameraRot;
   
   private void Start()
   {
      redColorButton.onClick.AddListener(() => ChangeColor(CarColor.red));
      blackColorButton.onClick.AddListener(() => ChangeColor(CarColor.black));
      spoilerButton.onClick.AddListener(SpoilerState);
      cameraPos = new Vector3(-2f, 1.3f, 2.85f);
      cameraRot = Quaternion.Euler(13.32f, 128.5f, 0f);
   }

   private void FixedUpdate()
   {
      CameraSmooth();
   }

   private void ChangeColor(CarColor color)
   {
      GameObject go = GetCurrentCar();

      CarCustomize carController = go.GetComponent<CarCustomize>();
      
      if (carController != null)
      {
         switch (color)
         {
            case CarColor.red:
               carController.frontLeftWheelMesh.GetComponent<MeshRenderer>().material.color = Color.red; // Пример изменения цвета на красный
               carController.frontRightWheelMesh.GetComponent<MeshRenderer>().material.color = Color.red; // Пример изменения цвета на красный
               carController.rearLeftWheelMesh.GetComponent<MeshRenderer>().material.color = Color.red; // Пример изменения цвета на красный
               carController.rearRightWheelMesh.GetComponent<MeshRenderer>().material.color = Color.red; // Пример изменения цвета на красный
               break;
            case CarColor.black:
               carController.frontLeftWheelMesh.GetComponent<MeshRenderer>().material.color  = Color.black; // Пример изменения цвета на черный
               carController.frontRightWheelMesh.GetComponent<MeshRenderer>().material.color  = Color.black;
               carController.rearLeftWheelMesh.GetComponent<MeshRenderer>().material.color  = Color.black;
               carController.rearRightWheelMesh.GetComponent<MeshRenderer>().material.color  = Color.black;
               break;
         }
      }

      cameraPos = new Vector3(-2f, 1.3f, 2.85f);
      cameraRot = Quaternion.Euler(13.32f, 128.5f, 0f);
   }

   private void SpoilerState()
   {
      GameObject go = GetCurrentCar();

      CarController carController = go.GetComponent<CarController>();
      
      carController.spoiler.gameObject.SetActive(!carController.spoiler.gameObject.activeSelf);
      
      cameraPos = new Vector3(-1.73f, 1.43f, -0.46f);
      cameraRot = Quaternion.Euler(23.6f, 117.58f, 0f);
   }

   private GameObject GetCurrentCar()
   {
      return garage.CarShop.CurrentCar;
   }

   private void CameraSmooth()
   {
      if (Camera.main.transform.position == cameraPos &&
          Camera.main.transform.rotation == cameraRot)
      {
         // Если камера уже в нужной позиции и повороте, ничего не делать
         return;
      }
      Vector3 smoothedPosition = Vector3.Lerp(Camera.main.transform.position, cameraPos, 0.125f);
      Camera.main.transform.position = smoothedPosition;
      Quaternion smoothedRotation = Quaternion.Lerp(Camera.main.transform.rotation, cameraRot, 0.125f);
      Camera.main.transform.rotation = smoothedRotation;
   }
}
