using UnityEngine;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class CLIENT : MonoBehaviour
{

    Manager gerente;
    void Awake()
    {

        gerente = gameObject.GetComponentInParent<Manager>();

    }

    void OnConnectedToServer()
    {



        GetComponent<NetworkView>().RPC("TellServerOurName", RPCMode.Server, gerente.connectplayerName);




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
        Network.RemoveRPCs(GetComponent<NetworkView>().owner);


        gerente.SetGameState(GameState.gameover);


    }








}
