using UnityEngine;
using System.Collections;
using System;




public class MenuRede : MonoBehaviour
{

    public Manager gerente;



    public GameObject PlayerRede;
    public GameObject PlayerRedeInstantiated;
    // PlayerActionOnLine playeractiononline;


    public GameObject mario;
    public GameObject luigi;
    public GameObject wario;
    public GameObject mimico;
    public GameObject fogo;


   


    /*void AdjustPlayer()
    {


        playername = PlayerPrefs.GetString("playerName", gerente.connectplayerName);

        if (playername == null || playername == "")
        {
            playername = "RandomName" + UnityEngine.Random.Range(1, 999);
        }


    }*/


    void Start()
    {


    }

    void Awake()
    {



        gerente = gameObject.GetComponentInParent<Manager>();

        gerente.OnStateChange += HandleOnStateChange;
        gerente.OnModeOnLineChange += HandleOnModeOnLineChange;


    }

    void Update()
    {
       

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

                if (!(Network.peerType == NetworkPeerType.Disconnected))
                {
                    Network.Disconnect(200);
                }


                Start();

                break;


            case GameState.pause:


                break;

            case GameState.dieing:

                PlayerRedeInstantiated.GetComponent<PlayerActionOnLine>().restart();


                break;

            case GameState.novafase:

                break;
        }
        Debug.Log("Handling state Rede change to: " + gerente.gameState);
    }


    public void HandleOnModeOnLineChange()
    {


        switch (gerente.gameModeOnLine)
        {

            case GameModeOnLine.takeflag:
               
                break;

            case GameModeOnLine.timeatack:
                iniciarJogoTimeAtack();

                break;
        }

    }


    public void SetRoupa(PlayerRoupa roupa)
    {
        gerente.SetRoupa(roupa);

        switch (roupa)
        {

            case PlayerRoupa.luigi:
                PlayerRede = luigi;
                break;

            case PlayerRoupa.fogo:
                PlayerRede = fogo;
                break;

            case PlayerRoupa.mimico:
                PlayerRede = mimico;
                break;

            case PlayerRoupa.mario:
                PlayerRede = mario;
                break;

            case PlayerRoupa.wario:
                PlayerRede = wario;
                break;

        }


    }

    public void iniciarJogo(string tagPlayer)
    {


        SetRoupa(gerente.roupa);
        PlayerRede.tag = tagPlayer;
        PlayerRedeInstantiated = (GameObject)Network.Instantiate(PlayerRede, PlayerRede.transform.position, PlayerRede.transform.rotation, 0);


        PlayerRedeInstantiated.tag = tagPlayer;
        PlayerRedeInstantiated.GetComponent<PlayerActionOnLine>().Tag = tagPlayer;


    }

    public void iniciarJogoTimeAtack()
    {

        GetComponent<NetworkView>().RPC("IniciarTimeAtack", RPCMode.All, gerente.connectplayerName);


        this.gameObject.GetComponentInChildren<SERVER>().IniciarTimeAtackServer();

        //networkView.RPC("IniciarTimeAtackServer", RPCMode.All, playername);
    }




    public void terminarJogo()
    {

        gerente.SetGameState(GameState.gameover);

        Network.Destroy(PlayerRedeInstantiated);
        //}

        // Network.RemoveRPCs(Network.player);
        // Network.DestroyPlayerObjects(Network.player);

    }







    public void ReiniciarJogoOnline()
    {
        GetComponent<NetworkView>().RPC("ReiniciarJogo", RPCMode.All);
    }


    [RPC]
    void SetStageMode(string stage, string playerdestino, GameModeOnLine gamemode)
    {

        if (gerente.connectplayerName == playerdestino)
        {
            if (Network.isClient)
            {
                Application.LoadLevel(stage);
                 gerente.SetGameModeOnLine(gamemode);

               

            }



        }




    }

   



    [RPC]
    void SetTag(string tagPlayer, string playerdestino)
    {

        if (gerente.connectplayerName == playerdestino)
        {

            iniciarJogo(tagPlayer);


            //PlayerRedeInstantiated.GetComponent<PlayerActionOnLine>().OnlineTagsPlacar();
        }



    }

    public void SetTagServer(string tagPlayer, string playerdestino)
    {
        GetComponent<NetworkView>().RPC("SetTag", RPCMode.All, tagPlayer, playerdestino);
    }



    [RPC]
    void LeaveGame(string playerdestino)
    {
        if (gerente.connectplayerName == playerdestino)
            terminarJogo();

    }

    public void LeaveGameServer(string playerdestino)
    {

        GetComponent<NetworkView>().RPC("LeaveGame", RPCMode.All, playerdestino);
    }





    [RPC]
    void IniciarTimeAtack(string playerdestino)
    {

       // BeginCount();
    }

    [RPC]
    void SetTimeAtack(float timeServer)
    {

        //ContinueCount(timeServer);
    }



    [RPC]
    void ReiniciarJogo()
    {
        if (PlayerRedeInstantiated != null)
            PlayerRedeInstantiated.GetComponent<PlayerActionOnLine>().restart();
    }



  



}
