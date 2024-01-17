using Leopotam.EcsLite;

namespace CarGame
{
    sealed class CarMoveSystem : IEcsInitSystem,IEcsRunSystem
    {
        private EcsWorld world;
        private EcsPool<Car>carPool;
        private EcsFilter carFilter;
        
        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
        }
        
        public void Run(IEcsSystems systems)
        {
            carPool = world.GetPool<Car>();
            carFilter = world.Filter<Car>().End();
            
            Move();
        }

        private void Move()
        {
            foreach (int carEntity in carFilter)
            {
                ref Car car = ref carPool.Get(carEntity);
                
                if (photonViewScript.IsMine && _lvl != null && _lvl.IsControlEnabled)
            {
                float steering = Input.GetAxis("Horizontal") * maxSteeringAngle;
                float inputVertical = Input.GetAxis("Vertical");
                float motor = 0f;

                if (inputVertical > 0f)
                {
                    motor = inputVertical * motorTorque; // Moving forward
                }
                else if (inputVertical < 0f)
                {
                    motor = inputVertical * motorTorque * 1f; // Moving backward with reduced torque
                }

                // Применяем моторный момент к задним колесам
                rearLeftWheelCollider.motorTorque = motor;
                rearRightWheelCollider.motorTorque = motor;

                // Применяем угол поворота к передним колесам
                frontLeftWheelCollider.steerAngle = steering;
                frontRightWheelCollider.steerAngle = steering;

                UpdateWheelMesh(frontLeftWheelCollider, frontLeftWheelMesh);
                UpdateWheelMesh(frontRightWheelCollider, frontRightWheelMesh);
                UpdateWheelMesh(rearLeftWheelCollider, rearLeftWheelMesh);
                UpdateWheelMesh(rearRightWheelCollider, rearRightWheelMesh);

                float angleToFuturePosition = CalculateAngleToFuturePosition();

                if (Mathf.Abs(angleToFuturePosition) > 40f)
                {
                    driftPoints++;
                    ShowDriftPoints(driftPoints.ToString());
                }
                else
                {
                    HideDriftPoints();
                }

                if (Input.GetKey(KeyCode.Space))
                {
                    // Определение коэффициента скольжения в зависимости от угла
                    float maxSlipAngle = 360; // Максимальный угол для скольжения
                    float slipCoefficient = Mathf.Clamp01(Mathf.Abs(angleToFuturePosition) / maxSlipAngle);

                    // Эмуляция скольжения задних колес по углу
                    WheelFrictionCurve rearFriction = rearLeftWheelCollider.sidewaysFriction;
                    rearFriction.stiffness = Mathf.Lerp(0.1f, 2f, slipCoefficient); // Изменение stiffness
                    rearLeftWheelCollider.sidewaysFriction = rearFriction;
                    rearRightWheelCollider.sidewaysFriction = rearFriction;
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
                    rearLeftWheelCollider.motorTorque = motor;
                    rearRightWheelCollider.motorTorque = motor;
                    rearLeftWheelCollider.brakeTorque = 0f;
                    rearRightWheelCollider.brakeTorque = 0f;
                    rb.drag = 0f;
                }
                else
                {
                    rearLeftWheelCollider.brakeTorque = brakeTorque;
                    rearRightWheelCollider.brakeTorque = brakeTorque;
                    rearLeftWheelCollider.motorTorque = 0f;
                    rearRightWheelCollider.motorTorque = 0f;
                    rb.drag = 1f;
                }
            }
            }
            
        }
    }
}