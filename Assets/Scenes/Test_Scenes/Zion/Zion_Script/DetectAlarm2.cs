using UnityEngine;

public class DectedAlarm2 : MonoBehaviour// its shame to do a same script twice although I know that I can just do an inheritance
{
    [SerializeField] private Material GlowMaterialRed;
    [SerializeField] private Material GlowMaterialGreen;
    [SerializeField] private Material GlowMaterialYellow;
    [SerializeField] private Material GlowMaterialBlue;
    MeshRenderer renderer;

    private TurrentLevel2 turrentLevel2;

   


    public MeshRenderer Renderer { get => renderer; set => renderer = value; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        turrentLevel2 = GetComponentInParent<TurrentLevel2>();
        // turrentLevel2 = transform.parent.transform.parent.GetComponent<TurrentLevel2>();
        if (turrentLevel2 == null)
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
        if (turrentLevel2 == null) return;

        if (turrentLevel2.isShoot == true || turrentLevel2.isCharging == true)
        {
            Renderer.material = GlowMaterialRed;
            //lightComponent.color = new Color(1.0f, 0.2313725f, 0.3030183f);

        }
        else if (turrentLevel2.isRotate == false && turrentLevel2.isShoot == false && turrentLevel2.isCharging == false && !turrentLevel2.IsStunned)
        {
            Renderer.material = GlowMaterialGreen;
            //lightComponent.color = new Color(1.0f, 0.6708761f, 0.2327043f);
        }
        else if (turrentLevel2.isRotate == true)
        {
            Renderer.material = GlowMaterialYellow;
            //lightComponent.color = new Color(1.0f, 0.6708761f, 0.2327043f);
        }
        else if (turrentLevel2.IsStunned == true)
        {
            Renderer.material = GlowMaterialBlue;
            //Debug.Log("Attemped at changing color to blue");

        }
    }
}
