using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class TestBowBehaviour : MonoBehaviour
{
    private float _maxDegrees;
    private float _currentRotaryValue;
    [SerializeField] private GameObject ObjectToRotate;
    [SerializeField] private float Sensitivity;
    [SerializeField] private Vector3 dir;
    Vector3 rot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ObjectToRotate.transform.eulerAngles += rot * Sensitivity * Time.deltaTime;
    }

    public void SetMaxRotaryDegrees(int degrees)
    {
        _maxDegrees = degrees;

    }

    public void UpdateRotaryValue(int rotaryInput)
    {
        _currentRotaryValue = Mathf.InverseLerp(0.0f, _maxDegrees, rotaryInput);
    }
    public void UpdateCameraRot(Vector3 deltaRot) // Deg/s
    {
        if (deltaRot.magnitude > 100 || deltaRot.y == 0 && deltaRot.z == 0)
        {
            Debug.Log("lmao: " + deltaRot);

        }
        else
        {
            rot = new Vector3(deltaRot.x * dir.x, deltaRot.y * dir.y, deltaRot.z * dir.z);
        }
    }


}
