using System.Collections;
using System.Drawing;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class TutorialScript : MonoBehaviour
{
    [SerializeField] private Camera Main;
    [SerializeField] private RawImage screenFadeImage;
    [SerializeField] private DialoguePlayer dialogue;
    private float elapsedFadeTime;
    private bool timerStarted;
    private bool bowEnabled;

    public bool AwaitingCalibration;

    [SerializeField] private GameObject BowObj;
    [SerializeField] private GameObject BowPullbackSlider;
    [SerializeField] private GameObject Communicator;
    [SerializeField] Vector3 CommunicatorRotation;
    [SerializeField] private AudioSource currentBackgroundMusic;

    [SerializeField] private float FadeTime;

    [SerializeField] private ParticleSystem roomParticles;
    [SerializeField] private ParticleSystem floorParticles;

    [SerializeField] private TutStare tutorialStare;
    [SerializeField] private GameManager gMan;
   

    [System.Serializable]
    public class RotatingObject
    {
        public GameObject objToRotate;
        public Vector3 AxisToRotateAround;
        public float RotationSpeed;

        public void UpdateRotateObject()
        {
            objToRotate.transform.RotateAround(objToRotate.transform.position, AxisToRotateAround, 10.0f * RotationSpeed * Time.deltaTime);
        }
        public IEnumerator LerpRotationSpeed(float endSpeed, float delay = 0)
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }
            float t = new float();
            float orgRotSpeed = RotationSpeed;
            while (t < 1)
            {
                RotationSpeed = Mathf.Lerp(orgRotSpeed, endSpeed, t);
                t += 1.0f * Time.deltaTime; 
                yield return null;
            }
            yield return null;
        }
        public IEnumerator SlerpSize(float scale, float time)
        {
            float t = new float();
            Vector3 origScale = objToRotate.transform.localScale;
            Vector3 endScale = Vector3.one * scale;
            while (t < time)
            {
                objToRotate.transform.localScale = Vector3.Slerp(origScale, endScale, t/time);
                t += (1.0f * Time.deltaTime);
                yield return null;
            }
            yield return null;
        }
    }

    public List<RotatingObject> objs = new List<RotatingObject>();

    [Header("StepObjs")]
    [SerializeField] private GameObject[] _TeleportPlatforms;
    [SerializeField] private GameObject _TeleportGoal;
    [SerializeField] private GameObject _EnemyToSpawn;

    TestBowBehaviour bowBehaviour;
    bool hasReachedTeleportGoal;
    [SerializeField] private float _startPitch;
    [SerializeField] private GameObject playerUIObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Fade(new Color32(0, 0, 0, 255), new Color32(0, 0, 0, 0), 5, 1, FadeTime));
        dialogue.ReadNextDoc(2.5f + FadeTime);
        currentBackgroundMusic = FindFirstObjectByType<MusicChanger>().GetComponent<AudioSource>();
        //currentBackgroundMusic.volume = 0.2f;
        currentBackgroundMusic.pitch = _startPitch;
        bowBehaviour = BowObj.GetComponent<TestBowBehaviour>();
        playerUIObj.SetActive(false);
        //bowBehaviour.ToggleBowInputs();
    }

    public void StartRoomTransition()
    {
        StartCoroutine(objs[0].SlerpSize(55, 3));
        StartCoroutine(objs[0].LerpRotationSpeed(50));
        StartCoroutine(objs[0].LerpRotationSpeed(1, 1.0f));
        UnityEngine.Color orgMainColor = Main.backgroundColor;
        StartCoroutine(Fade(orgMainColor, UnityEngine.Color.white, 1.5f, 0));
        StartCoroutine(Fade(UnityEngine.Color.white, UnityEngine.Color.black, 1.5f, 2));

        dialogue.SetTextColor(UnityEngine.Color.black);
        dialogue.ReadNextDoc(4);

        floorParticles.gameObject.SetActive(true);
        StartCoroutine(LerpSound(2.0f, 0.61f, 0.3f));

    }
    private void tempInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space) && AwaitingCalibration)
        {
            FinishedBowCalibration();
            gMan.CanToggleMenu = true;
        }
        /*
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartRoomTransition();
        }*/
    }
    // Update is called once per frame
    void Update()
    {
        if (timerStarted)
        {
            elapsedFadeTime += 1.0f * Time.deltaTime;
        }
        //MoveCommunicator();
        if (objs.Count > 0)
        {
            foreach (RotatingObject obj in objs)
            {
                obj.UpdateRotateObject();
            }
        }
        tempInputs();
    }
    public void FinishedBowCalibration() 
    {
        dialogue.ReadNextDoc();
        ToggleBow();
        AwaitingCalibration = false;

    }
    void MoveCommunicator()
    {
        Communicator.transform.RotateAround(Communicator.transform.position, Vector3.forward, 10.0f * Time.deltaTime);
        Communicator.transform.RotateAround(Communicator.transform.position, Vector3.right, 10.0f * Time.deltaTime);
    }
    public void ToggleBow()
    {
        bowEnabled = !bowEnabled;
        Main.GetComponent<CameraBehaviour>().ToggleCameraRotation();
        //BowObj.GetComponent<TestBowBehaviour>().CanUpdateBowInputs = bowEnabled;
        playerUIObj.SetActive(true);
        BowObj.GetComponent<TestBowBehaviour>().ToggleBowInputs();
        //BowPullbackSlider.SetActive(bowEnabled);
    }
    public IEnumerator LerpSound(float time, float endPitch, float endVolume)
    {
        float startPitch = currentBackgroundMusic.pitch;
        float startVolume = currentBackgroundMusic.volume;
        float t = new float();
        float newPitch = new float();
        float newVolume = new float();
        while (t < time)
        {
            t += 1.0f * Time.deltaTime;
            float fixedT = t / time;
            newPitch = Mathf.Lerp(startPitch, endPitch, fixedT);
            newVolume = Mathf.Lerp(startVolume, endVolume, fixedT);
            currentBackgroundMusic.pitch = newPitch;
            currentBackgroundMusic.volume = newVolume;
            yield return null;
            
        }
        yield return null;
    }
    public void ActivateStareObj()
    {
        tutorialStare.gameObject.SetActive(true);
        tutorialStare.IsAwaitingStare = true;
    }
    public void StartTutorialStep(int step)//Steps after stare
    {
        if (step == 4) //Activates platform to teleport to.
        {
            for (int i = 0; i < _TeleportPlatforms.Length; i++)
            {
                _TeleportPlatforms[i].SetActive(true);
            }
        }
        if (step == 5)
        {
            _EnemyToSpawn.SetActive(true);
            //_EnemyToSpawn.GetComponent<NewEnemyBehaviour>().spee
            bowBehaviour.UpdateAggros();
            bowBehaviour.ToggleBowInputs();
            dialogue.ReadNextDoc(5);
            //bowBehaviour.CanUpdateBowInputs = false;
            Debug.Log("IS GONNA SPAWN ENEMIES");
        }
        if (step == 6)
        {
            bowBehaviour.ToggleBowInputs();
        }
    }
    public void OnTeleport(Vector3 pos)
    {
        Collider[] colls = Physics.OverlapSphere(pos, 5);
        if (colls.Length > 0 && colls[0] != null)
        {
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i].gameObject == _TeleportGoal && !hasReachedTeleportGoal)
                {
                    Debug.Log("Teleported To the right place!");
                    dialogue.ReadNextDoc(); //"Great!
                    hasReachedTeleportGoal = true;
                }
            }
        }
    }
    

    IEnumerator Fade(Color32 colorA, Color32 colorB, float time,  int reason, float delay = 0)
    {
        var main = roomParticles.main.startColor;

        if (delay != 0)
        {
            Debug.Log("Delayed " + delay + " seconds before starting fade.");
            if (reason == 0)
            {
                Main.backgroundColor = colorA;
            }
            if (reason == 1)
            {
                screenFadeImage.color = colorA;
            }
            if (reason == 2)
            {
               // var main = roomParticles.main.startColor;
                main.color = colorA; 
            }
            yield return new WaitForSeconds(delay);
        }
        UnityEngine.Debug.Log("Started Fade");
        while (true)
        {
            elapsedFadeTime += 1.0f * Time.deltaTime;
            float t = Mathf.InverseLerp(0, time, elapsedFadeTime);
            if (t >= 1)
            {
                elapsedFadeTime = 0;
                UnityEngine.Debug.Log("Ended Fade");
                
                break;
            }
            else
            {
                Color32 newColor = Color32.Lerp(colorA, colorB, t);
                if (reason == 0)
                {
                    Main.backgroundColor = newColor;
                }
                if (reason == 1)
                {
                    screenFadeImage.color = newColor;
                }
                if (reason == 2)
                {
                    main.color = newColor;
                }

            }

            yield return null;
        }
        
    }
    
}
