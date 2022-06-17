using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public Camera playerCamera;
    public GameObject playerObject;
    public Transform objectIdlePosition, objectInUsePosition;
    [Space]
    public float playerObjectSpeed = 1.0f;

    public float cameraRotateSpeed = 1.0f;
    public float minCameraYRotation = -35.0f;
    public float maxCameraYRotation = 35.0f;
    private float cameraYRotation = 0.0f;

    public bool playerObjectUp = false;
    public float offsetToDecideIfObjectIsUp = 0.2f;

    private Vector3 desiredPosition;

    private void OnEnable()
    {
        GameManager.OnGameReset += ResetPlayer;
    }

    private void OnDisable()
    {
        GameManager.OnGameReset -= ResetPlayer;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        bool upInput = false;

        if(SystemInfo.deviceType == DeviceType.Handheld)
        {
            upInput = Input.touchCount > 0;
        }
        else
        {
            upInput = Input.GetKey(KeyCode.Space);
        }

        //Handle object up and down
        desiredPosition = upInput && GameManager.gameStarted ? objectInUsePosition.position : objectIdlePosition.position;
        HandleObjectPosition(desiredPosition);


        
        //Handle camera horizontal rotation
        var horizontalInput = GameManager.gameStarted ? Input.GetAxis("Horizontal") * Time.deltaTime : 0;
        
        cameraYRotation += horizontalInput * cameraRotateSpeed;
        cameraYRotation = Mathf.Clamp(cameraYRotation, minCameraYRotation, maxCameraYRotation);

        Quaternion desiredRotation = Quaternion.Euler(
            playerCamera.transform.rotation.x,
            cameraYRotation,
            playerCamera.transform.rotation.z
        );

        HandleCameraRotation(desiredRotation);

        
        playerObjectUp = Vector3.Distance(playerObject.transform.position, objectInUsePosition.position) < offsetToDecideIfObjectIsUp;
    }

    private void HandleObjectPosition(Vector3 desiredPosition)
    {
        playerObject.transform.position = Vector3.Slerp(playerObject.transform.position, desiredPosition, Time.deltaTime * playerObjectSpeed);
    }

    private void HandleCameraRotation(Quaternion desiredRotation)
    {
        playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, desiredRotation, Time.deltaTime * cameraRotateSpeed);
    }

    public void ResetPlayer()
    {
        desiredPosition = objectIdlePosition.position;
        cameraYRotation = 0.0f;
    }
}
