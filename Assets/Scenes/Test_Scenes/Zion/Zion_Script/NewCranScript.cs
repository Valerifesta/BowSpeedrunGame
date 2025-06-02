using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NewCranScript : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private Vector3 rotationDirection = new Vector3(0, 180, 0);
    [SerializeField] private float rotationDuration = 3f;
    [SerializeField] private float waitTime = 2f;

    [Header("Random Timer Settings")]
    [SerializeField] private float minRotationInterval = 4f;
    [SerializeField] private float maxRotationInterval = 6f;

    [Header("Animation Settings")]
    [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // <-------

    private Quaternion originalRotation;
    public bool isRotating = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalRotation = transform.rotation;
        StartCoroutine(RandomRotationTimer());
    }

    private IEnumerator RandomRotationTimer()
    {
        while (true)
        {
            //Wait at random time between rotations
            float randomWaitTime = Random.Range(minRotationInterval, maxRotationInterval);
            yield return new WaitForSeconds(randomWaitTime);
          
            // Start the rotation if we don´t already rotating
            if (!isRotating)
            {
                StartCoroutine(RotateObject());
            }

        }
    }

    private IEnumerator RotateObject()
    {
        isRotating = true;

        // Random rotation (Either the original direction or reverse)
        Vector3 randomRotation = Random.value > 0.5f ? rotationDirection : -rotationDirection;
        Quaternion targetRotation = Quaternion.Euler(randomRotation) * originalRotation;


        // Rotate at the target-position
        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            float progress = elapsed / rotationDuration;
            float curveValue = rotationCurve.Evaluate(progress);
            transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, curveValue);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;

        yield return new WaitForSeconds(waitTime);

        //Rotate back to original-position
        elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            float progress = elapsed / rotationDuration;
            float curveValue = rotationCurve.Evaluate(progress);
            transform.rotation = Quaternion.Lerp(targetRotation, originalRotation, curveValue);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = originalRotation;

        isRotating = false;


    }

    // Update is called once per frame
    void Update()
    {

    }
}
