using Leopotam.EcsLite;
using UnityEngine;

namespace CarGame
{
    sealed class CarMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld world;
        private EcsPool<Car> carPool;
        private EcsPool<Lvl> lvlPool;
        private EcsFilter carFilter;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            lvlPool = world.GetPool<Lvl>();
        }

        public void Run(IEcsSystems systems)
        {
            carPool = world.GetPool<Car>();
            carFilter = world.Filter<Car>().End();
            lvlPool = world.GetPool<Lvl>();
            
            Move();
        }

        private void Move()
        {
            foreach (int carEntity in carFilter)
            {
                ref Car car = ref carPool.Get(carEntity);

                if (car.lvlEntity.Unpack(world, out int unpacked))
                {
                    ref Lvl lvl = ref lvlPool.Get(unpacked);

                    if (car.carMonoRef.photonViewScript.IsMine &&  lvl.isControlEnabled)
                    {
                        float steering = Input.GetAxis("Horizontal") * car.carMonoRef.maxSteeringAngle;
                        float inputVertical = Input.GetAxis("Vertical");
                        float motor = 0f;

                        if (inputVertical > 0f)
                        {
                            motor = inputVertical * car.carMonoRef.motorTorque; // Moving forward
                        }
                        else if (inputVertical < 0f)
                        {
                            motor = inputVertical * car.carMonoRef.motorTorque *
                                    1f; // Moving backward with reduced torque
                        }

                        // Применяем моторный момент к задним колесам
                        car.carMonoRef.rearLeftWheelCollider.motorTorque = motor;
                        car.carMonoRef.rearRightWheelCollider.motorTorque = motor;

                        // Применяем угол поворота к передним колесам
                        car.carMonoRef.frontLeftWheelCollider.steerAngle = steering;
                        car.carMonoRef.frontRightWheelCollider.steerAngle = steering;

                        car.carMonoRef.UpdateWheelMesh(car.carMonoRef.frontLeftWheelCollider,
                            car.carMonoRef.frontLeftWheelMesh);
                        car.carMonoRef.UpdateWheelMesh(car.carMonoRef.frontRightWheelCollider,
                            car.carMonoRef.frontRightWheelMesh);
                        car.carMonoRef.UpdateWheelMesh(car.carMonoRef.rearLeftWheelCollider,
                            car.carMonoRef.rearLeftWheelMesh);
                        car.carMonoRef.UpdateWheelMesh(car.carMonoRef.rearRightWheelCollider,
                            car.carMonoRef.rearRightWheelMesh);

                        float angleToFuturePosition = car.carMonoRef.CalculateAngleToFuturePosition();


                        if (Input.GetKey(KeyCode.Space))
                        {
                            // Определение коэффициента скольжения в зависимости от угла
                            float maxSlipAngle = 360; // Максимальный угол для скольжения
                            float slipCoefficient = Mathf.Clamp01(Mathf.Abs(angleToFuturePosition) / maxSlipAngle);

                            // Эмуляция скольжения задних колес по углу
                            WheelFrictionCurve rearFriction = car.carMonoRef.rearLeftWheelCollider.sidewaysFriction;
                            rearFriction.stiffness = Mathf.Lerp(0.1f, 2f, slipCoefficient); // Изменение stiffness
                            car.carMonoRef.rearLeftWheelCollider.sidewaysFriction = rearFriction;
                            car.carMonoRef.rearRightWheelCollider.sidewaysFriction = rearFriction;
                        }
                        else
                        {
                            // Возвращение стандартных параметров для передачи движения на задние колеса
                            WheelFrictionCurve rearFriction = car.carMonoRef.rearLeftWheelCollider.sidewaysFriction;
                            rearFriction.stiffness = 1f; // Верните stiffness к обычному значению
                            car.carMonoRef.rearLeftWheelCollider.sidewaysFriction = rearFriction;
                            car.carMonoRef.rearRightWheelCollider.sidewaysFriction = rearFriction;
                        }

                        if (Input.GetAxis("Vertical") > 0)
                        {
                            car.carMonoRef.rearLeftWheelCollider.brakeTorque = 0f;
                            car.carMonoRef.rearRightWheelCollider.brakeTorque = 0f;
                            car.carMonoRef.rb.drag = 0f;
                        }
                        else if (Input.GetAxis("Vertical") < 0)
                        {
                            car.carMonoRef.rearLeftWheelCollider.motorTorque = motor;
                            car.carMonoRef.rearRightWheelCollider.motorTorque = motor;
                            car.carMonoRef.rearLeftWheelCollider.brakeTorque = 0f;
                            car.carMonoRef.rearRightWheelCollider.brakeTorque = 0f;
                            car.carMonoRef.rb.drag = 0f;
                        }
                        else
                        {
                            car.carMonoRef.rearLeftWheelCollider.brakeTorque = car.carMonoRef.brakeTorque;
                            car.carMonoRef.rearRightWheelCollider.brakeTorque = car.carMonoRef.brakeTorque;
                            car.carMonoRef.rearLeftWheelCollider.motorTorque = 0f;
                            car.carMonoRef.rearRightWheelCollider.motorTorque = 0f;
                            car.carMonoRef.rb.drag = 1f;
                        }
                    }
                }
            }
        }
    }
}