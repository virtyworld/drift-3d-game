using UnityEngine;

public class CarControllerDELETE : MonoBehaviour
{
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    public Transform frontLeftWheelMesh;
    public Transform frontRightWheelMesh;
    public Transform rearLeftWheelMesh;
    public Transform rearRightWheelMesh;
    public Rigidbody rb; 
    public Transform spoiler;
    public float motorTorque = 3000f;
    public float brakeTorque = 500f; 
    
    public float maxSteeringAngle = 30f;
    private float driftPoints = 0f;
    private Lvl _lvl;
  void Start()
    {
        WheelFrictionCurve rearFriction = rearLeftWheelCollider.sidewaysFriction;
        rearFriction.stiffness = 0f;
        rearLeftWheelCollider.sidewaysFriction = rearFriction;
        rearRightWheelCollider.sidewaysFriction = rearFriction;
        rb.drag = 1f;
    }
  public float decelerationRate = 10f;
    private void FixedUpdate()
    {
        float steering = Input.GetAxis("Horizontal") * maxSteeringAngle;
        float inputVertical = Input.GetAxis("Vertical");
        float motor = 0f;

        if (inputVertical > 0f) {
            motor = inputVertical * motorTorque; // Moving forward
        } else if (inputVertical < 0f) {
            motor = inputVertical * motorTorque * 1f; // Moving backward with reduced torque
        }
        
            // Применяем моторный момент к задним колесам
            rearLeftWheelCollider.motorTorque = motor;
            rearRightWheelCollider.motorTorque = motor;

            // Применяем угол поворота к передним колесам
            frontLeftWheelCollider.steerAngle = steering;
            frontRightWheelCollider.steerAngle = steering;

            
             // Debug.Log("Скорость машины: " + rb.velocity.magnitude); 
            UpdateWheelMesh(frontLeftWheelCollider, frontLeftWheelMesh);
            UpdateWheelMesh(frontRightWheelCollider, frontRightWheelMesh);
            UpdateWheelMesh(rearLeftWheelCollider, rearLeftWheelMesh);
            UpdateWheelMesh(rearRightWheelCollider, rearRightWheelMesh);

            if (Input.GetKey(KeyCode.Space))
            {
                float timeInSeconds = 1f; 

                Vector3 futurePosition = PredictFuturePosition(timeInSeconds);

                // Вычисление угла поворота задних колес к предсказанной позиции
                Vector3 directionToFuturePosition = (futurePosition - transform.position).normalized;
                float angleToFuturePosition =
                    Vector3.SignedAngle(transform.forward, directionToFuturePosition, transform.up);

                // Определение коэффициента скольжения в зависимости от угла
                float maxSlipAngle = 360; // Максимальный угол для скольжения
                float slipCoefficient = Mathf.Clamp01(Mathf.Abs(angleToFuturePosition) / maxSlipAngle);

                // Эмуляция скольжения задних колес по углу
                WheelFrictionCurve rearFriction = rearLeftWheelCollider.sidewaysFriction;
                rearFriction.stiffness = Mathf.Lerp(0.1f, 2f, slipCoefficient); // Изменение stiffness
                rearLeftWheelCollider.sidewaysFriction = rearFriction;
                rearRightWheelCollider.sidewaysFriction = rearFriction;

                if (Mathf.Abs(angleToFuturePosition) > 20f)
                {
                    driftPoints++;
                }

            }
            else
            {
                // Возвращение стандартных параметров для передачи движения на задние колеса
                WheelFrictionCurve rearFriction = rearLeftWheelCollider.sidewaysFriction;
                rearFriction.stiffness = 1f; // Верните stiffness к обычному значению
                rearLeftWheelCollider.sidewaysFriction = rearFriction;
                rearRightWheelCollider.sidewaysFriction = rearFriction;
            }
            
            if (Input.GetAxis("Vertical") > 0)
            {
                rearLeftWheelCollider.brakeTorque = 0f;
                rearRightWheelCollider.brakeTorque = 0f;
                rb.drag = 0f;
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                Debug.Log(motor);
                rearLeftWheelCollider.motorTorque = motor;
                rearRightWheelCollider.motorTorque = motor;
                rearLeftWheelCollider.brakeTorque = 0f;
                rearRightWheelCollider.brakeTorque = 0f;
                rb.drag = 0f;
            }
            else
            {
                Debug.Log("3");
                rearLeftWheelCollider.brakeTorque = brakeTorque;
                rearRightWheelCollider.brakeTorque = brakeTorque;
                rearLeftWheelCollider.motorTorque = 0f;
                rearRightWheelCollider.motorTorque = 0f;
                rb.drag = 1f;
            }

            // Получаем позицию, куда должна двигаться камера (позиция машины с учетом смещения)
            Vector3 targetPosition = gameObject.transform.position + new Vector3(0,5,-10);

            // Плавно перемещаем позицию камеры к новой позиции
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, 0.125f);

            // Направляем камеру на машину
            Camera.main.transform.LookAt(gameObject.transform); 
    }
    
    private Vector3 PredictFuturePosition(float time)
    {
        Vector3 currentPosition = transform.position; // Текущая позиция машины
        Vector3 currentVelocity = rb.velocity; // Текущая скорость машины
        Vector3 currentAngularVelocity = rb.angularVelocity; // Угловая скорость машины

        // Предполагаем, что движение прямолинейное и без ускорения (равномерное движение)
        Vector3 futurePosition = currentPosition + currentVelocity * time;

        // Учитываем поворот машины
        Quaternion rotationChange = Quaternion.Euler(currentAngularVelocity * Mathf.Rad2Deg * time);
        Vector3 direction = rotationChange * transform.forward;
        Vector3 predictedPositionWithRotation = currentPosition + direction * currentVelocity.magnitude * time;

        return predictedPositionWithRotation;
    }
    
    private void UpdateWheelMesh(WheelCollider collider, Transform mesh)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        mesh.position = position;
        mesh.rotation = rotation;
    }
}
