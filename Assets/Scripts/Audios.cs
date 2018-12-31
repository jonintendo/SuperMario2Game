

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


      //int audioChosen;
      //int stage;

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

     
        if (gerente.gameState == GameState.gameover)
        {

            StopAudio(menugui.audioChosen);
        }


        if (gerente.gameState == GameState.clear)
        {


            StopAudio(menugui.audioChosen);
        }

        if (gerente.gameState == GameState.dieing)
        {

            StopAudio(menugui.audioChosen);
            GetComponent<AudioSource>().PlayOneShot(dies);

          
        }

        if (gerente.gameState == GameState.restarting)
        {


            StopAudio(menugui.audioChosen);
        }

        if (gerente.gameState == GameState.novafase)
        {


            StopAudio(menugui.audioChosen);
          
        }


        if (gerente.gameState == GameState.opening)
        {


            StopAudio(0);
            ChooseAudio(0);
           
        }

        if (gerente.gameState == GameState.playing)
        {

          
            ChooseAudio(menugui.audioChosen);
        }





        if (gerente.gameState == GameState.pause)
        {

           


        }

        if (gerente.gameState == GameState.online)
        {
            Debug.Log("aqqqqjghwjqgwhjqgiwgqiuwgiqgwiuqgwiugqiuwghqiuwg       " + gerente.gameState);
            ChooseAudio(menugui.audioChosen);
        }



    }


    void Update()
    {

        if (gerente.gameState == GameState.playing)
        {
          

            if (Input.GetKeyDown(KeyCode.Escape))
            {

               
                MuteAudio(menugui.audioChosen, true);
                GetComponent<AudioSource>().PlayOneShot(pausas);

              

              
            }
        }
        else if (gerente.gameState == GameState.pause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                MuteAudio(menugui.audioChosen, false);
                GetComponent<AudioSource>().PlayOneShot(pausas);
               
            }
        }


        

    }




   public  void MuteAudio(int i, bool mute)
    {
        i = i + 3 * (menugui.stage - 1);
        Debug.Log(i);
        this.gameObject.GetComponents<AudioSource>()[i].mute = mute;
    }


    void PauseAudio(int i)
    {

        i = i + 3 * (menugui.stage - 1);
        this.gameObject.GetComponents<AudioSource>()[i].Pause();
    }

    void StopAudio(int i)
    {

        if (i == 0)
        {
            GetComponent<AudioSource>().Stop();
        }
        else
        {
            i = i + 3 * (menugui.stage - 1);
            this.gameObject.GetComponents<AudioSource>()[i].Stop();
        }
    }

    bool IsPlayingAudio(int i)
    {
        i = i + 3 * (menugui.stage - 1);

        return this.gameObject.GetComponents<AudioSource>()[i].isPlaying;
    }

   public  void ChooseAudio(int i)
    {

       

        //if (!this.gameObject.GetComponents<AudioSource>()[i].isPlaying)
        {
           // audioChosen = i;
           
            if(i!=0)
                i = i + 3 * (menugui.stage - 1);

            foreach (AudioSource tt in this.gameObject.GetComponents<AudioSource>())
            {
                tt.Stop();
            }

           
            this.gameObject.GetComponents<AudioSource>()[i].Play();

        }

    }
}


