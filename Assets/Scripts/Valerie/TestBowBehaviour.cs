using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class TestBowBehaviour : MonoBehaviour
{
    [SerializeField] private float _maxRotaryValue;
    [SerializeField] private float _currentRotaryValue;
    [SerializeField] private float _degSecReleaseRequirement; //Adapt to controller
    private bool _activeRelease;
    private float _rotaryValueOnRelease;

    [SerializeField] private GameObject ObjectToRotate;
    [SerializeField] private float Sensitivity;
    [SerializeField] private Vector3 dir;
    Vector3 rot;

    private bool _teleportArrowToggled;

    [SerializeField] private GameObject ArrowPrefab;
    [SerializeField] private Camera MainCam;
    public GameObject Player;
    [SerializeField] private Slider RotaryIndicator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ObjectToRotate.transform.eulerAngles += rot * Sensitivity * Time.deltaTime;

        temp_inputs();
        UpdateRotaryIndicator();
    }

    public void temp_inputs()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !_activeRelease)
        {
            UpdateRotaryValue(10);
        }
        else 
        {
            if (_rotaryValueOnRelease != 0.0f)
            {
                UpdateRotaryValue(_rotaryValueOnRelease * -10);
                print("value is " + _rotaryValueOnRelease * -10);
            }
            else
            {
                UpdateRotaryValue(-80); //Standard release value

            }
        }
    }
    /*
    public void SetMaxRotaryDegrees(int degrees)
    {
        _maxDegrees = degrees;

    }*/
    public void ToggleArrow()
    {
        _teleportArrowToggled = !_teleportArrowToggled;
    }
    public void UpdateRotaryValue(float rotaryInput)
    {
        if (rotaryInput < -_degSecReleaseRequirement && _currentRotaryValue > 0 && !_activeRelease)
        {
            _rotaryValueOnRelease = _currentRotaryValue;
            _activeRelease = true;
        }
        float valueToAdd = rotaryInput * Time.deltaTime;
        float nextValue = _currentRotaryValue + valueToAdd;
        if (nextValue > _currentRotaryValue)
        {
            if (nextValue < _maxRotaryValue)
            {
                _currentRotaryValue += valueToAdd;
                Debug.Log("Increased rotation");
            }
            else
            {
                _currentRotaryValue = _maxRotaryValue;
                Debug.Log("Reached max rotation value");
            }
        }
        else if (nextValue < _currentRotaryValue)
        {
            if (nextValue > 0.0f)
            {
                _currentRotaryValue += valueToAdd;
                Debug.Log("Decreased rotation");
            }
            else
            {
                _currentRotaryValue = 0.0f;
                Debug.Log("Reached minimum rotation value");

                if (_activeRelease)
                {
                    if (rotaryInput < -_degSecReleaseRequirement && Mathf.Abs(rotaryInput) > 0.5f)
                    {
                        Shoot(_rotaryValueOnRelease);

                    }
                    else
                    {
                        Debug.Log("Didnt assert enough force to shoot");
                    }
                    _rotaryValueOnRelease = 0.0f;
                    _activeRelease = false;
                }
            }

        }
        
        
    
    }
    void UpdateRotaryIndicator()
    {
        RotaryIndicator.value = _currentRotaryValue / 10;
    }

    public void TeleportPlayer(Vector3 position)
    {
        Player.transform.position = position;
    }
    public void Shoot(float valueOnRelease)
    {
        Debug.Log("Shot");
        GameObject arrow = Instantiate(ArrowPrefab);
        ArrowBehaviour behaviour = arrow.GetComponent<ArrowBehaviour>();
        Rigidbody arrowRigidbody = arrow.GetComponent<Rigidbody>();

        arrow.transform.up = MainCam.transform.forward;
        arrow.transform.position = MainCam.transform.position;
        behaviour.sender = this;
        behaviour.teleportToggled = _teleportArrowToggled;
        arrowRigidbody.AddForce(arrow.transform.up * 100 * valueOnRelease);
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
