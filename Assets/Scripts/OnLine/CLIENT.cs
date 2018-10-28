using UnityEngine;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class CLIENT : MonoBehaviour
{




    MenuRede menuRede;





    void Awake()
    {
        menuRede = GetComponent<MenuRede>();


    }

    //Client function


    void OnConnectedToServer()
    {

      

        networkView.RPC("TellServerOurName", RPCMode.Server, menuRede.playername);



       
    }



   


   
   
    

}
