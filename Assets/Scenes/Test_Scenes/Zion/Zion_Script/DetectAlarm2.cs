using UnityEngine;

public class DectedAlarm2 : MonoBehaviour// its shame to do a same script twice although I know that I can just do an inheritance
{
    [SerializeField] private Material GlowMaterialRed;
    [SerializeField] private Material GlowMaterialGreen;
    [SerializeField] private Material GlowMaterialYellow;
    [SerializeField] private Material GlowMaterialBlue;
    MeshRenderer renderer;

    //private TurrentLevel2 turrentLevel2;
    private NewEnemyBehaviour NEB;
   


    public MeshRenderer Renderer { get => renderer; set => renderer = value; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //turrentLevel2 = GetComponentInParent<TurrentLevel2>();
        NEB = GetComponentInParent<NewEnemyBehaviour>();
        // turrentLevel2 = transform.parent.transform.parent.GetComponent<TurrentLevel2>();
        if (NEB == null)
        {
            Debug.LogError("TurrentLevel2-komponenten saknas!");
            return; 
        }
      
        Renderer = GetComponent<MeshRenderer>();
        if (Renderer == null)
        {
            Debug.LogError("MeshRenderer saknas!");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (NEB == null) return;

        if (NEB.isShoot == true || NEB.isCharging == true)
        {
            Renderer.material = GlowMaterialRed;
            //lightComponent.color = new Color(1.0f, 0.2313725f, 0.3030183f);

        }
        else if (NEB.isRotate == false && NEB.isShoot == false && NEB.isCharging == false && !NEB.IsStunned)
        {
            Renderer.material = GlowMaterialGreen;
            //lightComponent.color = new Color(1.0f, 0.6708761f, 0.2327043f);
        }
        else if (NEB.isRotate == true)
        {
            Renderer.material = GlowMaterialYellow;
            //lightComponent.color = new Color(1.0f, 0.6708761f, 0.2327043f);
        }
        else if (NEB.IsStunned == true)
        {
            Renderer.material = GlowMaterialBlue;
            //Debug.Log("Attemped at changing color to blue");

        }
    }
}
