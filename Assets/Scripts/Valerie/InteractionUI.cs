using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private CameraBehaviour cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SwitchToScene(string sceneName)
    {
        //int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        Scene desiredScene = SceneManager.GetSceneByName(sceneName);
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < totalScenes; i++)
        {
            //Debug.Log(SceneManager.GetSceneByBuildIndex(i).name);
            if (SceneManager.GetSceneByBuildIndex(i) == desiredScene)
            {
                cam.SaveRotation();
                SceneManager.LoadScene(sceneName);
            }
        }
        /*
        int sceneIndex = new int();
        if (desiredScene.IsValid())
        {
            sceneIndex = desiredScene.buildIndex;
            if (sceneIndex < totalScenes)
            {
                SceneManager.LoadScene(sceneIndex);

            }
        }
        else
        {
            Debug.Log("'" + sceneName + "' was an invalid scene. Check if naming is correct.");
        }*/
        
    }
}
