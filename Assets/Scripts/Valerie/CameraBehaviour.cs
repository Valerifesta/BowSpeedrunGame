using UnityEditor.XR;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI.Table;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Camera MainCam;
    [SerializeField] private float Sensitivity;
    [SerializeField] private bool LockCursor;
    Vector3 currentRot;
    public bool TempMovement;
    public bool CanUpdateCamValues;

    [SerializeField] private GameObject ObjectToRotate;
   
    [SerializeField] private Vector3 dir;
    Vector3 rot;
    bool reset = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ToggleCameraLock();

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleFreeMouse();
        }*/
        if (CanUpdateCamValues)
        {
            if (TempMovement)
            {
                tempCamMovement();
            }
            else
            {
                controllerCamMovement();
            }


            currentRot.x = Mathf.Clamp(currentRot.x, -90, 90);

            MainCam.transform.rotation = Quaternion.Euler(currentRot);

        }

        //
    }
    void tempCamMovement()
    {
        Vector3 dir = new Vector3(Input.GetAxisRaw("Mouse Y"), -Input.GetAxisRaw("Mouse X"));
        Vector3 valueToAdd = -dir * Sensitivity * Time.deltaTime;
        currentRot += valueToAdd;
        

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentRot = new Vector3(0, 180, 0);
        }
    }
    void controllerCamMovement()
    {
        Vector3 sense = GameSettings.CameraSensitivity;

        if (sense == Vector3.zero)
        {
            sense = new Vector3(10, 10, 10);
        }
        currentRot += new Vector3(rot.x * sense.x, rot.y * sense.y, rot.z * sense.z) * Time.deltaTime;
        if (reset)
        {
            reset = false;
            currentRot = new Vector3(0, 180, 0);
        }
    }
    public void UpdateCameraRot(Vector3 deltaRot) // Deg/s
    {
        if (deltaRot.magnitude > 100 || deltaRot.y == 0 && deltaRot.z == 0)
        {
            Debug.Log("lmao: " + deltaRot);

        }
        else
        {
            rot = new Vector3(-deltaRot.y * dir.y, deltaRot.x * dir.x, deltaRot.z * dir.z);
        }
    }
    public void ResetRot()
    {
        reset = true;
    }
    public void ToggleCameraRotation()
    {
        CanUpdateCamValues = !CanUpdateCamValues;
    }
    public void ToggleCameraLock()
    {
        LockCursor = !LockCursor;

        if (LockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

    public void ToggleFreeMouse()
    {
        ToggleCameraRotation();
        ToggleCameraLock();
    }
    public void SaveRotation()
    {
        GameSettings.CameraRotLastScene = MainCam.transform.rotation;
    }
    public void LoadLastSceneRotation()
    {
        currentRot = GameSettings.CameraRotLastScene.eulerAngles;
    }
}
