using UnityEngine;
using System.Collections;

public class MenuGUI : MonoBehaviour
{

    public GameObject Player1;

    UnityEngine.Object player;

    //public PlayerAction playeraction;


    public GUISkin menuSkin;
    public float width;
    public float height;

    private bool showMenu;

    private delegate void GUIMethod();
    private GUIMethod currentMenu;





    Manager gerente;

    public int stage;
    public int audioChosen;




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

            gerente = this.GetComponent<Manager>();
            gerente.OnStateChange += HandleOnStateChange;

            stage = 0;
            audioChosen = 0;

            gerente.SetGameState(GameState.opening);


        }
    }



    void Start()
    {
        OnLevelWasLoaded(4);
        //Player1 = GameObject.FindGameObjectWithTag("player1");
        // playeraction = Player1.GetComponent<PlayerAction>();

    }

    void Update()
    {

        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gerente.gameState == GameState.playing)
            {

                gerente.SetGameState(GameState.pause);

            }

            else if (gerente.gameState == GameState.pause)
            {

                gerente.SetGameState(GameState.playing);


            }else  if (gerente.gameState == GameState.online)
            {


              
                Toggle();


            }

        }

    }

    public void HandleOnStateChange()
    {


        if (gerente.gameState == GameState.gameover)
        {

            if (Application.loadedLevelName != "GameOver")
            {


                showMenu = true;
                currentMenu = GameOverGame;
                Destroy(player);
                //Application.LoadLevel("GameOver");
            }


        }

        if (gerente.gameState == GameState.clear)
        {
            if (Application.loadedLevelName != "ClearStage")
            {

                showMenu = false;

                currentMenu = OpeningGame;

            }


        }

        if (gerente.gameState == GameState.dieing)
        {

            showMenu = false;

        }

        if (gerente.gameState == GameState.restarting)
        {
            // Player1.GetComponent<PlayerAction>().restart();


        }

        if (gerente.gameState == GameState.novafase)
        {


            NextStage();
        }


        if (gerente.gameState == GameState.opening)
        {

            showMenu = true;
            currentMenu = OpeningGame;


        }

        if (gerente.gameState == GameState.playing)
        {
            showMenu = false;
            currentMenu = MainMenu;

        }


        if (gerente.gameState == GameState.pause)
        {

            showMenu = true;
            currentMenu = MainMenu;


        }

        if (gerente.gameState == GameState.online)
        {
            showMenu = false;
            currentMenu = MainMenu;

        }


        Debug.Log("Handling state change to: " + gerente.gameState);
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

            SelectStage("Midle");

            //gerente.SetGameState(GameState.playing);

            GameObject[] others = GameObject.FindGameObjectsWithTag("player1");
            if (others.Length == 0)
            {
                player = Instantiate(Player1);
            }


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
            gerente.SetGameState(GameState.playing);
            // gerente.RemovePlayer();

            //if (TakePlayer1())
            //{
            //    Destroy(Player1);
            //}
            showMenu = false;
            currentMenu = MainMenu;

            CurrentStage();

        }


        if (GUILayout.Button("Quit"))
        {
            gerente.SetGameState(GameState.playing);

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

    public void CurrentStage()
    {
        switch (stage)
        {
            case 1:
                //stage = 2;

                SelectStage("Midle");
                break;

            case 2:
                //stage = 2;
                Application.LoadLevel("Midle2");
                break;

            default:
                //stage = 0;

                SelectStage("GameOver");
                break;

        }
    }

    public void NextStage()
    {
        switch (stage)
        {
            case 1:
                //stage = 2;

                SelectStage("Midle2");
                break;

            case 2:
                stage = 2;
                Application.LoadLevel("LevelM");
                break;

            default:
                //stage = 0;

                SelectStage("GameOver");
                break;

        }
    }


    public void SelectStage(string fase)
    {

        Application.LoadLevel(fase);

        OnLevelWasLoaded(5);
    }




    // bool TakePlayer1()
    //{
    //    Player1 = GameObject.FindGameObjectWithTag("player1");


    //    if (Player1 != null)
    //    {
    //        playeraction = Player1.GetComponent<PlayerAction>();
    //        return true;
    //    }
    //    else
    //        return false;

    //}

    void MainMenu()
    {

        GUI.skin = menuSkin;

        float screenX = Screen.width * 0.5f - width * 0.5f;
        float screenY = Screen.height * 0.5f - height * 0.5f;
        GUILayout.BeginArea(new Rect(screenX, screenY, width, height));

        if (gerente.gameState != GameState.online)
        {
            if (GUILayout.Button("Restart"))
            {

                //if (TakePlayer1())
                //playeraction.restart();
                //gerente.RestartPlayer();

                //Debug.Log("Play button!");
                //showMenu = false;
                //gerente.SetGameState(GameState.restarting);

                CurrentStage();
            }
        }

        if (GUILayout.Button("Options"))
        {
            Debug.Log("Options button!");
            currentMenu = OptionsMenu;
        }

        if (GUILayout.Button("Quit"))
        {
            Destroy(player);
            SelectStage("Opening");
            //gerente.SetGameState(GameState.opening);

            //gerente.RemovePlayer();
            //if (TakePlayer1())
            //{
            //Destroy(Player1);
            //}



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
            audioChosen = 1;
            this.GetComponent<Audios>().ChooseAudio(audioChosen);
        }

        if (GUILayout.Button("Guitarra"))
        {
            currentMenu = OptionsMenu;
            audioChosen = 2;
            this.GetComponent<Audios>().ChooseAudio(audioChosen);
        }

        if (GUILayout.Button("Bateria"))
        {
            currentMenu = OptionsMenu;
            audioChosen = 3;
            this.GetComponent<Audios>().ChooseAudio(audioChosen);
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


    void OnLevelWasLoaded(int level)
    {


        if (Application.loadedLevelName == "Midle")
        {
            stage = 1;
            audioChosen = 1;
            gerente.SetGameState(GameState.playing);
        }
        else

            if (Application.loadedLevelName == "Midle2")
            {
                stage = 2;
                audioChosen = 1;
                gerente.SetGameState(GameState.playing);
            }
            else
                if (Application.loadedLevelName == "Level1")
                {
                    stage = 1;
                    audioChosen = 1;

                    if (gerente.gameState != GameState.online)
                        gerente.SetGameState(GameState.playing);
                    else
                        gerente.SetGameState(GameState.online);
                }
                else
                    if (Application.loadedLevelName == "Level2")
                    {
                        stage = 2;
                        audioChosen = 1;

                        if (gerente.gameState != GameState.online)
                            gerente.SetGameState(GameState.playing);
                        else
                            gerente.SetGameState(GameState.online);
                    }
                    else
                        if (Application.loadedLevelName == "Level3" || Application.loadedLevelName == "LevelM")
                        {
                            stage = 3;
                            audioChosen = 1;

                            Debug.Log(gerente.gameState);
                            if (gerente.gameState != GameState.online)
                                gerente.SetGameState(GameState.playing);
                            else
                                gerente.SetGameState(GameState.online);
                        }
                        else


                            if (Application.loadedLevelName == "Opening")
                            {
                                stage = 0;
                                gerente.SetGameState(GameState.opening);


                            }
                            else

                                if (Application.loadedLevelName == "GameOver")
                                {
                                    gerente.SetGameState(GameState.gameover);


                                }




    }







}
