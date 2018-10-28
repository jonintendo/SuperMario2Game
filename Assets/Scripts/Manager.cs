using UnityEngine;
using System.Collections;



public enum GameState { gameover, clear, playing, opening, restarting, pause, dieing, novafase,online }
public delegate void OnStateChangeHandler();

public class Manager : MonoBehaviour
{

    //GUIText coinst;
    //GUIText lifet;

    public GameObject Player1;
    PlayerAction playeraction;
   // PlayerActionOnLine playeractiononline;

    public event OnStateChangeHandler OnStateChange;
    public GameState gameState { get; private set; }




    public void SetGameState(GameState gameState)
    {

        this.gameState = gameState;
        //Debug.Log(this.gameState.ToString());
        if (OnStateChange != null)
        {
            OnStateChange();
        }
    }


    void Start()
    {

    }
    void Awake()
    {

        OnStateChange += HandleOnStateChange;

        //coinst = this.gameObject.GetComponentsInChildren<GUIText>()[0];

        //lifet = this.gameObject.GetComponentsInChildren<GUIText>()[1];
    }


    public void HandleOnStateChange()
    {
        switch (gameState)
        {

            case GameState.clear:
                

                break;

            case GameState.playing:


                break;

            case GameState.gameover:

               // RemovePlayer();
                break;

            case GameState.restarting:

                //RestartPlayer();
                break;

            case GameState.opening:

               // RemovePlayer();
                break;


            case GameState.pause:


                break;

            case GameState.dieing:
                //if (Player1 != null)
                //    RemovePlayer();
                break;

            case GameState.novafase:
                //if (Player1 != null)
                //    RemovePlayer();
                break;
        }
    }

    void OnLevelWasLoaded(int level)
    {
        // if (level == 3)

        //if (conectado)
        //{ }
        //else
        //SetGameState(GameState.playing);

    }

    void Update()
    {

        switch (gameState)
        {

            case GameState.clear:
                break;

            case GameState.playing:


                //if (Input.GetKeyDown(KeyCode.Escape))
                //{
                //    SetGameState(GameState.pause);

                //}

                // Player1 = GameObject.FindGameObjectWithTag("player1");
                //if (Player1 != null)
                //{

                //    if (playeraction != null)
                //    {
                //        coinst.text = playeraction.coins.ToString();
                //        lifet.text = playeraction.lifes.ToString();
                //    }
                //    else
                //        if (playeractiononline != null)
                //        {

                //            coinst.text = playeractiononline.coins.ToString();
                //            lifet.text = playeractiononline.lifes.ToString();
                //        }

                //}

                break;

            case GameState.gameover:


                break;

            case GameState.restarting:


                break;

            case GameState.opening:


                break;


            case GameState.pause:


                break;

            case GameState.dieing:

                break;
        }




    }


    public void NewPlayer(GameObject Player)
    {
        if (Player1 == null)
        {
            Player1 = Player;
            playeraction = Player1.GetComponent<PlayerAction>();
        }
        else if (Player1.tag != Player.tag && Player.tag == "player1")
        {
            Player1 = Player;
            playeraction = Player1.GetComponent<PlayerAction>();
        }

        //if (playeraction == null)
        //{
        //    playeractiononline = Player1.GetComponent<PlayerActionOnLine>();
        //}

        Debug.Log("Added w player" + Player.name);

    }


    public void RemovePlayer(GameObject Player)
    {
        Destroy(Player);
        Player1 = null;
    }


    public void RemovePlayer()
    {
        Destroy(Player1);
        Player1 = null;

    }

    public void RestartPlayer(GameObject Player)
    {
        //if (playeraction == null)
        //{
        //    playeractiononline.restart(); ;
        //}
        //else
            playeraction.restart();

    }

    public void RestartPlayer()
    {
        //if (playeraction == null)
        //{
        //    playeractiononline.restart(); ;
        //}
        //else
            playeraction.restart();

    }





}
