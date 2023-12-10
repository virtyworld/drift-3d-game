using Photon.Pun;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 5f, -10f); // Смещение камеры относительно машины
    public float smoothSpeed = 0.125f; 
    public PhotonView photonView; 
    
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            // Получаем позицию, куда должна двигаться камера (позиция машины с учетом смещения)
            Vector3 targetPosition = gameObject.transform.position + offset;

            // Плавно перемещаем позицию камеры к новой позиции
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, smoothSpeed);

            // Направляем камеру на машину
            Camera.main.transform.LookAt(gameObject.transform); 
        }
    }
}
