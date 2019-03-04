using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GUIOnLine : MonoBehaviour
{





    private Rect window;
    private int width = 500;
    private int height = 180;
    private bool showMenu;


    public GUISkin menuSkin;
    private delegate void GUIMethod();
    private GUIMethod currentMenu;

    public GUIText Timert;


    private float lastUnfocusTime = 0;


    private String inputField = "";

    private Vector2 scrollPosition;


    public Manager gerente;

    private ArrayList chatEntries = new ArrayList();
    class ChatEntry
    {
        public String name = "";
        public String text = "";
    }


    private void Awake()
    {
        window = new Rect(150, 150, width, height);


        Timert = GetComponentInChildren<GUIText>();

        gerente = gameObject.GetComponentInParent<Manager>();

        gerente.OnStateChange += HandleOnStateChange;
        gerente.OnModeOnLineChange += HandleOnModeOnLineChange;
    }

    void SendCommand(String msg)
    {

        msg = msg.Replace("\n", "");

        GetComponent<NetworkView>().RPC("ComandodoJogo", RPCMode.Server, msg, gerente.connectplayerName);
        inputField = ""; //Clear line
        //GUI.UnfocusWindow ();//Deselect chat
        lastUnfocusTime = Time.time;

    }


    public void HitEnter(String msg, string playerdest)
    {

        msg = msg.Replace("\n", "");
        GetComponent<NetworkView>().RPC("ApplyChat", RPCMode.All, gerente.connectplayerName, msg, playerdest);

        inputField = ""; //Clear line

        lastUnfocusTime = Time.time;

    }


    public void HandleOnModeOnLineChange()
    {
       
        //showMenu = true;
        //currentMenu = OpeningNetwork;

    }



    public void HandleOnStateChange()
    {


        switch (gerente.gameState)
        {

            case GameState.clear:
                break;

            case GameState.playing:


                break;

            case GameState.gameover:


                break;

            case GameState.restarting:


                break;

            case GameState.opening:


                showMenu = true;
                currentMenu = OpeningGame;



                break;


            case GameState.pause:


                break;

            case GameState.dieing:
                HitEnter(gerente.connectplayerName + " Morreu ", "");


                break;

            case GameState.novafase:

                break;
        }
        Debug.Log("Handling state Rede change to: " + gerente.gameState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {

            Toggle();

        }

    }

    public void UpdateTimeOnScreen(string now)
    {

        Timert.text = now;
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

        if (Event.current.type == EventType.KeyDown && char.Equals(Event.current.character, '\n') && inputField.Length <= 0)
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

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return && inputField.Length > 0)
        {

            string destinatario = inputField.Split(':')[0];
            if (inputField.Contains(":"))
            {

                HitEnter(inputField, destinatario);
            }
            else
            {
                if (inputField.EndsWith(";") && gerente.gameState == GameState.playing)
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
            gerente.roupa = PlayerRoupa.luigi;
            currentMenu = OpeningNetwork;

        }
        if (GUILayout.Button("Fogo"))
        {
            gerente.roupa = PlayerRoupa.fogo;
            currentMenu = OpeningNetwork;

        }
        if (GUILayout.Button("Mimico"))
        {
            gerente.roupa = PlayerRoupa.mimico;
            currentMenu = OpeningNetwork;

        }
        if (GUILayout.Button("Mario"))
        {
            gerente.roupa = PlayerRoupa.mario;
            currentMenu = OpeningNetwork;

        }
        if (GUILayout.Button("Wario"))
        {
            gerente.roupa = PlayerRoupa.wario;
            currentMenu = OpeningNetwork;

        }


        GUILayout.EndArea();


    }


    void OnServerInitialized()
    {
       

    }

    void OpeningNetwork()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            //We are currently disconnected: Not a client or host
            GUILayout.Label("Connection status: Disconnected");

            gerente.connectToIP = GUILayout.TextField(gerente.connectToIP, GUILayout.MinWidth(100));
            gerente.connectPort = int.Parse(GUILayout.TextField(gerente.connectPort.ToString()));
            gerente.connectplayerName = GUILayout.TextField(gerente.connectplayerName, GUILayout.MinWidth(100));



            GUILayout.BeginVertical();
            if (GUILayout.Button("Connect as client"))
            {

                Network.Connect(gerente.connectToIP, gerente.connectPort);


            }

            if (GUILayout.Button("Start Server"))
            {


                //showMenu = true;
                // currentMenu = ModoGame;

                Network.useNat = false;
                Network.InitializeServer(32, gerente.connectPort);




            }

            if (GUILayout.Button("Title Screen"))
            {

                Application.LoadLevel("Opening");

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



            if (gerente.gameState != GameState.playing)
            {
                if (GUILayout.Button("Iniciar jogo"))
                {

                    GetComponent<NetworkView>().RPC("Entrarnojogo", RPCMode.All, gerente.connectplayerName);


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
                    GetComponent<NetworkView>().RPC("Sairdojogo", RPCMode.All, gerente.connectplayerName);

                }
            }

            if (GUILayout.Button("Disconnect"))
            {
                Network.Disconnect(200);
            }

            if (gerente.gameModeOnLine == GameModeOnLine.timeatack && Network.isServer)
            {

                string setUpTimerS = GUILayout.TextField(gerente.setUpTimer.ToString(), GUILayout.MinWidth(100));

                try
                {
                    gerente.setUpTimer = float.Parse(setUpTimerS);
                }
                catch
                {
                    gerente.setUpTimer = 10.0f;
                }

                if (GUILayout.Button("Iniciar Jogo pra Galera"))
                {
                    gerente.SetGameModeOnLine(GameModeOnLine.timeatack);

                }
            }
        }

    }


    [RPC]
    void ApplyChat(String name, String msg, string playerdestino)
    {

        var entry = new ChatEntry();
        entry.name = name;
        entry.text = msg;
        if (gerente.connectplayerName == playerdestino || playerdestino == "")
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

    void OpeningGame()
    {
        GUI.skin = menuSkin;

        float screenX = Screen.width * 0.5f - width * 0.5f;
        float screenY = Screen.height * 0.5f + height * 0.5f;
        GUILayout.BeginArea(new Rect(screenX, screenY, width, height));


        if (GUILayout.Button("Multiplayer"))
        {
            
            gerente.SetGameStateNetwork(GameStateNetwork.online);
            showMenu = true;
            currentMenu = OpeningNetwork;

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

            gerente.SetGameModeOnLine(GameModeOnLine.timeatack);


        }

        if (GUILayout.Button("Take Flag"))
        {

            showMenu = true;
            currentMenu = FaseGame;

            gerente.SetGameModeOnLine(GameModeOnLine.takeflag);



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

          
            Application.LoadLevel("Level1");


            Network.InitializeServer(32, gerente.connectPort, false);

        }

        if (GUILayout.Button("Fase 2"))
        {

            showMenu = true;
            currentMenu = OpeningNetwork;

           
            Application.LoadLevel("Level2");


            Network.InitializeServer(32, gerente.connectPort, false);

        }

        if (GUILayout.Button("Fase M"))
        {

            showMenu = true;
            currentMenu = OpeningNetwork;


            Application.LoadLevel("LevelM");


            Network.InitializeServer(32, gerente.connectPort, false);

        }

        GUILayout.EndArea();
    }


}
