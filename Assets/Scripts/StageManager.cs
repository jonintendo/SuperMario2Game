using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{


    Manager gerente;
    // Use this for initialization
    void Start()
    {

        gerente = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
        gerente.OnStateChange += HandleOnStateChange3;
       
        if (gerente.player != null)
        {
            GetComponent<AudioListener>().enabled = false;
            GetComponent<Camera>().enabled = false;
        }
        else
        {
            GetComponent<AudioListener>().enabled = true;
            GetComponent<Camera>().enabled = true;
        }

    }





    public void HandleOnStateChange3()
    {

        switch (gerente.gameState)
        {

            case GameState.clear:

                StopAudio();

                break;

            case GameState.playing:
                // if (gerente.lastGameState == GameState.pause)
                {

                    MuteAudio(false);

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


                break;

            case GameState.dieing:
                StopAudio();

                break;

            case GameState.novafase:
                StopAudio();
                break;

            case GameState.clearStage:

                break;

        }




    }

    public void MuteAudio(bool mute)
    {
        if (this != null)
            GetComponent<AudioSource>().mute = mute;
    }



    public void PauseAudio()
    {
        if (this != null)
            GetComponent<AudioSource>().Pause();
    }




    void StopAudio()
    {
        if (this != null)
            GetComponent<AudioSource>().Stop();

    }



    bool IsPlayingAudio()
    {

        if (this != null)
            return GetComponent<AudioSource>().isPlaying;
        else return false;

    }

    public void PlayAudio()
    {
        StopAudio();
        if (this != null)
            GetComponent<AudioSource>().Play();

    }

    private void OnDestroy()
    {
        Debug.Log(gameObject.scene.name);
    }

}
