using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreEndBox : MonoBehaviour
{

    public GameManager gameManager;
    public GameObject playerGameObject;
    public GameObject cameraGameObject;
    private PlayerMovement _playerMovement;
    private CameraMovement _cameraMovement;
    public Animator animator;

    private void Awake()
    {
        _playerMovement = playerGameObject.GetComponent<PlayerMovement>();
        _cameraMovement = cameraGameObject.GetComponent<CameraMovement>();
        animator = playerGameObject.GetComponent<Animator>();
    }
    
    private void OnTriggerEnter(Collider collider) 
    {
        if (collider.gameObject.tag == "Player")
        {
            // Stop Player Forward Movement
            _playerMovement.SetMovePlayerEnabledFalse();

            // Play animation and trigger camera change
            _cameraMovement.SetPlayCameraRotateAnimationTrue();
            animator.Play("CatCelebrating", 0, 0f);

            // Resume player Forward movement after wait
            StartCoroutine(WaitThenEnableMovePlayer());
            StartCoroutine(WaitThenDisablePlayCameraRotate());
        }
    }

    IEnumerator WaitThenEnableMovePlayer()
    {
        yield return new WaitForSeconds(1);
        _playerMovement.SetMovePlayerEnabledTrue();
    }

    IEnumerator WaitThenDisablePlayCameraRotate()
    {
        yield return new WaitForSeconds(4);
        _cameraMovement.SetPlayCameraRotateAnimationFalse();
    }
}
