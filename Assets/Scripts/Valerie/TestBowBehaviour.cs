using System.Runtime.InteropServices;
using UnityEngine;

public class TestBowBehaviour : MonoBehaviour
{
    private float _maxDegrees;
    private float _currentRotaryValue;
    [SerializeField] private GameObject ObjectToRotate;
    [SerializeField] private float Sensitivity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        ObjectToRotate.transform.eulerAngles += deltaRot;
    }


}
