using UnityEditor.Rendering;
using UnityEngine;

public class TutStare : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Camera main;
    Plane[] frustum;
    Bounds goalBounds;
    [SerializeField] private GameObject StareGoalObj;
    public bool IsAwaitingStare;
    [Header("Time Values")]
    [SerializeField] private float timeStared;
    [SerializeField] private float _TimeToGetChecked;

    [Header("Materials")]
    [SerializeField] private Material Blue;
    [SerializeField] private Material Green;
    private Material currentMaterial;

    private Vector3 _blueVec;
    private Vector3 _greenVec;
    private MeshRenderer coreRenderer;

    [Header("Managers")]
    [SerializeField] private TutorialScript _Tutorial;
    [SerializeField] private DialoguePlayer _Dialogue;

    void Start()
    {
        main = Camera.main;
        coreRenderer = StareGoalObj.GetComponent<MeshRenderer>();
        currentMaterial = new Material(Blue);
        coreRenderer.material = currentMaterial;

        _blueVec = new Vector3(Blue.color.r, Blue.color.g, Blue.color.b);
        _greenVec = new Vector3(Green.color.r, Green.color.g, Green.color.b);

    }

    // Update is called once per frame
    void Update()
    {
        if (IsAwaitingStare)
        {
            if (isWithinFrustumBounds())
            {
                timeStared += 1.0f * Time.deltaTime;
                tryUpdateMaterial();
                
                //print(StareGoalObj.name + " is within players view!");
            }
            else if (timeStared > 0)
            {
                timeStared = 0;
                tryUpdateMaterial();
            }

            if (timeStared > _TimeToGetChecked)
            {
                GetChecked();
                ResetValues();
            }
        }
    }
    bool isWithinFrustumBounds()
    {
        frustum = GeometryUtility.CalculateFrustumPlanes(main);
        goalBounds = StareGoalObj.GetComponent<Collider>().bounds;
        
        return GeometryUtility.TestPlanesAABB(frustum, goalBounds);
    }
    void GetChecked()
    {
        print(StareGoalObj.name + " got stare checked!");
        _Dialogue.ReadNextDoc();
        StareGoalObj.SetActive(false);
    }
    void ResetValues()
    {
        timeStared = 0;
        IsAwaitingStare = false;
    }
    
    void tryUpdateMaterial() //For some reason it always goes to red..?
    {
        float t = Mathf.InverseLerp(0.0f, _TimeToGetChecked, timeStared);

        Vector3 colorVector = Vector3.Lerp(_blueVec, _greenVec, t);
        Color newColor = new Color(colorVector.x, colorVector.y, colorVector.z);
        currentMaterial.color = newColor;
        currentMaterial.SetColor("_EmissionColor", newColor);
    }
}
