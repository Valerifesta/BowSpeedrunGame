using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EasyRail : MonoBehaviour
{
    public GameObject objectToMove;
    public Vector3 direction = Vector3.forward;
    public float speed = 5f;
    public Vector3 originalPos;
    [HideInInspector] public float timer = 0f;

    [Header("Rails Settings")]
    [SerializeField] private List<EasyRail> movingRails = new List<EasyRail>();

    [Header("Movement Settings")]
    [SerializeField] private float distanceToReset = 10f; 
    private bool isPressed = false;

    private void Start()
    {
        originalPos = transform.position;
    }

    private void Update()
    {
        if (isPressed) return; 

        for (int i = movingRails.Count - 1; i >= 0; i--)
        {
            var obj = movingRails[i];
            if (obj.objectToMove == null) continue;

            // Flytta objektet
            Vector3 movement = obj.direction.normalized * obj.speed * Time.deltaTime;
            obj.objectToMove.transform.position += movement;

            // Kontrollera om objektet har åkt tillräckligt långt
            float distanceTraveled = Vector3.Distance(obj.objectToMove.transform.position, obj.originalPos);
            if (distanceTraveled >= distanceToReset)
            {
                obj.objectToMove.transform.position = obj.originalPos;
            }
        }
    }

    private void OnMouseDown()
    {
        isPressed = true; 
    }
}
