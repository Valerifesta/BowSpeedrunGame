using UnityEngine;

public class CustomeGravity : MonoBehaviour
{
    [SerializeField] private Transform gravitationTarget; // Objektet som spelaren ska dras mot
    [SerializeField] private float gravitationalForce = 9.81f; // Styrkan p� gravitationen
    [SerializeField] private float rotationSpeed = 10f; // Hur snabbt spelaren roterar f�r att matcha ytan

    private Rigidbody rb;
    private Vector3 gravitationDirection;

    [SerializeField] private bool teleporting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("WalkArea"))
        {
            gravitationTarget = collision.transform;
            //gravitationTarget.CompareTag("WalkArea");
            // this.gameObject.GetComponentInChildren<GameObject>();
            this.gameObject.transform.parent = gravitationTarget.transform;
            //rb.GetComponent<Rigidbody>().isKinematic = true;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
  
    void FixedUpdate()
    {

        if (gravitationTarget == null) return;

        // Ber�kna riktningen mot gravitationsm�let
        gravitationDirection = (gravitationTarget.position - transform.position).normalized;

        // Applicera gravitationskraften
        rb.AddForce(gravitationDirection * gravitationalForce, ForceMode.Acceleration);

      
        // Rotera spelaren s� att "upp" �r motsatt gravitationsriktningen
        RotatePlayerTowardsGravity();
    }

    void RotatePlayerTowardsGravity()
    {
        // Ber�kna �nskad rotation d�r spelarens "upp" �r motsatt gravitationsriktningen
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravitationDirection) * transform.rotation;

        // Mjukt rotera mot m�lrotationen
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
