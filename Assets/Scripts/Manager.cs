using UnityEngine;
using System.Collections;



public enum GameState { gameover, clear, playing, opening, restarting, pause, dieing, novafase, online, clearStage }
public enum GameStateNetwork { local, online }
public delegate void OnStateChangeHandler();

public class Manager : MonoBehaviour
{

    //GUIText coinst;
    //GUIText lifet;

    public GameObject Player1;
    public GameObject Enemy1;
    public GameObject Enemy2;
    GameObject player;
    public GameObject stageManager;


    public event OnStateChangeHandler OnStateChange;
    public GameState gameState { get; private set; }

    string lastGameStage;
    public GameState lastGameState;
 


    public void SetGameState(GameState gameStateq)
    {
        lastGameState = gameState;
        gameState = gameStateq;

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
        player = null;

    }

    public void NewEnemies()
    {
        var entradasInimigos = GameObject.FindGameObjectsWithTag("EntradaFaseInimigo");

        foreach (var entradaInimigos in entradasInimigos)
        {
            NewEnemy(entradaInimigos.transform.position);
        }

    }



    public void NewEnemy(Vector3 position)
    {


        var enemy = Instantiate(Enemy1, position, Quaternion.identity);


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
        stageManager = GameObject.FindGameObjectWithTag("StageManager");
       

        switch (Application.loadedLevelName)
        {
            case "Opening":
                lastGameStage = "Opening";
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

        if (player != null)
        {
            stageManager.GetComponent<AudioListener>().enabled = false;
            stageManager.GetComponent<Camera>().enabled = false;
        }
        else
        {
            stageManager.GetComponent<AudioListener>().enabled = true;
            stageManager.GetComponent<Camera>().enabled = true;
        }
    }





}
