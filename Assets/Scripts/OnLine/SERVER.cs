using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SERVER : MonoBehaviour
{

    public Ilha2 ilha;    
    public ArrayList playerList = new ArrayList();

   
    GUIOnLine guiOnLine;
    Manager gerente;

    float timer;

    class PlayerChat
    {
        public String name = "";
        public NetworkPlayer networkPlayer;
    }

    void OnServerInitialized()
    {

        //AddNewPlayer(gerente.connectplayerName, Network.player);

        guiOnLine.HitEnter("server ativo", "");



        ilha = new Ilha2(gerente.gameModeOnLine);

    }


    void OnPlayerDisconnected(NetworkPlayer player)
    {
        guiOnLine.HitEnter("Player disconnected from: " + player.ipAddress + ":" + player.port, "");

        string hhh = ilha.RemovePlayerN(player);

        if (hhh != "")
        {
            guiOnLine.HitEnter(hhh + " left the Game", "");
        }


        string hh = RemovePlayer(player);

        if (hh != "")
        {
            guiOnLine.HitEnter(hh + " left the chat", "");
        }

        Network.DestroyPlayerObjects(player);
        Network.RemoveRPCs(player);



    }




    void OnPlayerConnected(NetworkPlayer player)
    {

        guiOnLine.HitEnter("Player connected from: " + player.ipAddress + ":" + player.port, "");


    }


    void OnDisconnectedFromServer()
    {

        playerList = new ArrayList();

        Debug.Log("server desconectou");
    }


    void Awake()
    {
        guiOnLine = GetComponent<GUIOnLine>();
        gerente = gameObject.GetComponentInParent<Manager>();

    }

    void Start()
    {
        //currentMenu = ChatWindow;
        //showMenu = false;

    }


    void Update()
    {

        //Debug.Log(timer);

        if (gerente.gameState == GameState.playing)
        {
            timer += Time.deltaTime;
            Debug.Log(timer.ToString());
           guiOnLine.UpdateTimeOnScreen(timer.ToString());
        }

        if (timer >= gerente.setUpTimer)
        {
            EndCount();

            ilha.timeUP = true;

            PlayerActionOnLine winner = ilha.GetWinner();




            if (!GameObject.Equals(winner, null))
            {
                guiOnLine.HitEnter(winner.Name + " Venceu!!!", "");


            }
            else
            {
                guiOnLine.HitEnter("Ninguem venceu :(", "");
            }

            GetComponent<NetworkView>().RPC("ReiniciarJogo", RPCMode.All);

            ilha.Begin();
            // gerenteRede.SetGameState(GameStateOnLine.clear);
            //reiniciar a gurizada for each na ilha pra cada player perder os pontos

        }
    }

    public void BeginCount()
    {
      
        gerente.SetGameState(GameState.playing);
       

        timer = 0;

    }


    public void EndCount()
    {
       
        gerente.SetGameState(GameState.clear);
        timer = 0;

    }



    public void ContinueCount(float timeServer)
    {

        timer = timeServer;
        gerente.SetGameState(GameState.playing);


    }


    public void SetJogoTimeAtack(NetworkMessageInfo info)
    {
        GetComponent<NetworkView>().RPC("SetTimeAtack", RPCMode.All, timer);

    }




    public void IniciarTimeAtackServer()
    {
        if (Network.isServer)
        {
            BeginCount();
            Debug.Log("qqqqwqwq");
        }

    }

    public void SetStageModeServer(string stage, string playerdestino, GameModeOnLine gamemode)
    {
        GetComponent<NetworkView>().RPC("SetStageMode", RPCMode.All, stage, playerdestino, gamemode);
    }



    [RPC]
    bool TellServerOurName(String name, NetworkMessageInfo info)
    {
        if (Network.isServer)
        {
            if (AddNewPlayer(name, info.sender))
            {
                guiOnLine.HitEnter(name + " joined the chat", "");

            }

        }
        return true;

    }


    public bool AddNewPlayer(String name, NetworkPlayer player)
    {
        Debug.Log(name);
        if (CheckNomeRepedido(name))
        {
            //Network.CloseConnection (info.sender, true);
            guiOnLine.HitEnter("Nome Repetido Jogador mais recente será desconectado", name);

            Network.CloseConnection(player, true);
            return false;
        }
        else
        {
            PlayerChat newPlayer = new PlayerChat();
            newPlayer.name = name;

            newPlayer.networkPlayer = player;

            playerList.Add(newPlayer);

            SetStageModeServer(Application.loadedLevelName, name, gerente.gameModeOnLine);
            return true;
        }

    }


    public bool CheckNomeRepedido(string name)
    {
        foreach (PlayerChat playerPlaying in playerList)
        {
            if (playerPlaying.name == name)
            {

                //Network.CloseConnection(info.sender,true);
                //networkView.RPC ("NameInUse", RPCMode.Others,info.sender);
                //NameInUse();
                //nameinuse = true;
                //break;

                return true;
            }
        }

        return false;
    }



    public string RemovePlayer(NetworkPlayer player)
    {

        //Remove playerstatus from the server list

        var playerRemovedfromGame = GetPlayer(player);

        if (!object.ReferenceEquals(playerRemovedfromGame, null))
        {
            string playerremovefromgamename = playerRemovedfromGame.name;
            //Remove player from the server list
            playerList.Remove(playerRemovedfromGame);
            //Debug.Log (playerRemovedfromGame.playerName);
            return playerremovefromgamename;
        }
        else
            return "";

    }


    PlayerChat GetPlayer(NetworkPlayer networkPlayer)
    {

        foreach (PlayerChat entry in playerList)
        {
            Debug.Log(entry.name);
            if (entry.networkPlayer == networkPlayer)
                return entry;

        }

        return null;

    }




    [RPC]
    bool ComandodoJogo(string command, string name, NetworkMessageInfo info)
    {
        if (Network.isServer)
        {
            var returnFromServer = ilha.movePlayer(name, command, info.sender);

            //if (returnFromServer != "")
            //{
            //    //ApplyGlobalChatTextSERVER ("", name + " Executed command: " + command + "Posicao=(" + possx + "," + possy + ")" + postesourox + "  " + postesouroy + "");
            //    menu.guiOnLine.HitEnter(returnFromServer, name);

            //}
            //networkView.RPC("ReceiveName", RPCMode.All, name, ilha.GetPlayer(info.sender).Tag);//avisar os clientes da mudança do player
        }
        return true;


    }



    [RPC]
    bool Entrarnojogo(String nome, NetworkMessageInfo info)
    {

        if (Network.isServer)
        {

            if (ilha.AddNewPlayer(nome, info.sender))
            {
                guiOnLine.HitEnter(ilha.GetPlayer(nome).Name + " joined the Game", "");
                guiOnLine.HitEnter("Bem vindo ao jogo", ilha.GetPlayer(nome).Name);


                Debug.Log(ilha.GetPlayer(nome).Tag);

              
                GetComponent<NetworkView>().RPC("SetTag", RPCMode.All, ilha.GetPlayer(nome).Tag, ilha.GetPlayer(nome).Name);

                if ( gerente.gameState== GameState.playing)
                {
                    SetJogoTimeAtack(info);

                }


            }
            else
            {
                guiOnLine.HitEnter("Voce nao pode se conectar ao jogo pois seu nome ja esta sendo usado", ilha.GetPlayer(nome).Name);

            }


        }
        return true;
    }



    [RPC]
    bool Sairdojogo(string nome, NetworkMessageInfo info)
    {
        if (Network.isServer)
        {

            string hh = ilha.RemovePlayerN(info.sender);

            Debug.Log("saiu do jogo  " + hh);

            if (hh != "")
            {


               
                GetComponent<NetworkView>().RPC("LeaveGame", RPCMode.All, hh);

                guiOnLine.HitEnter(hh + " left the Game", "");
            }

        }
        return true;
    }


    [RPC]
    bool ReceiveStatus(int coin, int life, bool flag, string playername, NetworkMessageInfo info)
    {



        if (Network.isServer)
        {


            Debug.Log("recebendo ConnectionTesterStatus");

            var returnFromServer = ilha.StatusPlayer(coin, life, flag, playername);

            if (returnFromServer != "")
            {
                guiOnLine.HitEnter(returnFromServer, playername);

                PlayerActionOnLine winner = ilha.GetWinner();



                if (!GameObject.Equals(winner, null))
                {
                    guiOnLine.HitEnter(winner.Name + " Venceu!!!", "");
                    GetComponent<NetworkView>().RPC("ReiniciarJogo", RPCMode.All);

                }

            }

           
        }




        return true;
    }




}



