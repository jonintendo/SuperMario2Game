using UnityEngine;
using System.Collections;
using System;

public enum GameState { gameover, clear, playing, opening, restarting, pause, dieing, novafase, clearStage }
public enum GameStateNetwork { local, online }
public enum GameModeOnLine { timeatack, takeflag }
public enum PlayerRoupa { luigi, mario, mimico, fogo, wario }


public delegate void OnStateChangeHandler();

public class Manager : MonoBehaviour
{

    public GameObject menu;
    public GameObject menurede;

    public event OnStateChangeHandler OnStateChange;
    public GameState gameState { get; private set; }
    string lastGameStage;
    public GameState lastGameState;
         

    public event OnStateChangeHandler OnModeOnLineChange;
    public GameModeOnLine gameModeOnLine { get; private set; }

    public GameStateNetwork gameStateNetwork { get; private set; }

    public PlayerRoupa roupa;
    public GameObject player;


    //online

    public String connectToIP = "127.0.0.1";
    public int connectPort = 25001;
    public String connectplayerName = "jonathan";
    public float setUpTimer = 10.0f;



    public void SetGameState(GameState gameStateq)
    {
        lastGameState = gameState;
        gameState = gameStateq;

        if (OnStateChange != null)
        {
            OnStateChange();
        }
    }

    public void SetGameModeOnLine(GameModeOnLine gameModeOnLineq)
    {
       
        gameModeOnLine = gameModeOnLineq;

        if (OnModeOnLineChange != null)
        {
            OnModeOnLineChange();
        }
    }


    public void SetGameStateNetwork(GameStateNetwork gameStateNetworkq)
    {
        gameStateNetwork = gameStateNetworkq;

        switch (gameStateNetwork)
        {
            case GameStateNetwork.local:
                menurede.SetActive(false);
                break;
            case GameStateNetwork.online:
                menu.SetActive(false);
                break;

        }
        Debug.Log(gameStateNetwork.ToString());
    }


    public void SetRoupa(PlayerRoupa roupaq)
    {
        roupa = roupaq;      

    }


    public void CurrentStage()
    {
        switch (Application.loadedLevelName)
        {
            case "Opening":
                Application.LoadLevel("Opening");
                break;
            case "Midle":
                Application.LoadLevel("Midle");
                break;
            case "Midle2":
                Application.LoadLevel("Midle2");
                break;
            case "Level1":
                Application.LoadLevel("Midle");

                break;
            case "Level2":
                Application.LoadLevel("Midle2");
                break;
            case "Level3":
            case "LevelM":
                Application.LoadLevel("GameOver");
                break;
            case "GameOver":
                Application.LoadLevel(lastGameStage);

                break;



        }

    }



        public void NextStage()
    {

        switch (Application.loadedLevelName)
        {
            case "Opening":
                Application.LoadLevel("Midle");
                break;
            case "Midle":
                Application.LoadLevel("Level1");
                break;
            case "Midle2":
                Application.LoadLevel("Level2");
                break;
            case "Level1":
                Application.LoadLevel("Midle2");

                break;
            case "Level2":
                Application.LoadLevel("ClearStage");
                break;
            case "ClearStage":
                Application.LoadLevel("GameOver");
                break;
            case "GameOver":
                Application.LoadLevel("Opening");

                break;



        }





    }




    void Awake()
    {
        DontDestroyOnLoad(gameObject);

    }


    void Start()
    {

        Application.LoadLevel("Opening");



    }






    void OnLevelWasLoaded(int level)
    {
       
             

        switch (Application.loadedLevelName)
        {
            case "Opening":
                lastGameStage = "Opening";
                menurede.active = true;             
                menu.active = true;
                SetGameState(GameState.opening);

                break;
            case "Midle":
                lastGameStage = "Opening";
                SetGameState(GameState.playing);

                break;
            case "Midle2":
                lastGameStage = "Opening";
                SetGameState(GameState.playing);

                break;
            case "Level1":
                lastGameStage = "Midle";
                SetGameState(GameState.playing);

                break;
            case "Level2":
                lastGameStage = "Midle2";
                SetGameState(GameState.playing);

                break;

            case "Level3":
            case "LevelM":

                SetGameState(GameState.playing);
                break;

            case "GameOver":
                SetGameState(GameState.gameover);
                break;

            case "ClearStage":

                SetGameState(GameState.clearStage);
                break;



        }

       
    }





}
