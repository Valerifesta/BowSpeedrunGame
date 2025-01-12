using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainStructur : MonoBehaviour
{
    [SerializeField]
    public GameObject[] carts;
    public enum CartsLevel { Learning, Easy, Medium, Hard,
        Power, CranAttack, TooManyGuest, Imposible, Randomm};

    private CartsLevel currentStateLevel = CartsLevel.Learning;

    int informationLevel()
    {


        switch (currentStateLevel)
        {
            case CartsLevel.Learning:
                
                
                break;
            case CartsLevel.Easy:


                break;
            case CartsLevel.Medium:


                break;
            case CartsLevel.Hard:


                break;
            case CartsLevel.Power:


                break;
            case CartsLevel.CranAttack:


                break;
            case CartsLevel.TooManyGuest:


                break;
            case CartsLevel.Imposible:


                break;
            case CartsLevel.Randomm:


                break;
        }
        return 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }



    // Update is called once per frame
    void Update()
    {
        int cart = Random.Range(0, carts.Length);
    }
}