public class Ilha2
{


    //private	int[,] mapa = new int[2, 2];


    private GameModeOnLine gamemode;

    public bool timeUP;

    public List<PlayerActionOnLine> playerListIlha = new List<PlayerActionOnLine>();

    static NetworkPlayer playertoberemoved;

    // public ArrayList playerListIlha = new ArrayList();

    public Ilha2(GameModeOnLine gameMode)
    {
        gamemode = gameMode;
        timeUP = false;
        //postesourox = UnityEngine.Random.Range (0, 20);
        //postesouroy = UnityEngine.Random.Range (0, 20);



        Begin();

    }

    public void RestartIlha(GameModeOnLine gameMode)
    {
        gamemode = gameMode;
        timeUP = false;

    }





    public bool CheckNomeRepedido(string nome)
    {
        foreach (PlayerActionOnLine playerPlaying in playerListIlha)
        {
            if (playerPlaying.Name == nome)
            {
                Debug.Log(playerPlaying.Name);
                //Network.CloseConnection(info.sender,true);
                //networkView.RPC ("NameInUse", RPCMode.Others,info.sender);
                //NameInUse();
                //nameinuse = true;
                //break;

                return true;
            }
        }

        return false;
    }

    public bool CheckTagRepedido(string tag)
    {
        foreach (PlayerActionOnLine playerPlaying in playerListIlha)
        {
            if (playerPlaying.Tag == tag)
            {

                //Network.CloseConnection(info.sender,true);
                //networkView.RPC ("NameInUse", RPCMode.Others,info.sender);
                //NameInUse();
                //nameinuse = true;
                //break;

                return true;
            }
        }

        return false;
    }

