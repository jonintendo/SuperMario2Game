using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject tartaruga;
    public GameObject monstro;

   // MenuRede menu;

    float timer;
    float timeEnemyAppear;
  
    void Start()
    {
        timer = 0.0f;
        timeEnemyAppear = 10.0f;
        //menu =  GetComponentInParent<MenuRede>();
    }

    
    void Update()
    {
        timer += Time.deltaTime;
        if (Network.isServer )
        {
            if (timer > timeEnemyAppear)
            {
                Network.Instantiate(tartaruga, tartaruga.transform.position, tartaruga.transform.rotation, 0);
                Network.Instantiate(monstro, monstro.transform.position, monstro.transform.rotation, 0);
                timer = 0;
            }
        }
    }



}
