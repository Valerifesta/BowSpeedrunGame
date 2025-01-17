using UnityEngine;

public class DectedAlarm : MonoBehaviour
{
    [SerializeField] Material GlowMaterialRed;
    [SerializeField] Material GlowMaterialGreen;
    MeshRenderer renderer;
    //private Light lightComponent;


    public MeshRenderer Renderer { get => renderer; set => renderer = value; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Renderer = GetComponent<MeshRenderer>();
       
      //  lightComponent = GetComponentInChildren<Light>();
        // Kontrollera om Light-komponenten finns
       /* if (lightComponent != null)
        {
            // Ändra ljusets färg till exempelvis röd
           // lightComponent.color = new Color(1.0f, 0.6708761f, 0.2327043f);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Renderer.material = GlowMaterialRed;
            //lightComponent.color = new Color(1.0f, 0.2313725f, 0.3030183f);

        }
        else if(Input.GetKeyDown(KeyCode.O))
        {
            Renderer.material = GlowMaterialGreen;
            //lightComponent.color = new Color(1.0f, 0.6708761f, 0.2327043f);
        }
    }
}
