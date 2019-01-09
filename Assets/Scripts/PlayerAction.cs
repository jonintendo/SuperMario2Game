using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

public class PlayerAction : MonoBehaviour
{



   
    public AudioClip cogumelos;
    public AudioClip moeda;
    public AudioClip pulo;
    public AudioClip puloalto;
    public AudioClip bump;


    public AudioClip estrela;
    public AudioClip estrelaguitar;
    public AudioClip estrelabatery;



    //objects
    public GameObject fireball;
    public GameObject fireballout;
    GameObject stageOut;
    GameObject stageIn;


    
    Manager gerente;

    //variables

    bool fireflower = false;
    bool cogumelo = false;
    bool star = false;

    public bool nochao = true;

    public int coins;
    public int lifes;



    private int pistolCD;

    int timedie;
    int timeclear;
    int timeinvencivel;

    float timer;
    float timerInvencivel;

    public GameObject placarPlayer;
    GameObject placar;
    GUIText coinst;
    GUIText lifet;

    public event OnStateChangeHandler OnStateChange;

    void Awake()
    {

        DontDestroyOnLoad(gameObject);


        placar = (GameObject)Instantiate(placarPlayer, placarPlayer.transform.position, placarPlayer.transform.rotation);
        DontDestroyOnLoad(placar);

        coinst = placar.GetComponentsInChildren<GUIText>()[1];
        lifet = placar.GetComponentsInChildren<GUIText>()[0];



       // menu = GameObject.FindGameObjectWithTag("Menu");
        gerente = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
        gerente.OnStateChange += HandleOnStateChange2;





    }

    void OnDestroy()
    {
        Destroy(placar);
    }

    public void Start()
    {

        timer = 0;

        fireflower = true;
        cogumelo = false;
        coins = 40;
        pistolCD = 60;

        timedie = 4;
        timeclear = 3;
        timeinvencivel = 70;

        nochao = true;
        lifes = 0;


    }

    public void restart()
    {
        stageOut = GameObject.FindGameObjectWithTag("SaidaFase");
        stageIn = GameObject.FindGameObjectWithTag("EntradaFase");
        //transform.position = new Vector3(-30.26363F, 48.491678F, 62.81319F);
        transform.position = stageIn.transform.position + 5 * Vector3.up;
        star = false;
        pistolCD = 60;
        GetComponent<CharacterMotor>().jumping.extraHeight = 1;
        desativaoControles(false);

    }

    void OnLevelWasLoaded(int level)
    {
        switch (Application.loadedLevelName)
        {
            case "Level1":
            case "Level2":              
                restart();
                break;
        }
    }

    void HandleOnStateChange2()
    {
        timer = 0;
        Debug.Log("aqqqqjghwjqgwhjqgiwgqiuwgiqgwiuqgwiugqiuwghqiuwg       " + gerente.gameState);
        switch (gerente.gameState)
        {
            case GameState.pause:
                desativaoControles(true);
                break;

            case GameState.playing:
                desativaoControles(false);
                break;

            case GameState.gameover:

                break;
            case GameState.opening:

                break;

            case GameState.restarting:
                restart();
                gerente.SetGameState(GameState.playing);
                break;

            case GameState.clear:
                desativaoControles(true);
                restart();

               
                break;


        }


    }

    void Update()
    {


        if (gerente.gameState == GameState.playing)
        {
            timer += Time.deltaTime;

            coinst.text = coins.ToString();
            lifet.text = lifes.ToString();

            if (star)
            {
                timerInvencivel += Time.deltaTime;

                if (timerInvencivel > timeinvencivel)
                {

                    star = false;
                    transform.FindChild("Corpo").tag = "Corpo";

                }

            }



            if (Input.GetMouseButton(0) && fireflower && pistolCD == 0)
            { //Tiro


                GameObject ee = Instantiate(fireball, fireballout.transform.position, fireballout.transform.rotation);
                ee.GetComponent<FireBall>().Player = gameObject;
               


                pistolCD = 30;
            }

            if (pistolCD > 0)
            {
                pistolCD--;
            }

            if (this.transform.position.y < 9)
            {
                nochao = true;
            }

            if (Input.GetKeyDown("space"))
            {



                if (nochao)
                {


                    if (cogumelo || fireflower)
                    {
                        GetComponent<AudioSource>().PlayOneShot(puloalto);
                    }
                    else
                    {
                        GetComponent<AudioSource>().PlayOneShot(pulo);
                    }

                }

                nochao = false;
            }

        }

        if (gerente.gameState == GameState.dieing)
        {
            timer += Time.deltaTime;


            if (timer < timedie / 2)
            {
                this.transform.position += Vector3.up;
            }
            else if (timer > timedie / 2 && timer < timedie)
            {
                this.transform.position += Vector3.down;

            }
            else if (timer > timedie)
            {

                if (lifes == 0)
                {

                    //gerente.SetGameState(GameState.gameover);
                    Application.LoadLevel("GameOver");

                }
                else
                {
                    lifes -= 1;
                    gerente.SetGameState(GameState.restarting);
                }
                //chama o game over
            }


        }

        if (gerente.gameState == GameState.clear)
        {

            timer += Time.deltaTime;

            if (timer < timeclear)
            {
                if (!nochao)
                {
                    transform.position += Vector3.down;
                }
                else
                {
                    float damping = 0.5f;
                    Quaternion rot = Quaternion.LookRotation(stageOut.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);
                    this.transform.position += this.transform.forward / 70;
                }

            }
            else if (timer > timeclear)
            {

                gerente.SetGameState(GameState.novafase);


            }


        }








    }

