

using UnityEngine;
using System.Collections;




public class Audios : MonoBehaviour
{

    public GameObject menu;
    public Manager gerente;
    public MenuGUI menugui;



    public AudioClip dies;
    public AudioClip pausas;
    public AudioClip clearStages;
    public AudioClip gameOvers;




    void Awake()
    {
        menu = GameObject.FindGameObjectWithTag("Menu");

        gerente = menu.GetComponent<Manager>();

        gerente.OnStateChange += HandleOnStateChange;

        menugui = menu.GetComponent<MenuGUI>();



    }
    void Start()
    {

        // Player1 = GameObject.Find("Mario");
        // playeraction = Player1.GetComponent<PlayerAction>();

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
                if (IsPlayingAudio())
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
                PlayAudio(0);
                break;
            case "Midle":
                StopAudio();
                break;
            case "Midle2":
                StopAudio();
                break;
            case "Level1":
                PlayAudio(1);

                break;
            case "Level2":
                PlayAudio(4);
                break;
            case "Level3":
            case "LevelM":
                PlayAudio(5);
                break;
            case "ClearStage":
                PlayAudio(11);
                break;
            case "GameOver":
                PlayAudio(10);

                break;



        }

    }

    void Update()
    {



    }




    public void MuteAudio(int i, bool mute)
    {

        this.gameObject.GetComponents<AudioSource>()[i].mute = mute;
    }

    public void MuteAudio(bool mute)
    {
        foreach (var audioSource in this.gameObject.GetComponents<AudioSource>())
        {
            if (audioSource.isPlaying)
                audioSource.mute = mute;
        }


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

        this.gameObject.GetComponents<AudioSource>()[i].Stop();

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
        return false;
    }

    public void PlayAudio(int i)
    {


        StopAudio();


        this.gameObject.GetComponents<AudioSource>()[i].Play();



    }
}


