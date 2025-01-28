using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OutlineRules : MonoBehaviour
{
   
    //This is Main Camera in the Scene
    Camera m_MainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_MainCamera = Camera.main;
    }

    private void NoOutLine()
    {
        LayerMask mask = LayerMask.GetMask("IgnoreOutLine");
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
