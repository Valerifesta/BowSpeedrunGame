using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExploEffekt : MonoBehaviour
{
    [SerializeField] private Material DissolveMat;
    [SerializeField] private Material ExplosionMat;
    [SerializeField] private bool isDestroying;
    [SerializeField] private float timeToDisapearing;
    MeshRenderer renderer;


    public MeshRenderer Renderer { get => renderer; set => renderer = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Renderer = GetComponent<MeshRenderer>();
        aliveYet();
    }

    public void aliveYet()
    {
        
        if(isDestroying == true)
        {
            Renderer.material = DissolveMat;
            Renderer.material = ExplosionMat;
           // ActionCoroutine();

            ExplosionMat.SetFloat("_Distance", 2);
            DissolveMat.SetFloat("_Dissolve", 1);
           
        }
        else
        {
            isDestroying = false;
        }
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
    }
}