    public PlayerActionOnLine GetPlayerN(NetworkPlayer networkPlayer)
    {
        foreach (PlayerActionOnLine entry in playerListIlha)
        {
            // Debug.Log(entry.Name);
            if (entry.networkPlayer == networkPlayer)
                return entry;

        }
        Debug.LogError("GetPlayerNode: Requested a playernode of non-existing player!");
        return null;
    }

    public PlayerActionOnLine GetPlayer(string nome)
    {

        foreach (PlayerActionOnLine entry in playerListIlha)
        {

            if (entry.Name == nome)
                return entry;

        }
        Debug.LogError("GetPlayerNode: Requested a playernode of non-existing player!");
        return null;
    }

    public PlayerActionOnLine GetWinner()
    {

        if (gamemode == GameModeOnLine.takeflag)
        {

            foreach (PlayerActionOnLine entry in playerListIlha)
            {
                Debug.Log("flagggg" + entry.flag);
                if (entry.flag)
                {
                    return entry;


                }

            }
        }

        else if (gamemode == GameModeOnLine.timeatack && timeUP)
        {

            int Max = 0;
            PlayerActionOnLine ChosenOne = null;

            foreach (PlayerActionOnLine entry in playerListIlha)
            {



                if (entry.GetPlacar()[0] > Max)
                {
                    Max = entry.GetPlacar()[0];
                    ChosenOne = entry;
                }


            }


            return ChosenOne;

        }

        return null;
    }




