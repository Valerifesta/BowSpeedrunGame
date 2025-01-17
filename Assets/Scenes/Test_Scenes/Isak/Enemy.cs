using UnityEngine;

public class Enemy : MonoBehaviour
{
    bool hit_player;
    [SerializeField]
    GameObject player;

    float looktime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
      RaycastHit info;
      Physics.Raycast(transform.position, transform.position - player.transform.position, out info, 100);
      if(info.collider.gameObject == player){
        looktime += Time.deltaTime;
        if(looktime > 6000){
          Debug.Log("Hit!!!");
          looktime = 0;
        }

      }
      else{
        looktime = 0;
      }
    }
}
