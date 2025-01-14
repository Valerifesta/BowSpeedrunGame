using UnityEngine;
using System.Collections;

public class CraneRotate : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private Vector3 rotationDirection = new Vector3(0, 180, 0);
    [SerializeField] private float rotationDuration = 3f;
    [SerializeField] private float waitTime = 2f;

    [Header("Random Timer Settings")]
    [SerializeField] private float minRotationInterval = 4f;
    [SerializeField] private float maxRotationInterval = 6f;

    [Header("Animation Settings")]
    [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Quaternion originalRotation;
    private bool isRotating = false;

    private void Start()
    {
        originalRotation = transform.rotation;
        StartCoroutine(RandomRotationTimer());
    }

    private IEnumerator RandomRotationTimer()
    {
        while (true)
        {
            // Vänta en slumpmässig tid mellan rotationer
            float randomWaitTime = Random.Range(minRotationInterval, maxRotationInterval);
            yield return new WaitForSeconds(randomWaitTime);

            // Starta rotation om vi inte redan roterar
            if (!isRotating)
            {
                StartCoroutine(RotateObject());
            }
        }
    }

    private IEnumerator RotateObject()
    {
        isRotating = true;

        // Slumpmässig rotation (antingen original riktning eller motsatt)
        Vector3 randomRotation = Random.value > 0.5f ? rotationDirection : -rotationDirection;
        Quaternion targetRotation = Quaternion.Euler(randomRotation) * originalRotation;

        // Rotera till målposition
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

        // Rotera tillbaka till originalposition
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

    // Optional: Stoppa alla rotationer
    public void StopRotations()
    {
        StopAllCoroutines();
        isRotating = false;
    }

    // Optional: Starta om rotationerna
    public void RestartRotations()
    {
        StopRotations();
        StartCoroutine(RandomRotationTimer());
    }
}