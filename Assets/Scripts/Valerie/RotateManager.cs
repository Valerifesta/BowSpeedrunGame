//using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Linq;
using System.Collections.Generic;
using static TutorialScript;

public class RotateManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public List<RotatingObject> objs = new List<RotatingObject>();

    // Update is called once per frame
    void Update()
    {
        if (objs.Count > 0)
        {
            foreach (RotatingObject obj in objs)
            {
                obj.UpdateRotateObject();
            }
        }
    }
}
