using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExploEffekt : MonoBehaviour
{
    [Header("Material")]
    [SerializeField] private Material DissolveMat;
   // [SerializeField] private Material ExplosionMat;
    
    MeshRenderer meshRenderer;
    [Header("Settings")]
    [SerializeField] private float transitionDuration = 4f; 
    [SerializeField] private float startDelay = 0f;
    [SerializeField] private bool isDestroying;
    [SerializeField] private float timeToDisapearing;

    [SerializeField] private float currentDissolveValue = 1f;
  //  private float currentExplosionDistance = 2f;
    private bool effectStarted = false;

    [Header("Reference")]
    [SerializeField] private NewEnemyBehaviour NEB;

   //public MeshRenderer Renderer { get => meshRenderer; set => meshRenderer = value; }//alt1

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //NEB = FindAnyObjectByType<NewEnemyBehaviour>();
       // NEB = GetComponentInParent<NewEnemyBehaviour>();
        //Renderer = GetComponent<MeshRenderer>();//alt1
        meshRenderer = GetComponent<MeshRenderer>();
        aliveYet();

        
    }

    public void aliveYet()
    {
        
        if(isDestroying == true)
        {
            //Renderer.material = DissolveMat;//alt1
           // Renderer.material = ExplosionMat;//alt1
                                             // ActionCoroutine();
            meshRenderer.material = DissolveMat;//alt1
           // meshRenderer.material = ExplosionMat;

          //  ExplosionMat.SetFloat("_Distance", 2);
           // DissolveMat.SetFloat("_Dissolve", 0);
           
        }
        else
        {
            isDestroying = false;
        }
    }

    public void killMe()
    {
        if (NEB.isKillingEnemy == true)
        {
            isDestroying = true;
        }
        else
        {
            isDestroying = false;
        }
        StartEffect();
    } 
    public void StartEffect()
    {
        if (!effectStarted)
        {
            effectStarted = true;
            // Renderer.material = DissolveMat;//alt1
            meshRenderer.material = DissolveMat;
            StartCoroutine(TransitionEffect());
            print("StartCoroutine started");////////////////////////////////TEMP_PRINT///////////////////////////
        }
    }


    private IEnumerator TransitionEffect()
    {
        print("TransitionEffect started");////////////////////////////////TEMP_PRINT///////////////////////////
        yield return new WaitForSeconds(startDelay);
        print("TransitionEffect after startDelay");////////////////////////////////TEMP_PRINT///////////////////////////
        float elapsedTime = 0f;

        // Sätt startvärden
        currentDissolveValue = 0f;
       // currentExplosionDistance = 2f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

           
            currentDissolveValue = Mathf.Lerp(1f, 0f, t);
           // currentExplosionDistance = Mathf.Lerp(2f, 0f, t);

           
            DissolveMat.SetFloat("_Dissolve", currentDissolveValue);
            //ExplosionMat.SetFloat("_Distance", currentExplosionDistance);

            yield return null;
        }

        // Säkerställ att vi når slutvärdena
       // DissolveMat.SetFloat("_Dissolve", 1f);
       // ExplosionMat.SetFloat("_Distance", 0f);

        
        Destroy(gameObject);
        print("gameObject destroyed in enemy");////////////////////////////////TEMP_PRINT///////////////////////////
    }

    
    /* private IEnumerator ActionCoroutine()
     {
         yield return new WaitForSeconds(1f);
         float t = 0;
         while (t < 1f)
         {
             t += Time.deltaTime / 2f;
            propertyBlock.SetFloat("_Dissolve", t);
             for (int i = 0; i < meshRenderer.Length; i++)
                 meshRenderer[i].SetPropertyBlock(propertyBlock);
             yield return null;


         }
         Destroy(gameObject);
     }*/
    // Update is called once per frame
    void Update()
    {
        aliveYet();
        killMe();
    }
}
