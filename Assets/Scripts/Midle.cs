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


        gerente = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();


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
