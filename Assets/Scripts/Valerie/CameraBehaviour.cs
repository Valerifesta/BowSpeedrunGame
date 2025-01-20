using UnityEngine;
using UnityEngine.InputSystem;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Camera MainCam;
    [SerializeField] private float Sensitivity;
    [SerializeField] private bool LockCursor;
    Vector3 currentRot;
    public bool TempMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (LockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TempMovement)
        {
            tempCamMovement();
        }
        //
    }
    void tempCamMovement()
    {
        Vector3 dir = new Vector3(Input.GetAxisRaw("Mouse Y"), -Input.GetAxisRaw("Mouse X"));
        Vector3 valueToAdd = -dir * Sensitivity * Time.deltaTime;
        currentRot += valueToAdd;
        currentRot.x = Mathf.Clamp(currentRot.x, -90, 90);

        MainCam.transform.rotation = Quaternion.Euler(currentRot);
       
    }
}
