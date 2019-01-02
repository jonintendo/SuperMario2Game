using UnityEngine;
using System.Collections;

public class MenuGUI : MonoBehaviour
{

    // public GameObject Player1;

    // UnityEngine.Object player;

    //public PlayerAction playeraction;


    public GUISkin menuSkin;
    public float width;
    public float height;

    private bool showMenu;

    private delegate void GUIMethod();
    private GUIMethod currentMenu;





    Manager gerente;




    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        GameObject[] others = GameObject.FindGameObjectsWithTag(transform.gameObject.tag);
        if (others.Length > 1)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {

            gerente = GetComponent<Manager>();
            gerente.OnStateChange += HandleOnStateChange;


            gerente.SetGameState(GameState.opening);


        }
    }


    void OnGUI()
    {


        if (!showMenu)
            return;
        currentMenu();
    }


    void OpeningGame()
    {
        GUI.skin = menuSkin;

        float screenX = Screen.width * 0.5f - width * 0.5f;
        float screenY = Screen.height * 0.5f + height * 0.5f;
        GUILayout.BeginArea(new Rect(screenX, screenY, width, height));


        if (GUILayout.Button("1P Game"))
        {

            //showMenu = false;
            //currentMenu = MainMenu;
            //stage = 1;

            Application.LoadLevel("Midle");

            //gerente.SetGameState(GameState.playing);

            gerente.NewPlayer();

        }

        //if (GUILayout.Button("Multiplayer Game"))
        //{
        //    GameObject menuObj = GameObject.Find("NetworkChat");
        //    MPBase2 menu = menuObj.GetComponent<MPBase2>();
        //    menu.Toggle();

        //    currentMenu = MultiplayerMenu;

        //}

        GUILayout.EndArea();
    }


    void GameOverGame()
    {
        GUI.skin = menuSkin;

        float screenX = Screen.width * 0.5f - width * 0.5f;
        float screenY = Screen.height * 0.5f + height * 0.5f;
        GUILayout.BeginArea(new Rect(screenX, screenY, width, height));


        if (GUILayout.Button("Continue"))
        {
            //gerente.SetGameState(GameState.playing);
            // gerente.RemovePlayer();

            //if (TakePlayer1())
            //{
            //    Destroy(Player1);
            //}
            showMenu = false;
            currentMenu = MainMenu;

            gerente.CurrentStage();

        }


        if (GUILayout.Button("Quit"))
        {
            //gerente.SetGameState(GameState.playing);

            //gerente.RemovePlayer();
            //if (TakePlayer1())
            //{
            //    Destroy(Player1);
            //}
            showMenu = true;
            currentMenu = OpeningGame;
            Application.LoadLevel("Opening");

        }

        //if (GUILayout.Button("Multiplayer Game"))
        //{
        //    GameObject menuObj = GameObject.Find("NetworkChat");
        //    MPBase2 menu = menuObj.GetComponent<MPBase2>();
        //    menu.Toggle();

        //    currentMenu = MultiplayerMenu;

        //}

        GUILayout.EndArea();
    }


    void MainMenu()
    {

        GUI.skin = menuSkin;

        float screenX = Screen.width * 0.5f - width * 0.5f;
        float screenY = Screen.height * 0.5f - height * 0.5f;
        GUILayout.BeginArea(new Rect(screenX, screenY, width, height));


        if (GUILayout.Button("Restart"))
        {
            gerente.SetGameState(GameState.restarting);
            gerente.CurrentStage();
        }


        if (GUILayout.Button("Options"))
        {
            Debug.Log("Options button!");
            currentMenu = OptionsMenu;
        }

        if (GUILayout.Button("Quit"))
        {

            Application.LoadLevel("Opening");

        }
        GUILayout.EndArea();


    }

    void OptionsMenu()
    {
        GUI.skin = menuSkin;

        float screenX = Screen.width * 0.5f - width * 0.5f;
        float screenY = Screen.height * 0.5f - height * 0.5f;

        GUILayout.BeginArea(new Rect(screenX, screenY, width, height));

        GUILayout.Label("Settings");
        if (GUILayout.Button("Sound"))
        {
            Debug.Log("Sound !");
            currentMenu = Sound;
        }
        if (GUILayout.Button("Buttons"))
        {
            Debug.Log("Buttons!");
            currentMenu = Buttons;
        }
        if (GUILayout.Button("Camera"))
        {
            Debug.Log("Camera");
            currentMenu = Cameras;
        }

        if (GUILayout.Button("Return to main menu"))
            currentMenu = MainMenu;

        GUILayout.EndArea();
    }

    void Sound()
    {
        GUI.skin = menuSkin;

        float screenX = Screen.width * 0.5f - width * 0.5f;
        float screenY = Screen.height * 0.5f - height * 0.5f;

        GUILayout.BeginArea(new Rect(screenX, screenY, width, height));

        GUILayout.Label("Sound");


        if (GUILayout.Button("Normal"))
        {
            currentMenu = OptionsMenu;
            // audioChosen = 1;
            // this.GetComponent<Audios>().ChooseAudio(audioChosen);
        }

        if (GUILayout.Button("Guitarra"))
        {
            currentMenu = OptionsMenu;
            //  audioChosen = 2;
            // this.GetComponent<Audios>().ChooseAudio(audioChosen);
        }

        if (GUILayout.Button("Bateria"))
        {
            currentMenu = OptionsMenu;
            // audioChosen = 3;
            // this.GetComponent<Audios>().ChooseAudio(audioChosen);
        }

        if (GUILayout.Button("Return"))
            currentMenu = OptionsMenu;

        GUILayout.EndArea();
    }

    void Buttons()
    {
        GUI.skin = menuSkin;

        float screenX = Screen.width * 0.5f - width * 0.5f;
        float screenY = Screen.height * 0.5f - height * 0.5f;

        GUILayout.BeginArea(new Rect(screenX, screenY, width, height));

        GUILayout.Label("Button");


        if (GUILayout.Button("Return"))
            currentMenu = OptionsMenu;

        GUILayout.EndArea();
    }

    void Cameras()
    {
        GUI.skin = menuSkin;

        float screenX = Screen.width * 0.5f - width * 0.5f;
        float screenY = Screen.height * 0.5f - height * 0.5f;

        GUILayout.BeginArea(new Rect(screenX, screenY, width, height));

        GUILayout.Label("Cameras");


        if (GUILayout.Button("Return"))
            currentMenu = OptionsMenu;

        GUILayout.EndArea();
    }


    void HandleOnStateChange()
    {
        switch (gerente.gameState)
        {
            case GameState.gameover:
                showMenu = true;
                currentMenu = GameOverGame;
                break;

            case GameState.clear:
                if (Application.loadedLevelName != "ClearStage")
                {

                    showMenu = false;
                    currentMenu = OpeningGame;

                }

                break;

            case GameState.dieing:
                showMenu = false;
                break;

            case GameState.opening:
                showMenu = true;
                currentMenu = OpeningGame;
                break;

            case GameState.playing:
                showMenu = false;
                currentMenu = MainMenu;
                break;

            case GameState.pause:
                showMenu = true;
                currentMenu = MainMenu;
                break;
        }


    }


    public bool ShowMenu
    {
        get { return showMenu; }
    }

    public void Toggle()
    {
        showMenu = !showMenu;
    }

    public void Toggle(GameObject Player)
    {
        showMenu = !showMenu;
    }










}
