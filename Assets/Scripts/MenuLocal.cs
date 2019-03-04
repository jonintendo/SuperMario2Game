using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLocal : MonoBehaviour {


    Manager gerente;
    public GameObject Player1;
   

    void Awake()
    {

        gerente = gameObject.GetComponentInParent<Manager>();
        gerente.OnStateChange += HandleOnStateChange;


    }


    void Start () {
		
	}

    void Update()
    {



        if (Input.GetKeyDown(KeyCode.Escape))
        {

            switch (gerente.gameState)
            {
                case GameState.playing:
                    gerente.SetGameState(GameState.pause);
                    break;
                case GameState.pause:
                    gerente.SetGameState(GameState.playing);
                    break;
            }

        }




    }



    public void HandleOnStateChange()
    {

        switch (gerente.gameState)
        {
            case GameState.novafase:
                gerente.NextStage();
                break;


            case GameState.gameover:
                //RemovePlayer();
                break;

            case GameState.playing:
                NewPlayer();
                
                break;

            case GameState.opening:
                RemovePlayer();

                break;

            case GameState.clearStage:
                break;
        }

    }


   


    public void NewPlayer()
    {
      

        if (gerente.player==null)
        {
            gerente.player = Instantiate(Player1);
        }

    }

   

    public void RemovePlayer()
    {
        if (gerente.player != null)
        {
            DestroyImmediate(gerente.player);
            gerente.player = null;
        }

    }


}
