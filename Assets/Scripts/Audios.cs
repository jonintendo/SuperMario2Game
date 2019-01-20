

using UnityEngine;
using System.Collections;




public class Audios : MonoBehaviour
{


   
    //BMG
    public AudioClip dies;
    public AudioClip pausas;
    public AudioClip clearStages;
    public AudioClip gameOvers;

    Manager gerente;



    void Awake()
    {
        gerente = gameObject.GetComponentInParent<Manager>();
        gerente.OnStateChange += HandleOnStateChange4;

    }
    void Start()
    {



    }


    public void HandleOnStateChange4()
    {

        switch (gerente.gameState)
        {

            case GameState.clear:

               // StopAudio();
                GetComponent<AudioSource>().PlayOneShot(clearStages);
                break;

            case GameState.playing:
                if (gerente.lastGameState == GameState.pause)
                {
                   
                    //MuteAudio(false);
                    GetComponent<AudioSource>().PlayOneShot(pausas);
                }

                break;

            case GameState.gameover:

                //StopAudio();
                break;

            case GameState.restarting:

                //StopAudio();
                break;

            case GameState.opening:


                break;


            case GameState.pause:
                //MuteAudio(true);
                GetComponent<AudioSource>().PlayOneShot(pausas);

                break;

            case GameState.dieing:
                //StopAudio();
                GetComponent<AudioSource>().PlayOneShot(dies);
                break;

            case GameState.novafase:
                //StopAudio();
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
                // PlayAudio(0);
                break;
            case "Midle":
                //StopAudio();
                break;
            case "Midle2":
                //StopAudio();
                break;
            case "Level1":
                //PlayAudio(1);

                break;
            case "Level2":
                // PlayAudio(4);
                break;
            case "Level3":
            case "LevelM":
                // PlayAudio(5);
                break;
            case "ClearStage":
                //PlayAudio(11);
                break;
            case "GameOver":
                //PlayAudio(10);

                break;



        }

    }

   
   
}


