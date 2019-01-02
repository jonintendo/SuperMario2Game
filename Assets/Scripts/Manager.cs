using UnityEngine;
using System.Collections;



public enum GameState { gameover, clear, playing, opening, restarting, pause, dieing, novafase, online, clearStage }
public delegate void OnStateChangeHandler();

public class Manager : MonoBehaviour
{

    //GUIText coinst;
    //GUIText lifet;

    public GameObject Player1;
    GameObject player;

    public event OnStateChangeHandler OnStateChange;
    public GameState gameState { get; private set; }

    string lastGameStage;


    public void SetGameState(GameState gameState)
    {
        lastGameStage = Application.loadedLevelName;
        this.gameState = gameState;

        if (OnStateChange != null)
        {
            OnStateChange();
        }
    }

    public void NewPlayer(GameObject Player)
    {
        if (Player1 == null)
        {
            player = Player;

        }
        else if (Player1.tag != Player.tag && Player.tag == "player1")
        {
            player = Player;

        }



    }


    public void NewPlayer()
    {
        GameObject[] others = GameObject.FindGameObjectsWithTag("player1");
        if (others.Length == 0)
        {
            player = Instantiate(Player1);
        }

    }

    public void RemovePlayer(GameObject Player)
    {
        Destroy(Player);

    }

    public void RemovePlayer()
    {
        Destroy(player);


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



    void Start()
    {

        Application.LoadLevel("Opening");
        OnStateChange += HandleOnStateChange;


    }

    void Update()
    {



        if (Input.GetKeyDown(KeyCode.Escape))
        {

            switch (gameState)
            {
                case GameState.playing:
                    SetGameState(GameState.pause);
                    break;
                case GameState.pause:
                    SetGameState(GameState.playing);
                    break;
            }

        }

    }

    public void HandleOnStateChange()
    {

        switch (gameState)
        {
            case GameState.novafase:
                NextStage();
                break;


            case GameState.gameover:
                //RemovePlayer();
                break;

            case GameState.playing:
                NewPlayer();
                break;

            case GameState.opening:
                RemovePlayer();
                Debug.Log("Handling state change to: " + gameState);
                break;

            case GameState.clearStage:
                break;
        }

    }


    void OnLevelWasLoaded(int level)
    {
        switch (Application.loadedLevelName)
        {
            case "Opening":

                SetGameState(GameState.opening);

                break;
            case "Midle":

                SetGameState(GameState.playing);

                break;
            case "Midle2":

                SetGameState(GameState.playing);

                break;
            case "Level1":

                SetGameState(GameState.playing);

                break;
            case "Level2":

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
