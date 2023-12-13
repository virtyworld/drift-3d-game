using Photon.Pun;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private PhotonView photonView;
    [SerializeField] private float distance = 7.0f;
    [SerializeField] private float height = 3.0f;
    [SerializeField] private Vector3 centerOffset = Vector3.zero;
    [SerializeField] private bool followOnStart = true;

    private Vector3 cameraOffset = Vector3.zero;
    private Transform cameraTransform;
    private bool isFollowing;
    
    private void Start()
    {
        // Start following the target if wanted.
        if (followOnStart)
        {
            OnStartFollowing();
        }
    }
    
    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            if (cameraTransform == null && isFollowing)
            {
                OnStartFollowing();
            }
        
            // only follow is explicitly declared
            if (isFollowing)
            {
                Follow();
            }
        }
    }
    private void Follow()
    {
        cameraOffset.z = -distance;
        cameraOffset.y = height;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position,
            this.transform.position + this.transform.TransformVector(cameraOffset), smoothSpeed * Time.deltaTime);

        cameraTransform.LookAt(this.transform.position + centerOffset);
    }
    private void OnStartFollowing()
    {
        cameraTransform = Camera.main.transform;
        isFollowing = true;
        Cut();
    }
    private void Cut()
    {
        cameraOffset.z = -distance;
        cameraOffset.y = height;

        cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);

        cameraTransform.LookAt(this.transform.position + centerOffset);
    }
}