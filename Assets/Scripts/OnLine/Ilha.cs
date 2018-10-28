using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class Ilha : MonoBehaviour
{


    //private	int[,] mapa = new int[2, 2];

    bool contando;
    private GameModeOnLine gamemode;

    float setupTimer;
    public float timer;

    public ArrayList playerList = new ArrayList();

    public Ilha(GameModeOnLine gameMode, float setUpTimer)
    {
        gamemode = gameMode;
        setupTimer = setUpTimer;
        contando = false;
        //postesourox = UnityEngine.Random.Range (0, 20);
        //postesouroy = UnityEngine.Random.Range (0, 20);



        Begin();

    }

    public void RestartIlha(GameModeOnLine gameMode, float setUpTimer)
    {
        gamemode = gameMode;
        setupTimer = setUpTimer;
        contando = false;
    }


    void Update()
    {

        //Debug.Log(timer);
        Debug.Log(contando.ToString());
        if (contando)
        {
            timer += Time.deltaTime;
            Debug.Log(timer.ToString());
        }

        if (timer >= setupTimer)
            EndCount();
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

    public bool CheckNomeRepedido(string name)
    {
        foreach (PlayerActionOnLine playerPlaying in playerList)
        {
            if (playerPlaying.Name == name)
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

    public bool CheckTagRepedido(string tag)
    {
        foreach (PlayerActionOnLine playerPlaying in playerList)
        {
            if (playerPlaying.name == tag)
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

    public PlayerActionOnLine GetPlayer(NetworkPlayer networkPlayer)
    {
        foreach (PlayerActionOnLine entry in playerList)
        {
            // Debug.Log(entry.Name);
            if (entry.networkPlayer == networkPlayer)
                return entry;

        }
        Debug.LogError("GetPlayerNode: Requested a playernode of non-existing player!");
        return null;
    }

    public PlayerActionOnLine GetWinner()
    {

        if (gamemode == GameModeOnLine.takeflag)
        {

            foreach (PlayerActionOnLine entry in playerList)
            {

                if (entry.flag)
                    return entry;

            }
        }
        else if (gamemode == GameModeOnLine.timeatack && timer > setupTimer)
        {

            int Max = 0;
            PlayerActionOnLine ChosenOne = null;

            foreach (PlayerActionOnLine entry in playerList)
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




    public bool AddNewPlayer(String name, NetworkPlayer player)
    {

        if (CheckNomeRepedido(name))
        {
            //Network.CloseConnection (info.sender, true);
            Network.CloseConnection(player, true);
            return false;
        }
        else
        {


            PlayerActionOnLine newPlayer = new PlayerActionOnLine();

            if (!CheckTagRepedido("player2"))
                newPlayer.Tag = "player2";
            else
                if (!CheckTagRepedido("player3") == null)
                    newPlayer.Tag = "player3";
                else
                    if (!CheckTagRepedido("player4") == null)
                        newPlayer.Tag = "player4";
                    else
                        newPlayer.Tag = "player1";

            newPlayer.Name = name;

            newPlayer.networkPlayer = player;

            playerList.Add(newPlayer);

            Debug.Log("added " + name);

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
                if (!CheckTagRepedido("player3") == null)
                    newPlayer.Tag = "player3";
                else
                    if (!CheckTagRepedido("player4") == null)
                        newPlayer.Tag = "player4";
                    else
                        newPlayer.Tag = "player1";

            newPlayer.Name = name;

            newPlayer.networkPlayer = player;

            playerList.Add(newPlayer);



            return true;
        }

    }



    public string RemovePlayer(NetworkPlayer player)
    {

        //Remove playerstatus from the server list

        var playerRemovedfromGame = GetPlayer(player);

        if (!object.ReferenceEquals(playerRemovedfromGame, null))
        {
            string playerremovefromgamename = playerRemovedfromGame.Name;
            //Remove player from the server list
            playerList.Remove(playerRemovedfromGame);
            //Debug.Log (playerRemovedfromGame.playerName);
            return playerremovefromgamename;
        }
        else
            return "";

    }

    //        public string movePlayer2(String name, string command, NetworkPlayer player)
    //        {
    //                int possx = 0;
    //                int possy = 0;


    //                string answerToPlayer = "";

    //                var playerToComand = GetPlayer (player);

    //                switch (command) {

    //                case "direita;":
    //                        if (playerToComand.posx < mapa.Length) {
    //                                playerToComand.posx += 1;
    //                                answerToPlayer = "Voce foi pra direita";
    //                        } else
    //                                answerToPlayer = "Voce nao pode ir pra direita";
    //                        break;

    //                case "esquerda;":
    //                        if (playerToComand.posx > 0) {
    //                                playerToComand.posx -= 1;
    //                                answerToPlayer = "Voce foi pra esquerda";
    //                        } else
    //                                answerToPlayer = "Voce nao pode ir pra esquerda";
    //                        break;

    //                case "cima;":
    //                        if (playerToComand.posy < mapa.Length) {
    //                                playerToComand.posy += 1;
    //                                answerToPlayer = "Voce foi pra cima";
    //                        } else
    //                                answerToPlayer = "Voce nao pode ir pra cima";
    //                        break;

    //                case "baixo;":
    //                        if (playerToComand.posy > 0) {
    //                                playerToComand.posy -= 1;
    //                                answerToPlayer = "Voce foi pra baixo";
    //                        } else
    //                                answerToPlayer = "Voce nao pode ir pra baixo";
    //                        break;
    //                case "myposition;":
    //                        answerToPlayer = PosicaoPlayer (playerToComand);
    //                        break;
    //                default:
    //                        break;
    //                }

    //                //tem q atualizar o player playerList.


    //                if (playerToComand.posx == postesourox && playerToComand.posy == postesouroy) {

    //                        achado = true;
    //                        return playerToComand.name + " Ganhou o jogo";

    //                }

    //                //return name + " Executed command: " + command + "Posicao=(" + possx + "," + possy + ")" + postesourox + "  " + postesouroy + "";

    //                if (answerToPlayer == "")
    //                        answerToPlayer = "Comando invalido";

    //                return answerToPlayer;	

    ////				if (achado) {
    ////						postesourox = UnityEngine.Random.Range (0, 20);
    ////						postesouroy = UnityEngine.Random.Range (0, 20);
    ////					
    ////						HitEnter ("O proximo tesouro esta na posicao " + postesourox + "  " + postesouroy + "");	
    ////						achado = false;
    ////				
    ////				}

    //        }





    bool Begin()
    {




        foreach (PlayerActionOnLine jogadorrecomeca in playerList)
        {
            try
            {
                //jogadorrecomeca.moedas = 0;
                //jogadorrecomeca.areaatual = (Area)mapa[3];
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
        PlayerActionOnLine dd = GetPlayer(Nplayer);

        dd.Tag = comando;

        return dd.Tag;
    }

    public string StatusPlayer(int coin, int life,bool flag, NetworkPlayer Nplayer)
    {
        PlayerActionOnLine dd = GetPlayer(Nplayer);

        dd.SetPlacar(coin, life,flag);
        return dd.Tag + "  coin " + dd.GetPlacar()[0].ToString() + "  life " + dd.GetPlacar()[1].ToString();
    }
}
