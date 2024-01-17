using Photon.Pun;
using UnityEngine;

public class CarMono : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] public GameObject PlayerUiPrefab;
    
    public PhotonView photonViewScript;
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
    public Lvl _lvl;
    public float motorTorque = 3000f;
    public float brakeTorque = 500f; 
    
    public float maxSteeringAngle = 30f;
    private float driftPoints = 0f;

    public float DriftPoints => driftPoints;
    
    private void Awake()
    {
        if (photonViewScript.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        WheelFrictionCurve rearFriction = rearLeftWheelCollider.sidewaysFriction;
        rearFriction.stiffness = 1f;
        rearLeftWheelCollider.sidewaysFriction = rearFriction;
        rearRightWheelCollider.sidewaysFriction = rearFriction;
        
        PhotonStartLogic();
    }
    
    private void FixedUpdate()
    {
        if (photonViewScript.IsMine && _lvl != null && _lvl.IsControlEnabled)
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

            UpdateWheelMesh(frontLeftWheelCollider, frontLeftWheelMesh);
            UpdateWheelMesh(frontRightWheelCollider, frontRightWheelMesh);
            UpdateWheelMesh(rearLeftWheelCollider, rearLeftWheelMesh);
            UpdateWheelMesh(rearRightWheelCollider, rearRightWheelMesh);

            float angleToFuturePosition = CalculateAngleToFuturePosition();

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

    private void PhotonStartLogic()
    {
        if (PlayerUiPrefab != null)
        {
            GameObject _uiGo = Instantiate(PlayerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
        else
        {
            Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
        }
#if UNITY_5_4_OR_NEWER
// Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
    }

    private void UpdateWheelMesh(WheelCollider collider, Transform mesh)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        mesh.position = position;
        mesh.rotation = rotation;
    }
  

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
#if UNITY_5_4_OR_NEWER
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        this.CalledOnLevelWasLoaded(scene.buildIndex);
    }
#endif
#if !UNITY_5_4_OR_NEWER
/// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
void OnLevelWasLoaded(int level)
{
    this.CalledOnLevelWasLoaded(level);
}
#endif
#if UNITY_5_4_OR_NEWER
    public override void OnDisable()
    {
        // Always call the base to remove callbacks
        base.OnDisable();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
#endif
    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }

        GameObject _uiGo = Instantiate(this.PlayerUiPrefab);
        _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }
    private float CalculateAngleToFuturePosition()
    {
        float timeInSeconds = 0.5f;
        Vector3 futurePosition = PredictFuturePosition(timeInSeconds);

        Vector3 directionToFuturePosition = (futurePosition - transform.position).normalized;
        float angleToFuturePosition = Vector3.SignedAngle(transform.forward, directionToFuturePosition, transform.up);

        return angleToFuturePosition;
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
}