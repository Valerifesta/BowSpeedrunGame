using UnityEngine;

public class ScriptActivate : MonoBehaviour
{
    private MasterMind mm;

    [SerializeField]
    private MonoBehaviour targetScript;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mm = FindAnyObjectByType<MasterMind>();
        if (targetScript != null)
        {
            targetScript.enabled = false;
        }
       
    }

    public void ActivateTargetScript()
    {
        if (targetScript != null)
        {
            targetScript.enabled = true;
            Debug.Log($"Activated script: {targetScript.GetType().Name}");
        }
        else
        {
            Debug.LogWarning("No target script assigned!");
        }
    }
   

    private void Update()
    {
        if(mm.IntroSceneOn == false)
        {
            Invoke("ActivateTargetScript", 4);
           
        }
        
    }
   
}