    void desativaoControles(bool desativa)
    {
        if (this != null)
        {
            var script = (MonoBehaviour)gameObject.GetComponent("CharacterMotor");
            script.enabled = !desativa;

            var script3 = (MonoBehaviour)gameObject.GetComponent("MouseLook");
            script3.enabled = !desativa;

            var script1 = (MonoBehaviour)gameObject.GetComponent("FPSInputController");
            script1.enabled = !desativa;

            foreach (var scriptj in gameObject.GetComponentsInChildren<MonoBehaviour>())
            {


                if (scriptj.name == "Main Camera")
                {
                    var script2 = (MonoBehaviour)scriptj.GetComponent("MouseLook");
                    script2.enabled = !desativa;
                }

            }
        }

    }

    void Invencivel(int TimeInvencivel)
    {

        timerInvencivel = 0;
        timeinvencivel = TimeInvencivel;
        star = true;
        transform.FindChild("Corpo").tag = "estrela";
    }




    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.gameObject.tag == "enemy")
        {

            if (star)
            {


                coins += 100;


            }
            else if (fireflower)
            {
                GetComponent<AudioSource>().PlayOneShot(bump);
                fireflower = false;
                cogumelo = true;
                Invencivel(1);

            }
            else if (cogumelo)
            {
                GetComponent<AudioSource>().PlayOneShot(bump);
                cogumelo = false;
                Invencivel(1);
                GetComponent<CharacterMotor>().jumping.extraHeight = 1;
            }
            else
            {

                gerente.SetGameState(GameState.dieing);
                desativaoControles(true);
            }


        }


        if (hit.gameObject.name == "Water")
        {
            gerente.SetGameState(GameState.dieing);


            desativaoControles(true);

        }

        if (hit.gameObject.tag == "bandeira")
        {
            if (gerente.gameState != GameState.clear)
                gerente.SetGameState(GameState.clear);


        }

        if (hit.gameObject.tag == "chao")
        {

            nochao = true;
            // Debug.Log("colidiu com o chao");

        }



        if (hit.gameObject.tag == "Cogumelo")
        {
            GetComponent<CharacterMotor>().jumping.extraHeight = 10;
            GetComponent<AudioSource>().PlayOneShot(cogumelos);
            cogumelo = true;
            Destroy(hit.gameObject);

        }


        if (hit.gameObject.tag == "Flor")
        {
            GetComponent<CharacterMotor>().jumping.extraHeight = 10;
            GetComponent<AudioSource>().PlayOneShot(cogumelos);
            cogumelo = true;
            fireflower = true;
            Destroy(hit.gameObject);
        }


        if (hit.gameObject.name == "Moeda")
        {
            GetComponent<AudioSource>().PlayOneShot(moeda);
            coins += 100;
            Destroy(hit.gameObject);

        }

        if (hit.gameObject.tag == "estrela")
        {

            // MenuGUI menugui = menu.GetComponent<MenuGUI>();

            // Audios audios = menu.GetComponent<Audios>();

            // audios.MuteAudio(menugui.audioChosen, true);

            //  if (menugui.audioChosen == 0)
            GetComponent<AudioSource>().PlayOneShot(estrela);
            // else
            //  if (menugui.audioChosen == 2)
            //     GetComponent<AudioSource>().PlayOneShot(estrelaguitar);
            // else
            //  if (menugui.audioChosen == 1)
            //     GetComponent<AudioSource>().PlayOneShot(estrelabatery);


            Invencivel(11);
            star = true;
            Destroy(hit.gameObject);

        }





    }
}
