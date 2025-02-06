using UnityEngine;

public class DectedAlarm : MonoBehaviour
{
    [SerializeField] private Material GlowMaterialRed;
    [SerializeField] private Material GlowMaterialGreen;
    [SerializeField] private Material GlowMaterialYellow;
    [SerializeField] private Material GlowMaterialBlue;
    MeshRenderer renderer;

    private NewEnemyBehaviour enemyBehavior;
   
    //private Light lightComponent;


    public MeshRenderer Renderer { get => renderer; set => renderer = value; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyBehavior = transform.parent.transform.parent.GetComponent<NewEnemyBehaviour>();
       
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
        if (enemyBehavior.isShoot==true || enemyBehavior.isCharging == true)
        {
            Renderer.material = GlowMaterialRed;
            //lightComponent.color = new Color(1.0f, 0.2313725f, 0.3030183f);

        }
        else if(enemyBehavior.isRotate == false && enemyBehavior.isShoot == false && enemyBehavior.isCharging == false && !enemyBehavior.IsStunned)
        {
            Renderer.material = GlowMaterialGreen;
            //lightComponent.color = new Color(1.0f, 0.6708761f, 0.2327043f);
        }
        else if (enemyBehavior.isRotate == true)
        {
            Renderer.material = GlowMaterialYellow;
            //lightComponent.color = new Color(1.0f, 0.6708761f, 0.2327043f);
        }
        else if (enemyBehavior.IsStunned == true)
        {
            Renderer.material = GlowMaterialBlue;
            //Debug.Log("Attemped at changing color to blue");
          
        }
    }
}
