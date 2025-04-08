using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField]
    private Material GlowMaterialWhite;
    private Shader shader;
    MeshRenderer renderer;

    public MeshRenderer Renderer { get => renderer; set => renderer = value; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Renderer = GetComponent<MeshRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Renderer.material = GlowMaterialWhite;

        }
        else
        {
          
        }
       
    }
}
