using UnityEngine;
using System.Collections;
using System;



public enum GameModeOnLine { timeatack, takeflag }
public class MenuRede : MonoBehaviour
{
    public GameModeOnLine gameMode { get; set; }

    public int Roupa = 0;
    public int audioChosen = 0;

    String connectToIP = "127.0.0.1";
    int connectPort = 25001;
    public String connectplayerName = "jonathan";
    public float setUpTimer = 10.0f;


    private Rect window;
    private int width = 500;
    private int height = 180;
    private bool showMenu;


    public GUISkin menuSkin;
    private delegate void GUIMethod();
    private GUIMethod currentMenu;

    private float lastUnfocusTime = 0;


    private String inputField = "";
    public String playername = "jj";
    private Vector2 scrollPosition;

    // MenuGui menuOnline;




    public GameObject menu;
    public Manager gerente;
    public MenuGUI menugui;


    public ManagerOnLine gerenteRede;

    public bool jogando;

    public GameObject PlayerRede;
    public GameObject PlayerRedeInstantiated;
    // PlayerActionOnLine playeractiononline;


    public GameObject mario;
    public GameObject luigi;
    public GameObject wario;
    public GameObject mimico;
    public GameObject fogo;



    public GUIText Timert;

    float timer;

    private ArrayList chatEntries = new ArrayList();
    class ChatEntry
    {
        public String name = "";
        public String text = "";
    }


    public bool contando = false;


    void AdjustPlayer()
    {


        playername = PlayerPrefs.GetString("playerName", connectplayerName);

        if (playername == null || playername == "")
        {
            playername = "RandomName" + UnityEngine.Random.Range(1, 999);
        }


    }


    void Start()
    {


    }

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


            timer = 0;
            contando = false;

            window = new Rect(150, 150, width, height);
            currentMenu = OpeningGame;
            showMenu = true;

            jogando = false;


            menu = GameObject.FindGameObjectWithTag("Menu");

            gerente = menu.GetComponent<Manager>();

            gerente.OnStateChange += HandleOnStateChange;

            gerenteRede = this.gameObject.GetComponent<ManagerOnLine>();

            gerenteRede.OnStateChange += HandleOnStateRedeChange;


            menugui = menu.GetComponent<MenuGUI>();

            Timert = GetComponentInChildren<GUIText>();

