using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraMovement : MonoBehaviour
{
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    public GameObject playerGameObject;
    private const float Speed = 2.5f;
    private bool _playCameraRotateAnimation;

    // Start is called before the first frame update
    private void Awake()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachineVirtualCamera.m_Follow = playerGameObject.transform;

        _playCameraRotateAnimation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playCameraRotateAnimation) {
            _cinemachineVirtualCamera.m_Follow = null;
            _cinemachineVirtualCamera.m_LookAt = playerGameObject.transform;
            float rotationAroundYAxis = -10 * Time.deltaTime * Speed; // camera moves horizontally
            float rotationAroundXAxis = -5 * Time.deltaTime * Speed; // camera moves vertically
            
            transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
            transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);
            
            transform.Translate(new Vector3(1, -1, 2) * (Time.deltaTime * Speed), Space.World);
        } else {
            _cinemachineVirtualCamera.m_Follow = playerGameObject.transform;
            _cinemachineVirtualCamera.m_LookAt = null;
        }
    }

    public void SetPlayCameraRotateAnimationTrue()
    {
        _playCameraRotateAnimation = true;
    }

    public void SetPlayCameraRotateAnimationFalse()
    {
        _playCameraRotateAnimation = false;
    }
}
