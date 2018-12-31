using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

public class PlayerAction : MonoBehaviour
{



    public AudioClip fireballs;
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


    GameObject menu;
    Manager gerente;
    //variables

    public bool fireflower = false;
    public bool cogumelo = false;
    public bool invencivel = false;
    public bool star = false;

    public bool nochao = true;

    public int coins = 0;
    public int lifes = 3;



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
        

        GameObject[] others = GameObject.FindGameObjectsWithTag(this.gameObject.tag);
        if (others.Length > 1)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {

            placar = (GameObject)Instantiate(placarPlayer, placarPlayer.transform.position, placarPlayer.transform.rotation);

            DontDestroyOnLoad(placar);

            coinst = placar.GetComponentsInChildren<GUIText>()[0];

            lifet = placar.GetComponentsInChildren<GUIText>()[1];
          
            
            menu = GameObject.FindGameObjectWithTag("Menu");
          
            if (menu != null)
            {
                gerente = menu.GetComponent<Manager>();

                gerente.NewPlayer(this.gameObject);

                gerente.OnStateChange += HandleOnStateChange;
            }

        }


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
        lifes = 3;

        //gerente.SetGameState(GameState.playing);
        

    }

    public void restart()
    {
        transform.position = new Vector3(-30.26363F, 48.491678F, 62.81319F);
        invencivel = false;
        pistolCD = 60;
        GetComponent<CharacterMotor>().jumping.extraHeight = 1;
        desativaoControles(false);

    }

    public void HandleOnStateChange()
    {
        timer = 0;

        if (gerente.gameState == GameState.pause)
        {

            desativaoControles(true);

        }

        if (gerente.gameState == GameState.playing)
        {
            desativaoControles(false);
        }

        //if (gerente.gameState == GameState.gameover || gerente.gameState == GameState.opening)
        //{
        //   Destroy(this.gameObject);
            
        //}
        if (gerente.gameState == GameState.restarting)
        {
           restart();
           gerente.SetGameState(GameState.playing);
        }

        if (gerente.gameState == GameState.clear)
        {
            stageOut = GameObject.FindGameObjectWithTag("SaidaFase");
          
        }
    }

    void Update()
    {


        if (gerente.gameState == GameState.playing)
        {
            timer += Time.deltaTime;

            coinst.text = coins.ToString();
            lifet.text = lifes.ToString();

            if (invencivel)
            {
                timerInvencivel += Time.deltaTime;

                if (timerInvencivel > timeinvencivel)
                {
                    invencivel = false;
                    star = false;

                    GetComponent<AudioSource>().Stop();

                    MenuGUI menugui = menu.GetComponent<MenuGUI>();
                    Audios audios = menu.GetComponent<Audios>();

                    audios.MuteAudio(menugui.audioChosen, false);

                   
                }

            }



            if (Input.GetMouseButton(0) && fireflower && pistolCD == 0)
            { //Tiro

                FireBall playeraction = fireball.GetComponent<FireBall>();
                playeraction.myPlayer = this.gameObject;
                GameObject ee = (GameObject)Instantiate(fireball, fireballout.transform.position, fireballout.transform.rotation);
                ee.name = this.tag;
                //ee.tag=this.name;
                GetComponent<AudioSource>().PlayOneShot(fireballs);
                //PlayerAnimation.pistolshoting = true;
                //attribs.ammo9mm--;

                //    Quaternion g = Quaternion.Euler(Mathf.Cos(i), i, 0);
                //    var randomRotation = Quaternion.Euler(0, Random.Range(120, 60), 0);
                //    Instantiate(minibullet, minibulletout.transform.position, this.camera.transform.rotation * Quaternion.Euler(Mathf.Cos(i), Random.Range(-5, 5), 0));
                //    Instantiate(minibullet, minibulletout.transform.position, this.camera.transform.rotation * Quaternion.Euler(-Mathf.Cos(i), Random.Range(-5, 5), 0));


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
                        GetComponent<AudioSource>().PlayOneShot(puloalto);
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

                    gerente.SetGameState(GameState.gameover);
                    //Application.LoadLevel("GameOver");
                }
                else
                {
                    lifes -= 1;

                    restart();
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
                   this.transform.position += Vector3.down;
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

               // Application.LoadLevel("Midle2");
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
        invencivel = true;
    }


  

    void OnControllerColliderHit(ControllerColliderHit hit)
    {


        if (hit.gameObject.tag == "enemy" && star)
        {
            Debug.Log("matou com estrela");

          coins += 100;
          Destroy(hit.gameObject);
           
        }
        else

        if (hit.gameObject.tag == "enemy" && !invencivel)
        {

            //if (!nochao && Mathf.Abs(Vector3.Distance(hit.gameObject.transform.position, transform.FindChild("Pes").transform.position)) < 1.5f)
            //{
            //    audio.PlayOneShot(smashenemy);
            //    coins += 100;
            //    Destroy(hit.gameObject);


            //}
            //else 


            if (fireflower)
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
            //    audio.PlayOneShot(clearStages);
            // clearStage = true;
            gerente.SetGameState(GameState.clear);
            desativaoControles(true);

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

            MenuGUI menugui = menu.GetComponent<MenuGUI>();

              Audios audios = menu.GetComponent<Audios>();

              audios.MuteAudio(menugui.audioChosen, true);

            if (menugui.audioChosen == 0)
                GetComponent<AudioSource>().PlayOneShot(estrela);
            else
                if (menugui.audioChosen == 2)
                    GetComponent<AudioSource>().PlayOneShot(estrelaguitar);
                else
                    if (menugui.audioChosen == 1)
                        GetComponent<AudioSource>().PlayOneShot(estrelabatery);
            Invencivel(11);
            star = true;
            Destroy(hit.gameObject);

        }

        //if(hit.gameObject.name == "Item")
        //{


        //    if(hit.gameObject.tag=="moeda")
        //        audio.PlayOneShot(moeda);
        //        else
        //    audio.PlayOneShot(item);

        //Texture mat3 = Resources.Load<Texture>("Assets/Materials/BlocoUsado");
        //hit.gameObject.GetComponent<MeshRenderer> ().materials [0].mainTexture = mat3;
        //hit.gameObject.renderer.material=(Material)Resources.Load("Assets/Materials/BlocoUsado");




        //hit.gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.cyan;

        //Destroy(hit.gameObject);

        //}



    }
}
