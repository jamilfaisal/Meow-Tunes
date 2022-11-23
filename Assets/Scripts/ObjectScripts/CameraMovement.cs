using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraMovement : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    public GameObject playerGameObject;
    private float speed = 2.5f;
    private Vector3 lookAtPoint;
    private bool _playCameraRotateAnimation;

    // Start is called before the first frame update
    void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.m_Follow = playerGameObject.transform;

        lookAtPoint = playerGameObject.transform.position;
        _playCameraRotateAnimation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playCameraRotateAnimation) {
            cinemachineVirtualCamera.m_Follow = null;
            cinemachineVirtualCamera.m_LookAt = playerGameObject.transform;
            float rotationAroundYAxis = -10 * Time.deltaTime * speed; // camera moves horizontally
            float rotationAroundXAxis = -5 * Time.deltaTime * speed; // camera moves vertically
            
            transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
            transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);
            
            transform.Translate(new Vector3(1, -1, 2) * Time.deltaTime * speed, Space.World);
        } else {
            cinemachineVirtualCamera.m_Follow = playerGameObject.transform;
            cinemachineVirtualCamera.m_LookAt = null;
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
