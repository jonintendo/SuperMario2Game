using UnityEngine;
using System.Collections;

public class Midle : MonoBehaviour
{


    private float timer;

    public float openingTime = 1.0f;
    GameObject menu;
    Manager gerente;
    MenuGUI menugui;
    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        if (timer > openingTime)
        {
            //gerente.SetGameState(GameState.playing);
            //Application.LoadLevel("Level1");

            if (Application.loadedLevelName == "Midle")
            Application.LoadLevel("Level1");

            if (Application.loadedLevelName == "Midle2")
                Application.LoadLevel("Level2");

        }

    }


    void Awake()
    {
        openingTime = 1.0f;

        menu = GameObject.FindGameObjectWithTag("Menu");
        // if (menu != null)
        {
            gerente = menu.GetComponent<Manager>();

            gerente.NewPlayer(this.gameObject);

            //gerente.OnStateChange += HandleOnStateChange;

            menugui = menu.GetComponent<MenuGUI>();

        }
    }
}
