using UnityEngine;
using System.Collections;

public class Midle : MonoBehaviour
{


    private float timer;

    public float openingTime = 1.0f;
    GameObject menu;
    Manager gerente;
   // MenuGUI menugui;


    void Awake()
    {
        openingTime = 1.0f;

        menu = GameObject.FindGameObjectWithTag("Menu");
        // if (menu != null)
        {
            gerente = menu.GetComponent<Manager>();

            gerente.NewPlayer(this.gameObject);

            //gerente.OnStateChange += HandleOnStateChange;

           // menugui = menu.GetComponent<MenuGUI>();

        }
    }


    void Update()
    {

        timer += Time.deltaTime;
        if (timer > openingTime)
        {


            gerente.NextStage();

        }

    }


   
}
