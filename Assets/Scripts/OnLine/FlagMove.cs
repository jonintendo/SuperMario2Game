using UnityEngine;
using System.Collections;

public class FlagMove : MonoBehaviour {
   
    
    public GameObject flag;

    public GameObject flaginstantiated;

    public GameObject bol1;
    public GameObject bol2;
    public GameObject bol3;
    public GameObject bol4;


   

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
        if (Network.isServer)
        {
            if (timer > timeEnemyAppear)
            {
               
                if (flaginstantiated != null)
                    Network.Destroy(flaginstantiated);


                int bolchoosen = UnityEngine.Random.Range(1,4);
                
                if (bolchoosen==1)

                    flaginstantiated = (GameObject)Network.Instantiate(flag, bol1.transform.position, bol1.transform.rotation, 0);

                if (bolchoosen == 2)

                    flaginstantiated = (GameObject)Network.Instantiate(flag, bol2.transform.position, bol2.transform.rotation, 0);

                if (bolchoosen == 3)

                    flaginstantiated = (GameObject)Network.Instantiate(flag, bol3.transform.position, bol3.transform.rotation, 0);

                if (bolchoosen == 4)

                    flaginstantiated = (GameObject)Network.Instantiate(flag, bol4.transform.position, bol4.transform.rotation, 0);


            
                timer = 0;
            }
        }
    }


}
