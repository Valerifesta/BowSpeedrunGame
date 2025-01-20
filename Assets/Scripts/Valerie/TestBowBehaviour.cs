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

    bool decreasing;
    float max_time_wait = 0.2f;
    float time_wait; 

    private bool _teleportArrowToggled;

    [SerializeField] float shootCD; 
    float currentCD;
    float deltaloss = 0;
    [SerializeField] private GameObject ArrowPrefab;
    [SerializeField] private Camera MainCam;
    public GameObject Player;
    [SerializeField] private Slider RotaryIndicator;
    [SerializeField] private TeleportManager teleportManager;

    public PlayerManager playerManager;
    [SerializeField] private GameManager GMan;

    public bool tempInputs = false;
    
    bool reset = false;

    float lastrotaryvalue = 0;
    bool shoot = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _teleportArrowToggled = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 sense = GameSettings.CameraSensitivity;
        Vector3 sense = new Vector3(10,10,10);
        ObjectToRotate.transform.eulerAngles += new Vector3(rot.x * sense.x ,rot.y * sense.y, rot.z * sense.z) * Time.deltaTime;
        if(reset){
            reset = false;
            ObjectToRotate.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        if (tempInputs)
        {
            temp_inputs();
        }
        if(Input.GetKeyDown(KeyCode.R)){
            ObjectToRotate.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        UpdateRotaryIndicator();

        if(shoot && currentCD < 0 && time_wait < 0.2){
          if(lastrotaryvalue > 1){
            currentCD = shootCD;
            Shoot(lastrotaryvalue);
          }
          shoot = false;
        }
        time_wait -= (float)Time.deltaTime;
        currentCD -= (float)Time.deltaTime;
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleArrow();
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
        if(shoot && time_wait > 0){
          shoot = false;
        }
        time_wait = max_time_wait;
        if(rotaryInput < 0){
          deltaloss += Mathf.Abs(rotaryInput);
        }
        else{
          deltaloss = 0;
        }
        float valueToAdd = rotaryInput;
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

            if (deltaloss > _degSecReleaseRequirement) {
              lastrotaryvalue = _currentRotaryValue;
              Debug.Log(lastrotaryvalue);
              shoot = true;
              Debug.Log("Shooting");
              _rotaryValueOnRelease = 0.0f;
              _currentRotaryValue = 0.0f;

            }
            else
            {
                _currentRotaryValue += valueToAdd;
                Debug.Log("Didnt assert enough force to shoot");
            }
            Debug.Log("Reached minimum rotation value");

        }
        
        
    
    }
    void UpdateRotaryIndicator()
    {
        RotaryIndicator.value = _currentRotaryValue / 10;
    }

    public void TeleportPlayer(Vector3 position)
    {
        teleportManager.Teleport(Player, position);
        teleportManager.UpdateTeleportArray(position);

        //Check if there are enemies within bounding volume
        TryAggroEnemies();
    }
    public Collider[] GetNearestEnemyColliders()
    {
        return Physics.OverlapSphere(Player.transform.position, 10, ~LayerMask.NameToLayer("Enemy"));
        
    }
    public void TryAggroEnemies()
    {
        Collider[] colls = GetNearestEnemyColliders();
        if (colls.Length != 0)
        {
            for (int i = 0; i < colls.Length; i++)
            {

                GameObject enemyParent = colls[i].gameObject.transform.parent.gameObject;
                float dist = Vector3.Distance(Player.transform.position, enemyParent.transform.position);
                enemyParent.GetComponent<NewEnemyBehaviour>().TargetPlayer(dist);
                Debug.Log(enemyParent);

            }
        }
    }
    public void TryDeaggroEnemies()
    {
        Collider[] colls = GetNearestEnemyColliders();
        if (colls.Length != 0)
        {
            for (int i = 0; i < colls.Length; i++)
            {
                GameObject enemyParent = colls[i].gameObject.transform.parent.gameObject;
                NewEnemyBehaviour behaviour = enemyParent.GetComponent<NewEnemyBehaviour>();
                if (behaviour.CanTargetPlayer)
                {
                    behaviour.StartCoroutine(behaviour.PauseEnemy(playerManager.ElapsedShieldDuration));
                }
            }
        }
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
        behaviour.GameMan = GMan;
        arrowRigidbody.AddForce(arrow.transform.up * 1000 * Mathf.InverseLerp(_degSecReleaseRequirement, _maxRotaryValue, valueOnRelease));
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
    public void ResetRot(){
        reset = true;
    }


}
