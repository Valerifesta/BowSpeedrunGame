using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class TestBowBehaviour : MonoBehaviour
{
    [SerializeField] private float _maxRotaryValue;
    [SerializeField] private float _currentRotaryValue;
    [SerializeField] private float _degSecReleaseRequirement; //Adapt to controller. 363.6 without  controller.
    private bool _activeRelease;
    private float _rotaryValueOnRelease;

   
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
    private CameraBehaviour camBehaviour;

    public PlayerManager playerManager;
    [SerializeField] private GameManager GMan;

    public bool tempInputs = false;
    

    float lastrotaryvalue = 0;
    bool shoot = false;

    public bool CanUpdateBowInputs;

    //public int ArrowsFired;
    //public int TimesTeleported;
    public List<NewEnemyBehaviour> previousEnemies = new List<NewEnemyBehaviour>();
    //[SerializeField] private GameManager PlayerUI;
    [Header("Arrow Icons")]
    [SerializeField] private GameObject AttackIcon;
    [SerializeField] private GameObject TeleportIcon;
    [SerializeField] private Color ActiveColor;
    [SerializeField] private Color InactiveColor;


    private GameObject[] arrowIcons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camBehaviour = MainCam.GetComponent<CameraBehaviour>();

        _teleportArrowToggled = false;
        arrowIcons = new GameObject[] { AttackIcon, TeleportIcon };
        SetArrowIconVisible();
    }

    // Update is called once per frame
    
    void Update()
    {
        

        //^this is for camera. Should prolly move later.
        if (CanUpdateBowInputs)
        {
            if (tempInputs)
            {
                temp_inputs();
            }


            UpdateRotaryIndicator(); //visual slider

            if (shoot && currentCD < 0 && time_wait < 0.2)
            {
                if (lastrotaryvalue > 1)
                {
                    currentCD = shootCD;
                    Shoot(lastrotaryvalue);
                }
                shoot = false;
            }
            time_wait -= (float)Time.deltaTime;
            currentCD -= (float)Time.deltaTime;
        }
        
    }
    
    public void temp_inputs() 
    {
        if (Input.GetKey(KeyCode.Mouse0) && !_activeRelease && currentCD <= 0)
        {
            UpdateRotaryValue(10);
        }
        else 
        {
            if (_rotaryValueOnRelease != 0.0f)
            {
                UpdateRotaryValue(_rotaryValueOnRelease * -1);
                print("value is " + _rotaryValueOnRelease * -1);
            }
            else if (_currentRotaryValue > 0)
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
        SetArrowIconVisible();
        Debug.Log("Toggled arrow");

    }
    void SetArrowIconVisible() //should always be called AFTER a toggle has happened.
    {
        if (_teleportArrowToggled)
        {
            AttackIcon.SetActive(false);
            TeleportIcon.SetActive(true);
        }
        else
        {
            AttackIcon.SetActive(true);
            TeleportIcon.SetActive(false);
        }
    }
    void SetArrowIconColor(bool canTakeInputs)
    {
        Color newColor = new Color();
        if (canTakeInputs)
        {
            newColor = ActiveColor;
        }
        else
        {
            newColor = InactiveColor;
        }
        for (int i = 0; i < arrowIcons.Length; i++)
        {
            arrowIcons[i].GetComponent<Image>().color = newColor;
        }
    }
    public void ToggleBowInputs()
    {
        CanUpdateBowInputs = !CanUpdateBowInputs;
        SetArrowIconColor(CanUpdateBowInputs);
        Debug.Log("Toggled Bow Inputs");

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
        float t = Mathf.InverseLerp(0, _maxRotaryValue, _currentRotaryValue);
        RotaryIndicator.value = t;
        camBehaviour.ChangeFOV(60.0f, 55.0f, t);

    }

    public void TeleportPlayer(Vector3 position)
    {
        teleportManager.Teleport(Player, position);
        teleportManager.UpdateTeleportArray(position);
        GMan.TimesTeleported += 1;

        UpdateAggros();
    }
    public void UpdateAggros()
    {
        //Check if there are enemies within bounding volume
        DetectSurroundingEnemies();

        float elaspedShield = playerManager.ShieldTimeRemaining;
        if (elaspedShield > 0)
        {
            StunSurroundingtEnemies(elaspedShield);
        }
        else
        {
            AggroSurroundingEnemies();
        }
    }
    public Collider[] GetNearestEnemyColliders()
    {
        return Physics.OverlapSphere(Player.transform.position, 10, ~LayerMask.NameToLayer("Enemy"));
        
    }
    public void DetectSurroundingEnemies()
    {
        Collider[] colls = GetNearestEnemyColliders();
        NewEnemyBehaviour[] behaviourArray = new NewEnemyBehaviour[colls.Length];
        
        if (colls.Length != 0)
        {
            for (int i = 0; i < colls.Length; i++)
            {
                GameObject enemyParent = colls[i].gameObject.transform.parent.gameObject;
                //dist = Vector3.Distance(Player.transform.position, enemyParent.transform.position);
                NewEnemyBehaviour behaviour = enemyParent.GetComponent<NewEnemyBehaviour>();
                Debug.Log(" behaviour is " +behaviour);
                behaviourArray[i] = behaviour;
                if (!previousEnemies.Contains(behaviourArray[i]))
                {
                    previousEnemies.Add(behaviour);
                }
                Debug.Log(enemyParent);
            }
        }

        NewEnemyBehaviour[] enemiesToDeaggro = new NewEnemyBehaviour[previousEnemies.Count];

        for (int i = 0; i < previousEnemies.Count; i++)
        {
            Debug.Log("Deciding Aggro");
            if (behaviourArray.Contains(previousEnemies[i]))
            {
                Debug.Log(previousEnemies[i].gameObject.name + " was inside of the detection bounds. starting aggro");
            }
            else
            {
                Debug.Log(previousEnemies[i].gameObject.name + " was outside of the detection bounds. Added enemy to 'enemiesToDeaggro' array");
                enemiesToDeaggro[i] = previousEnemies[i];
            }
        }
        for (int i = 0; i < enemiesToDeaggro.Length; i++) //This deaggros all enemies within the "enemiesToDeaggro" array. 
        {
            if (enemiesToDeaggro[i] != null)
            {
                previousEnemies.Remove(enemiesToDeaggro[i]);
                enemiesToDeaggro[i].StartIdle();
               // Debug.Log("Deaggrod " + enemiesToDeaggro[i]);
            }
            
        }
    }
    private void AggroSurroundingEnemies()
    {
        foreach (NewEnemyBehaviour behaviour in previousEnemies)
        {
            behaviour.TargetPlayer(Vector3.Distance(Player.transform.position, behaviour.gameObject.transform.position));
        }
    }
    private void StunSurroundingtEnemies(float remainingStunTime)
    {
        foreach (NewEnemyBehaviour behaviour in previousEnemies)
        {
            if (!behaviour.IsStunned)
            {
                behaviour.StunEnemy(remainingStunTime);
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
        GMan.ArrowsShot += 1;

    }
 

}
