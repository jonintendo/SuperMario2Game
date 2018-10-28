using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SERVER : MonoBehaviour
{



    MenuRede menuRede;
    public Ilha2 ilha;

    bool contando;
    //  float setupTimer;
    public float timer;

    public ArrayList playerList = new ArrayList();

    class PlayerChat
    {
        public String name = "";
        public NetworkPlayer networkPlayer;
    }

    void OnServerInitialized()
    {


        // window = new Rect(150, 150, width, height);
        menuRede = GetComponent<MenuRede>();

        //currentMenu = ChatWindow;

        AddNewPlayer(menuRede.playername, Network.player);

        menuRede.HitEnter("server ativo", "");

        //showMenu = true;

        ilha = new Ilha2(menuRede.gameMode);

    }


    void OnPlayerDisconnected(NetworkPlayer player)
    {
        menuRede.HitEnter("Player disconnected from: " + player.ipAddress + ":" + player.port, "");

        string hhh = ilha.RemovePlayerN(player);

        if (hhh != "")
        {
            menuRede.HitEnter(hhh + " left the Game", "");
        }


        string hh = RemovePlayer(player);

        if (hh != "")
        {
            menuRede.HitEnter(hh + " left the chat", "");
        }

        Network.DestroyPlayerObjects(player);
        Network.RemoveRPCs(player);



    }




    void OnPlayerConnected(NetworkPlayer player)
    {

        menuRede.HitEnter("Player connected from: " + player.ipAddress + ":" + player.port, "");


    }


    void OnDisconnectedFromServer()
    {

        playerList = new ArrayList();

        Debug.Log("server desconectou");
    }


    void Awake()
    {

        menuRede = GetComponent<MenuRede>();
    }

    void Start()
    {
        //currentMenu = ChatWindow;
        //showMenu = false;

    }


    void Update()
    {

        //Debug.Log(timer);

        if (contando)
        {
            timer += Time.deltaTime;
            Debug.Log(timer.ToString());
        }

        if (timer >= menuRede.setUpTimer)
        {
            EndCount();

            ilha.timeUP = true;

            PlayerActionOnLine winner = ilha.GetWinner();




            if (!GameObject.Equals(winner, null))
            {
                menuRede.HitEnter(winner.Name + " Venceu!!!", "");


            }
            else
            {
                menuRede.HitEnter("Ninguem venceu :(", "");
            }

            menuRede.ReiniciarJogoOnline();

            ilha.Begin();
            // gerenteRede.SetGameState(GameStateOnLine.clear);
            //reiniciar a gurizada for each na ilha pra cada player perder os pontos

        }
    }

    public void BeginCount()
    {
        contando = true;
        Debug.Log(contando.ToString());

        timer = 0;

    }


    public void EndCount()
    {
        contando = false;
        timer = 0;

    }





    public void IniciarTimeAtackServer()
    {
        if (Network.isServer)
        {
            BeginCount();
            Debug.Log("qqqqwqwq");
        }

    }

    [RPC]
    bool TellServerOurName(String name, NetworkMessageInfo info)
    {
        if (Network.isServer)
        {
            if (AddNewPlayer(name, info.sender))
            {
                menuRede.HitEnter(name + " joined the chat", "");

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
            menuRede.HitEnter("Nome Repetido Jogador mais recente será desconectado", name);

            Network.CloseConnection(player, true);
            return false;
        }
        else
        {
            PlayerChat newPlayer = new PlayerChat();
            newPlayer.name = name;

            newPlayer.networkPlayer = player;

            playerList.Add(newPlayer);

            menuRede.SetStageModeServer(Application.loadedLevelName, name, menuRede.gameMode.ToString());
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
            //    menu.HitEnter(returnFromServer, name);

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
                menuRede.HitEnter(ilha.GetPlayer(nome).Name + " joined the Game", "");
                menuRede.HitEnter("Bem vindo ao jogo", ilha.GetPlayer(nome).Name);


                Debug.Log(ilha.GetPlayer(nome).Tag);

                menuRede.SetTagServer(ilha.GetPlayer(nome).Tag, ilha.GetPlayer(nome).Name);


                if (contando)
                {
                    menuRede.SetJogoTimeAtack(info);

                }


            }
            else
            {
                menuRede.HitEnter("Voce nao pode se conectar ao jogo pois seu nome ja esta sendo usado", ilha.GetPlayer(nome).Name);

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


                menuRede.LeaveGameServer(hh);

                menuRede.HitEnter(hh + " left the Game", "");
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
                menuRede.HitEnter(returnFromServer, playername);

                PlayerActionOnLine winner = ilha.GetWinner();



                if (!GameObject.Equals(winner, null))
                {
                    menuRede.HitEnter(winner.Name + " Venceu!!!", "");
                    menuRede.ReiniciarJogoOnline();

                }

            }

            //PlayerRedeInstantiated.tag = tag;
            //TextMesh nametext = PlayerRedeInstantiated.gameObject.GetComponentsInChildren<TextMesh>()[0];
            //nametext.text = PlayerRedeInstantiated.tag;




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
                Debug.Log("flagggg"+entry.flag);
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
