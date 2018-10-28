



    using UnityEngine;
using System.Collections;



public enum GameStateOnLine { gameover, clear, playing, opening, restarting, pause, dieing, novafase }
public delegate void OnStateRedeChangeHandler();


public class ManagerOnLine : MonoBehaviour
{

    //GUIText coinst;
    //GUIText lifet;

    public GameObject Player1;
   // PlayerAction playeraction;
    PlayerActionOnLine playeractiononline;

    public event OnStateChangeHandler OnStateChange;
    public GameStateOnLine gameState { get; private set; }




    public void SetGameState(GameStateOnLine gameState)
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

        OnStateChange += HandleOnStateRedeChange;

     
        //coinst = this.gameObject.GetComponentsInChildren<GUIText>()[0];

        //lifet = this.gameObject.GetComponentsInChildren<GUIText>()[1];
    }


    public void HandleOnStateRedeChange()
    {
        switch (gameState)
        {

            case GameStateOnLine.clear:

                
                break;

            case GameStateOnLine.playing:


                break;

            case GameStateOnLine.gameover:

               // RemovePlayer();
                break;

            case GameStateOnLine.restarting:

                //RestartPlayer();
                break;

            case GameStateOnLine.opening:

               // RemovePlayer();
                break;


            case GameStateOnLine.pause:


                break;

            case GameStateOnLine.dieing:
                //if (Player1 != null)
                //    RemovePlayer();
                break;

            case GameStateOnLine.novafase:
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

            case GameStateOnLine.clear:
                break;

            case GameStateOnLine.playing:


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

            case GameStateOnLine.gameover:


                break;

            case GameStateOnLine.restarting:


                break;

            case GameStateOnLine.opening:


                break;


            case GameStateOnLine.pause:


                break;

            case GameStateOnLine.dieing:

                break;
        }




    }


    //public void NewPlayer(GameObject Player)
    //{
    //    if (Player1 == null)
    //    {
    //        Player1 = Player;
    //        playeractiononline = Player1.GetComponent<PlayerActionOnLine>();
    //    }
    //    else if (Player1.tag != Player.tag && Player.tag == "player1")
    //    {
    //        Player1 = Player;
    //        playeractiononline = Player1.GetComponent<PlayerActionOnLine>();
    //    }

      

    //    Debug.Log("Added w player" + Player.name);

    //}


    //public void RemovePlayer(GameObject Player)
    //{
    //    Destroy(Player);
    //    Player1 = null;
    //}


    //public void RemovePlayer()
    //{
    //    Destroy(Player1);
    //    Player1 = null;

    //}

    //public void RestartPlayer(GameObject Player)
    //{
       
    //        playeractiononline.restart(); ;
      
    //}

    //public void RestartPlayer()
    //{
      
    //        playeractiononline.restart(); ;
    
    //}





}


