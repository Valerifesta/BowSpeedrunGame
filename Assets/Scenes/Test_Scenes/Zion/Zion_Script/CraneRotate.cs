using UnityEngine;
using System.Collections;

public class CraneRotate : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private Vector3 rotationDirection = new Vector3(0, 180, 0); // Rotation in degrees
    [SerializeField] private float rotationDuration = 3f; // How long the rotation takes
    [SerializeField] private float waitTime = 2f; // Time to wait before rotating back

    [Header("Animation Settings")]
    [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Quaternion originalRotation;
    private bool isRotating = false;

    private void Start()
    {
        // Store the initial rotation
        originalRotation = transform.rotation;
    }

    public void StartRotation()
    {
        if (!isRotating)
        {
            StartCoroutine(RotateObject());
        }
    }

    private IEnumerator RotateObject()
    {
        isRotating = true;

        // Store target rotation
        Quaternion targetRotation = Quaternion.Euler(rotationDirection) * originalRotation;

        // Rotate to target
        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            float progress = elapsed / rotationDuration;
            float curveValue = rotationCurve.Evaluate(progress);

            transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, curveValue);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure we reach exact target rotation
        transform.rotation = targetRotation;

        // Wait specified time
        yield return new WaitForSeconds(waitTime);

        // Rotate back to original position
        elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            float progress = elapsed / rotationDuration;
            float curveValue = rotationCurve.Evaluate(progress);

            transform.rotation = Quaternion.Lerp(targetRotation, originalRotation, curveValue);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure  original rotation
        transform.rotation = originalRotation;

        isRotating = false;
    }

    // Optional: Rotate when space key is pressed
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartRotation();
        }
    }
}
