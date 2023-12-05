using UnityEngine;

public class CarController : MonoBehaviour
{
     public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    public Transform frontLeftWheelMesh;
    public Transform frontRightWheelMesh;
    public Transform rearLeftWheelMesh;
    public Transform rearRightWheelMesh;

    public float motorTorque = 1500f;
    public float maxSteeringAngle = 30f;

    private bool isDrifting = false;
    private float driftPoints = 0f;
    public Rigidbody rb; // Объект Rigidbody вашей машины
    
   
 
    
    void Start()
    {
        // Получаем текущие параметры сцепления задних колес
        WheelFrictionCurve rearFriction = rearLeftWheelCollider.sidewaysFriction;

        // Меняем значение stiffness (коэффициента сцепления)
        rearFriction.stiffness = 2; // Здесь newValue - новое значение коэффициента сцепления

        // Применяем новое значение к задним колесам
        rearLeftWheelCollider.sidewaysFriction = rearFriction;
        rearRightWheelCollider.sidewaysFriction = rearFriction;
    }

    private void FixedUpdate()
    {
        float motor = Input.GetAxis("Vertical") * motorTorque;
        float steering = Input.GetAxis("Horizontal") * maxSteeringAngle;

        // Apply motor torque to wheels
        rearLeftWheelCollider.motorTorque = motor;
        rearRightWheelCollider.motorTorque = motor;

        // Apply steering angle to front wheels
        frontLeftWheelCollider.steerAngle = steering;
        frontRightWheelCollider.steerAngle = steering;

        // Rotate wheel meshes
        UpdateWheelMesh(frontLeftWheelCollider, frontLeftWheelMesh);
        UpdateWheelMesh(frontRightWheelCollider, frontRightWheelMesh);
        UpdateWheelMesh(rearLeftWheelCollider, rearLeftWheelMesh);
        UpdateWheelMesh(rearRightWheelCollider, rearRightWheelMesh);

        if (Input.GetKey(KeyCode.Space))
        {
            float timeInSeconds = 1f; // Время через 1 секунду для примера

            Vector3 futurePosition = PredictFuturePosition(timeInSeconds);

            // Вычисление угла поворота задних колес к предсказанной позиции
            Vector3 directionToFuturePosition = (futurePosition - transform.position).normalized;
            float angleToFuturePosition = Vector3.SignedAngle(transform.forward, directionToFuturePosition, transform.up);

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
            rearFriction.stiffness = 2f; // Верните stiffness к обычному значению
            rearLeftWheelCollider.sidewaysFriction = rearFriction;
            rearRightWheelCollider.sidewaysFriction = rearFriction;
        }
        Debug.Log(driftPoints);
    }

    private void UpdateWheelMesh(WheelCollider collider, Transform mesh)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        mesh.position = position;
        mesh.rotation = rotation;
    }
    public Vector3 PredictFuturePosition(float time)
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


}


