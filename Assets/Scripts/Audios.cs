

using UnityEngine;
using System.Collections;




public class Audios : MonoBehaviour
{


    Manager gerente;



    //BMG
    public AudioClip dies;
    public AudioClip pausas;
    public AudioClip clearStages;
    public AudioClip gameOvers;





    void Awake()
    {


        gerente = gameObject.GetComponent<Manager>();

        gerente.OnStateChange += HandleOnStateChange;





    }
    void Start()
    {



    }


    public void HandleOnStateChange()
    {

        switch (gerente.gameState)
        {

            case GameState.clear:

                StopAudio();
                GetComponent<AudioSource>().PlayOneShot(clearStages);
                break;

            case GameState.playing:
                if (gerente.lastGameState== GameState.pause)
                {
                   
                    MuteAudio(false);
                    GetComponent<AudioSource>().PlayOneShot(pausas);
                }

                break;

            case GameState.gameover:

                StopAudio();
                break;

            case GameState.restarting:

                //StopAudio();
                break;

            case GameState.opening:


                break;


            case GameState.pause:
                MuteAudio(true);
                GetComponent<AudioSource>().PlayOneShot(pausas);

                break;

            case GameState.dieing:
                StopAudio();
                GetComponent<AudioSource>().PlayOneShot(dies);
                break;

            case GameState.novafase:
                StopAudio();
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
                StopAudio();
                break;
            case "Midle2":
                StopAudio();
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

    void Update()
    {



    }




    public void MuteAudio(int i, bool mute)
    {

        gameObject.GetComponents<AudioSource>()[i].mute = mute;
    }

    public void MuteAudio(bool mute)
    {
        foreach (var audioSource in gameObject.GetComponents<AudioSource>())
        {
            if (audioSource.isPlaying)
                audioSource.mute = mute;
        }

        gerente.stageManager.GetComponent<AudioSource>().mute = mute;
    }



    public void PauseAudio()
    {

        foreach (var audioSource in this.gameObject.GetComponents<AudioSource>())
        {
            audioSource.Pause();
        }

    }

    void PauseAudio(int i)
    {
        this.gameObject.GetComponents<AudioSource>()[i].Pause();
    }

    void StopAudio(int i)
    {

        gameObject.GetComponents<AudioSource>()[i].Stop();

    }


    void StopAudio()
    {

        foreach (var audioSource in gameObject.GetComponents<AudioSource>())
        {
            audioSource.Stop();
        }

    }


    bool IsPlayingAudio(int i)
    {


        var audioSource = gameObject.GetComponents<AudioSource>();
        if (audioSource != null)
            return audioSource[i].isPlaying;
        else
            return false;

    }

    bool IsPlayingAudio()
    {


        foreach (var audioSource in gameObject.GetComponents<AudioSource>())
        {
            if (audioSource.isPlaying)
            {
                return true;
            }
        }

        return gerente.stageManager.GetComponent<AudioSource>().isPlaying;
       
    }

    public void PlayAudio()
    {
        StopAudio();
        // this.gameObject.GetComponent<AudioSource>()..PlayOneShot();
        gerente.stageManager.GetComponent<AudioSource>().Play();

    }
}