    public bool AddNewPlayer(string nome, NetworkPlayer player)
    {
        Debug.Log("adddddddddddddddddddddddddddded " + nome);
        if (CheckNomeRepedido(nome))
        {
            //Network.CloseConnection (info.sender, true);
            //Network.CloseConnection(player, true);
            return false;
        }
        else
        {


            PlayerActionOnLine newPlayer = new PlayerActionOnLine();

            if (!CheckTagRepedido("player2"))
                newPlayer.Tag = "player2";
            else
                if (!CheckTagRepedido("player3"))
                newPlayer.Tag = "player3";
            else
                    if (!CheckTagRepedido("player4"))
                newPlayer.Tag = "player4";
            else
                newPlayer.Tag = "player1";

            newPlayer.Name = nome;

            newPlayer.networkPlayer = player;

            playerListIlha.Add(newPlayer);

            Debug.Log("added " + nome);

            return true;
        }

    }



    public bool AddNewPlayer(String name, NetworkPlayer player, PlayerActionOnLine newPlayer)
    {

        if (CheckNomeRepedido(name))
        {
            //Network.CloseConnection (info.sender, true);
            Network.CloseConnection(player, true);
            return false;
        }
        else
        {

            if (!CheckTagRepedido("player2"))
                newPlayer.Tag = "player2";
            else
                if (!CheckTagRepedido("player3"))
                newPlayer.Tag = "player3";
            else
                    if (!CheckTagRepedido("player4"))
                newPlayer.Tag = "player4";
            else
                newPlayer.Tag = "player1";

            newPlayer.Name = name;

            newPlayer.networkPlayer = player;

            playerListIlha.Add(newPlayer);



            return true;
        }

    }

    private static bool SpecificNetworkPlayer(PlayerActionOnLine s)
    {
        if (s.networkPlayer == playertoberemoved)
            return true;
        else return false;
    }


    public string RemovePlayerN(NetworkPlayer player)
    {

        //Remove playerstatus from the server list
        playertoberemoved = player;

        var playerRemovedfromGame = GetPlayerN(player);

        if (!object.ReferenceEquals(playerRemovedfromGame, null))
        {
            string playerremovefromgamename = playerRemovedfromGame.Name;

            playerListIlha.RemoveAll(SpecificNetworkPlayer);


            foreach (PlayerActionOnLine ff in playerListIlha)
            {
                Debug.Log("depois                                                     " + ff.Name);
            }



            return playerremovefromgamename;
        }
        else
            return "";

    }


    public string RemovePlayer(string nome)
    {



        var playerRemovedfromGame = GetPlayer(nome);

        if (!object.ReferenceEquals(playerRemovedfromGame, null))
        {
            string playerremovefromgamename = playerRemovedfromGame.Name;




            playerListIlha.Remove(playerRemovedfromGame);

            foreach (PlayerActionOnLine ff in playerListIlha)
            {
                // Debug.Log("depois                                                     "+ff.Name);
            }

            return playerremovefromgamename;
        }
        else
            return "";

    }





    public bool Begin()
    {




        foreach (PlayerActionOnLine jogadorrecomeca in playerListIlha)
        {
            try
            {
                // jogadorrecomeca.SetPlacar(0, 0);


                //jogadorrecomeca. = (Area)mapa[3];
                //jogadorrecomeca.items.RemoveRange(0, jogadorrecomeca.items.Count);
            }
            catch
            {
            }



        }




        return true;
    }





    public string movePlayer(String name, string comando, NetworkPlayer Nplayer)
    {
        PlayerActionOnLine dd = GetPlayerN(Nplayer);

        dd.Tag = comando;

        return dd.Tag;
    }

    public string StatusPlayerN(int coin, int life, bool flag, NetworkPlayer Nplayer)
    {
        PlayerActionOnLine dd = GetPlayerN(Nplayer);

        dd.SetPlacar(coin, life, flag);
        return dd.Tag + "  coin " + dd.GetPlacar()[0].ToString() + "  life " + dd.GetPlacar()[1].ToString();
    }

    public string StatusPlayer(int coin, int life, bool flag, string nome)
    {



        PlayerActionOnLine dd = GetPlayer(nome);

        dd.SetPlacar(coin, life, flag);

        return dd.Tag + "  coin " + dd.GetPlacar()[0].ToString() + "  life " + dd.GetPlacar()[1].ToString();
    }



}