            //if (!(Network.peerType == NetworkPeerType.Disconnected))
            //{
            //    Network.Disconnect(200);
            //}


        }

    }

    void Update()
    {
        if (contando)
        {
            timer += Time.deltaTime;
            UpdateTimeOnScreen(timer.ToString());
        }

        if (timer >= setUpTimer)
        {
            EndCount();
            //if (PlayerRedeInstantiated != null)
            //{
            //    var placar = PlayerRedeInstantiated.GetComponent<PlayerActionOnLine>().GetPlacar();
            //    PlayerRedeInstantiated.GetComponent<PlayerActionOnLine>().SetPlacar(placar[0], placar[1]);
            //}
        }

        if (Input.GetKeyDown(KeyCode.P))
        {

            Toggle();

        }

        if (PlayerRedeInstantiated != null)
        {
            audioChosen = menugui.audioChosen;
        }

    }




    public void HandleOnStateChange()
    {
        Debug.Log("Handling state change to: " + gerente.gameState);

        switch (gerente.gameState)
        {

            case GameState.clear:

                break;

            case GameState.playing:

                // showMenu = false;

                break;

            case GameState.gameover:


                break;

            case GameState.restarting:


                break;

            case GameState.opening:

                if (!(Network.peerType == NetworkPeerType.Disconnected))
                {
                    Network.Disconnect(200);
                }
                showMenu = true;
                currentMenu = OpeningGame;

                break;


            case GameState.pause:


                break;

            case GameState.dieing:

                break;

            case GameState.novafase:


                break;

            case GameState.online:

                showMenu = true;
                currentMenu = OpeningNetwork;

                break;
        }

        //Debug.Log("Handling state change to:" + gerente.gameState + "vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv ");
    }


    public void HandleOnStateRedeChange()
    {


        switch (gerenteRede.gameState)
        {

            case GameStateOnLine.clear:
                break;

            case GameStateOnLine.playing:


                break;

            case GameStateOnLine.gameover:


                break;

            case GameStateOnLine.restarting:


                break;

            case GameStateOnLine.opening:



                Start();

                break;


            case GameStateOnLine.pause:


                break;

            case GameStateOnLine.dieing:
                HitEnter(connectplayerName + " Morreu ", "");
                PlayerRedeInstantiated.GetComponent<PlayerActionOnLine>().restart();

                //if (Player1 != null)
                //    RemovePlayer();
                break;

            case GameStateOnLine.novafase:
                //if (Player1 != null)
                //    RemovePlayer();
                break;
        }
        Debug.Log("Handling state Rede change to: " + gerenteRede.gameState);
    }





    public void BeginCount()
    {
        contando = true;
        timer = 0;

    }


    public void EndCount()
    {
        contando = false;
        timer = 0;

    }


    public void ContinueCount(float timeServer)
    {

        timer = timeServer;
        contando = true;
    }

    public void UpdateTimeOnScreen(string now)
    {

        Timert.text = now;
    }

    public void Toggle()
    {
        showMenu = !showMenu;
    }


    void SetClothes()
    {


        GUI.skin = menuSkin;

        float screenX = Screen.width * 0.5f - width * 0.5f;
        float screenY = Screen.height * 0.5f + height * 0.5f;
        GUILayout.BeginArea(new Rect(screenX, screenY, width, height));


        if (GUILayout.Button("Luigi"))
        {
            Roupa = 0;
            currentMenu = OpeningNetwork;

        }
        if (GUILayout.Button("Fogo"))
        {
            Roupa = 1;
            currentMenu = OpeningNetwork;

        }
        if (GUILayout.Button("Mimico"))
        {
            Roupa = 2;
            currentMenu = OpeningNetwork;

        }
        if (GUILayout.Button("Mario"))
        {
            Roupa = 3;
            currentMenu = OpeningNetwork;

        }
        if (GUILayout.Button("Wario"))
        {
            Roupa = 4;
            currentMenu = OpeningNetwork;

        }


        GUILayout.EndArea();


    }


    void OpeningNetwork()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            //We are currently disconnected: Not a client or host
            GUILayout.Label("Connection status: Disconnected");

            connectToIP = GUILayout.TextField(connectToIP, GUILayout.MinWidth(100));
            connectPort = int.Parse(GUILayout.TextField(connectPort.ToString()));
            connectplayerName = GUILayout.TextField(connectplayerName, GUILayout.MinWidth(100));



            GUILayout.BeginVertical();
            if (GUILayout.Button("Connect as client"))
            {

                //Connect to the "connectToIP" and "connectPort" as entered via the GUI
                //Ignore the NAT for now
                //Network.useNat = false;
                AdjustPlayer();
                Network.Connect(connectToIP, connectPort);


            }

            if (GUILayout.Button("Start Server"))
            {
                //Start a server for 32 clients using the "connectPort" given via the GUI
                //Ignore the nat for now	
                //Network.useNat = false;


                // if (GUILayout.Button("Return"))
                {
                    showMenu = true;
                    currentMenu = ModoGame;


                }




            }

            if (GUILayout.Button("Title Screen"))
            {

                menugui.SelectStage("Opening");

            }
            GUILayout.EndVertical();


        }
        else
        {
            //We've got a connection(s)!

            //networkView.RPC ("ReceiveName", RPCMode.Server, connectplayerName);

            if (Network.peerType == NetworkPeerType.Connecting)
            {

                GUILayout.Label("Connection status: Connecting");

            }
            else if (Network.peerType == NetworkPeerType.Client)
            {

                GUILayout.Label("Connection status: Client!");
                GUILayout.Label("Ping to server: " + Network.GetAveragePing(Network.connections[0]));


            }
            else if (Network.peerType == NetworkPeerType.Server)
            {

                GUILayout.Label("Connection status: Server!");
                GUILayout.Label("Connections: " + Network.connections.Length);

                if (Network.connections.Length >= 1)
                {
                    GUILayout.Label("Ping to first player: " + Network.GetAveragePing(Network.connections[0]));
                }
            }



            if (!jogando)
            {
                if (GUILayout.Button("Iniciar jogo"))
                {

                    networkView.RPC("Entrarnojogo", RPCMode.All, playername);


                }
                if (GUILayout.Button("Escolher Roupa"))
                {

                    currentMenu = SetClothes;

                }


            }
            else
            {
                if (GUILayout.Button("Sair do jogo"))
                {
                    networkView.RPC("Sairdojogo", RPCMode.All, playername);

                }
            }

            if (GUILayout.Button("Disconnect"))
            {
                Network.Disconnect(200);
            }

            if (gameMode == GameModeOnLine.timeatack && Network.isServer)
            {

                string setUpTimerS = GUILayout.TextField(setUpTimer.ToString(), GUILayout.MinWidth(100));

                try
                {
                    setUpTimer = float.Parse(setUpTimerS);
                }
                catch
                {
                    setUpTimer = 10.0f;
                }

                if (GUILayout.Button("Iniciar Jogo pra Galera"))
                {



                    iniciarJogoTimeAtack();
                }
            }
        }

    }


    void OpeningGame()
    {
        GUI.skin = menuSkin;

        float screenX = Screen.width * 0.5f - width * 0.5f;
        float screenY = Screen.height * 0.5f + height * 0.5f;
        GUILayout.BeginArea(new Rect(screenX, screenY, width, height));


        if (GUILayout.Button("Multiplayer"))
        {



            gerente.SetGameState(GameState.online);

        }


        GUILayout.EndArea();
    }

    void ModoGame()
    {
        GUI.skin = menuSkin;

        float screenX = Screen.width * 0.5f - width * 0.5f;
        float screenY = Screen.height * 0.5f + height * 0.5f;
        GUILayout.BeginArea(new Rect(screenX, screenY, width, height));


        if (GUILayout.Button("Time Atack"))
        {

            showMenu = true;
            currentMenu = FaseGame;

            gameMode = GameModeOnLine.timeatack;


        }

        if (GUILayout.Button("Take Flag"))
        {

            showMenu = true;
            currentMenu = FaseGame;

            gameMode = GameModeOnLine.takeflag;



        }



        GUILayout.EndArea();
    }

    void FaseGame()
    {
        GUI.skin = menuSkin;

        float screenX = Screen.width * 0.5f - width * 0.5f;
        float screenY = Screen.height * 0.5f + height * 0.5f;
        GUILayout.BeginArea(new Rect(screenX, screenY, width, height));


        if (GUILayout.Button("Fase 1"))
        {

            showMenu = true;
            currentMenu = OpeningNetwork;

            gerente.SetGameState(GameState.online);
            menugui.SelectStage("Level1");

            AdjustPlayer();
            Network.InitializeServer(32, connectPort, false);

        }

        if (GUILayout.Button("Fase 2"))
        {

            showMenu = true;
            currentMenu = OpeningNetwork;

            gerente.SetGameState(GameState.online);
            menugui.SelectStage("Level2");

            AdjustPlayer();
            Network.InitializeServer(32, connectPort, false);

        }

        if (GUILayout.Button("Fase M"))
        {

            showMenu = true;
            currentMenu = OpeningNetwork;

            gerente.SetGameState(GameState.online);
            menugui.SelectStage("LevelM");

            AdjustPlayer();
            Network.InitializeServer(32, connectPort, false);

        }

        GUILayout.EndArea();
    }



    public void SetRoupa(int roupa)
    {
        if (roupa == 0)
            PlayerRede = luigi;

        if (roupa == 1)
            PlayerRede = fogo;

        if (roupa == 2)
            PlayerRede = mimico;

        if (roupa == 3)
            PlayerRede = mario;

        if (roupa == 4)
            PlayerRede = wario;


    }

    public void iniciarJogo(string tagPlayer)
    {



        jogando = true;
        // DestroyImmediate(gerente.Player1);
        //PlayerRede.name = playername;

        SetRoupa(Roupa);
        PlayerRede.tag = tagPlayer;
        PlayerRedeInstantiated = (GameObject)Network.Instantiate(PlayerRede, PlayerRede.transform.position, PlayerRede.transform.rotation, 0);

        PlayerRedeInstantiated.GetComponent<PlayerActionOnLine>().menuRede = this.GetComponent<MenuRede>();
        PlayerRedeInstantiated.tag = tagPlayer;
        PlayerRedeInstantiated.GetComponent<PlayerActionOnLine>().Tag = tagPlayer;






        //PlayerRedeInstantiated.GetComponent<NetworkView>().owner

        // networkView.RPC("ReceiveName", RPCMode.All, name,server.ilha.GetPlayer(NetworkView.player).Tag);


    }

    public void iniciarJogoTimeAtack()
    {

        networkView.RPC("IniciarTimeAtack", RPCMode.All, playername);


        this.gameObject.GetComponentInChildren<SERVER>().IniciarTimeAtackServer();

        //networkView.RPC("IniciarTimeAtackServer", RPCMode.All, playername);
    }



    public void SetJogoTimeAtack(NetworkMessageInfo info)
    {
        networkView.RPC("SetTimeAtack", RPCMode.All, timer);

    }


    public void terminarJogo()
    {



        jogando = false;



        Network.Destroy(PlayerRedeInstantiated);
        //}

        // Network.RemoveRPCs(Network.player);
        // Network.DestroyPlayerObjects(Network.player);

    }




    void OnGUI()
    {
        if (!showMenu)
            return;

        currentMenu();

        if (Network.peerType == NetworkPeerType.Server || Network.peerType == NetworkPeerType.Client)
            ChatWindow();
    }



    void ChatWindow()
    {

        if (Event.current.type == EventType.keyDown && char.Equals(Event.current.character, '\n') && inputField.Length <= 0)
        {
            if (lastUnfocusTime + 0.25 < Time.time)
            {
                // usingChat = true;
                GUI.FocusWindow(5);
                GUI.FocusControl("Chat input field");
            }
        }

        // GUI.skin = menuSkin;

        float screenX = Screen.width * 0.5f - width * 0.5f;
        float screenY = Screen.height * 0.5f + height * 0.5f;
        //  GUILayout.BeginArea(new Rect(screenX, screenY, width, height));
        GUILayout.BeginArea(window);


        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.EndVertical();

        // Begin a scroll view. All rects are calculated automatically - 
        // it will use up any available screen space and make sure contents flow correctly.
        // This is kept small with the last two parameters to force scrollbars to appear.
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (ChatEntry entry in chatEntries)
        {
            GUILayout.BeginHorizontal();
            if (entry.name == "")
            {//Game message
                GUILayout.Label(entry.text);
            }
            else
            {
                GUILayout.Label(entry.name + ": " + entry.text);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(3);

        }


        // End the scrollview we began above.
        GUILayout.EndScrollView();

        if (Event.current.type == EventType.keyDown && Event.current.keyCode == KeyCode.Return && inputField.Length > 0)
        {

            string destinatario = inputField.Split(':')[0];
            if (inputField.Contains(":"))
            {

                HitEnter(inputField, destinatario);
            }
            else
            {
                if (inputField.EndsWith(";") && jogando)
                    SendCommand(inputField);
                else
                    HitEnter(inputField, "");
            }
        }

        GUI.SetNextControlName("Chat input field");
        inputField = GUILayout.TextField(inputField);


        //if (Input.GetKeyDown("mouse 0"))
        //{
        //    if (usingChat)
        //    {
        //        usingChat = false;
        //        GUI.UnfocusWindow();//Deselect chat
        //        lastUnfocusTime = Time.time;
        //    }
        //}

        GUILayout.EndArea();
    }


    public void SendStatus(int coin, int life, bool flags)
    {


        networkView.RPC("ReceiveStatus", RPCMode.All, coin, life, flags,playername);//so mandando pra todos pq se nao o server nao recebe quando eh ele que manda, era so pra ir proserver


    }


    void SendCommand(String msg)
    {

        msg = msg.Replace("\n", "");

        networkView.RPC("ComandodoJogo", RPCMode.Server, msg, playername);
        inputField = ""; //Clear line
        //GUI.UnfocusWindow ();//Deselect chat
        lastUnfocusTime = Time.time;

    }


    public void HitEnter(String msg, string playerdest)
    {

        msg = msg.Replace("\n", "");
        networkView.RPC("ApplyChat", RPCMode.All, playername, msg, playerdest);

        inputField = ""; //Clear line
        //GUI.UnfocusWindow ();//Deselect chat
        lastUnfocusTime = Time.time;
        //showMenu = false;
    }


    public void ReiniciarJogoOnline()
    {
        networkView.RPC("ReiniciarJogo", RPCMode.All);
    }


    [RPC]
    void SetStageMode(string stage, string playerdestino, string gamemode)
    {

        if (playername == playerdestino)
        {
            if (Network.isClient)
            {
                 menugui.SelectStage(stage);
                // gerente.SetGameState(GameState.playing);

                if (stage == GameModeOnLine.takeflag.ToString())
                    gameMode = GameModeOnLine.takeflag;

                if (stage == GameModeOnLine.timeatack.ToString())
                    gameMode = GameModeOnLine.timeatack;

            }



        }




    }

    public void SetStageModeServer(string stage, string playerdestino, string gamemode)
    {
        networkView.RPC("SetStageMode", RPCMode.All, stage, playerdestino, gamemode);
    }



    [RPC]
    void SetTag(string tagPlayer, string playerdestino)
    {

        if (playername == playerdestino)
        {

            iniciarJogo(tagPlayer);


            //PlayerRedeInstantiated.GetComponent<PlayerActionOnLine>().OnlineTagsPlacar();
        }



    }

    public void SetTagServer(string tagPlayer, string playerdestino)
    {
        networkView.RPC("SetTag", RPCMode.All, tagPlayer, playerdestino);
    }







    [RPC]
    void LeaveGame(string playerdestino)
    {
        if (playername == playerdestino)
            terminarJogo();

    }

    public void LeaveGameServer(string playerdestino)
    {

        networkView.RPC("LeaveGame", RPCMode.All, playerdestino);
    }


    [RPC]
    void ApplyChat(String name, String msg, string playerdestino)
    {

        var entry = new ChatEntry();
        entry.name = name;
        entry.text = msg;
        if (playername == playerdestino || playerdestino == "")
        {
            chatEntries.Add(entry);
            Timert.text = msg;
        }

        //Remove old entries
        if (chatEntries.Count > 30)
        {
            chatEntries.RemoveAt(0);
        }

        scrollPosition.y = 1000000;
    }


    [RPC]
    void IniciarTimeAtack(string playerdestino)
    {

        BeginCount();
    }

    [RPC]
    void SetTimeAtack(float timeServer)
    {

        ContinueCount(timeServer);
    }



    [RPC]
    void ReiniciarJogo()
    {
        if (PlayerRedeInstantiated!=null)
        PlayerRedeInstantiated.GetComponent<PlayerActionOnLine>().restart();
    }


    void OnDisconnectedFromServer()
    {
        Debug.Log("cliente desconectou");

        //  menu.terminarJogo();
        UnityEngine.Object playerToDestroy;

        playerToDestroy = GameObject.FindGameObjectWithTag("player1");
        if (playerToDestroy != null) Destroy(playerToDestroy);

        playerToDestroy = GameObject.FindGameObjectWithTag("player2");
        if (playerToDestroy != null) Destroy(playerToDestroy);

        playerToDestroy = GameObject.FindGameObjectWithTag("player3");
        if (playerToDestroy != null) Destroy(playerToDestroy);

        playerToDestroy = GameObject.FindGameObjectWithTag("player4");
        if (playerToDestroy != null) Destroy(playerToDestroy);


        foreach (var enemyToDestroy in GameObject.FindGameObjectsWithTag("enemy"))
        {
            Destroy(enemyToDestroy);
        }

        foreach (var fireballToDestroy in GameObject.FindGameObjectsWithTag("Fireball"))
        {
            Destroy(fireballToDestroy);
        }
        Network.RemoveRPCs(networkView.owner);
        jogando = false;


        menugui.GetComponent<AudioListener>().enabled = true;
        menugui.camera.enabled = true;

    }

}
