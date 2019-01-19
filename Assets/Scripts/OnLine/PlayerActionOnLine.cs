

using UnityEngine;
using System.Collections;
using System;
//using System.Collections.Generic;

public class PlayerActionOnLine : MonoBehaviour
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



    Manager gerente;



    //variables




    public NetworkPlayer networkPlayer;
    public string Name;
    public string Tag;

    public bool fireflower = false;
    public bool cogumelo = false;
    public bool invencivel = false;
    public bool flag = false;
    public bool star = false;


    public bool nochao = true;

    bool Jump = false;
    bool Walk = false;
    bool FireBall = false;


    public Texture marioFogo;
    public Texture marioMimic;
    public Texture mario;
    public Texture marioLuigi;
    public Texture marioWario;

    public Material mat;



    // public PlayerRoupa roupa;



    public Animation ff;
    public CharacterController tt;
    float speedV;
    float speedH;



    int coins = 0;
    int lifes = 3;



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


    public void SetRoupa(PlayerRoupa roupa)
    {
        switch (roupa)
        {
            case PlayerRoupa.luigi:
                mat.mainTexture = marioLuigi;
                break;

            case PlayerRoupa.mario:
                mat.mainTexture = mario;
                break;
            case PlayerRoupa.fogo:
                mat.mainTexture = marioFogo;
                break;
            case PlayerRoupa.mimico:
                mat.mainTexture = marioMimic;
                break;
            case PlayerRoupa.wario:
                mat.mainTexture = marioWario;
                break;
        }


    }

    public void SetPlacar(int moedas, int vidas, bool? flags)
    {
        this.coins += moedas;

        lifes = vidas;


        bool ffg = false;

        if (flags == null)
            ffg = flag;
        else
        {
            ffg = bool.Parse(flags.ToString());
            flag = bool.Parse(flags.ToString()); ;
        }



        if (coinst != null)
        {
            this.coinst.text = coins.ToString();
            this.lifet.text = lifes.ToString();

            GetComponent<NetworkView>().RPC("ReceiveStatus", RPCMode.All, this.coins, this.lifes, flags, gerente.connectplayerName);//so mandando pra todos pq se nao o server nao recebe quando eh ele que manda, era so pra ir proserver


        }
    }

    public int[] GetPlacar()
    {

        var placar = new int[2];

        placar[0] = this.coins;
        placar[1] = this.lifes;

        return placar;
    }


    [RPC]
    bool TellAllOurName(String name, string tagq, PlayerRoupa roupa, NetworkMessageInfo info)
    {


        if (info.sender == GetComponent<NetworkView>().owner && !GetComponent<NetworkView>().isMine)
        {
            // this.name = name;
            tag = tagq;
            
            SetRoupa(roupa);
            TextMesh nametext = this.gameObject.GetComponentsInChildren<TextMesh>()[0];
            nametext.text = this.tag;


            Debug.Log(roupa.ToString());
        }


        return true;
    }


    [RPC]
    bool TellAllOurPlacar(int lifeN, int coinsN, NetworkMessageInfo info)
    {



        if (info.sender == GetComponent<NetworkView>().owner && !GetComponent<NetworkView>().isMine)
        {

            Vector3 placarPosition = new Vector3(0, 0, 0);
            if (this.tag == "player1")
                placarPosition = new Vector3(98.28f, 23.49591f, 0);
            else if (this.tag == "player2")
                placarPosition = new Vector3(98.52f, 23.49591f, 0);
            else if (this.tag == "player3")
                placarPosition = new Vector3(98.8f, 23.49591f, 0);
            else if (this.tag == "player4")
                placarPosition = new Vector3(99f, 23.49591f, 0);


            placar.transform.position = placarPosition;




            this.coins = coinsN;
            this.lifes = lifeN;


            this.coinst.text = coins.ToString();
            this.lifet.text = lifes.ToString();

        }

        return true;
    }





    void OnNetworkInstantiate(NetworkMessageInfo info)
    {


        //Vector3 placarPosition = new Vector3(0, 0, 0);
        //if (this.tag == "player1")
        //    placarPosition = new Vector3(98.28f, 23.49591f, 0);
        //else if (this.tag == "player2")
        //    placarPosition = new Vector3(98.52f, 23.49591f, 0);
        //else if (this.tag == "player3")
        //    placarPosition = new Vector3(98.8f, 23.49591f, 0);
        //else if (this.tag == "player4")
        //    placarPosition = new Vector3(99f, 23.49591f, 0);

        //placar.name = this.tag;
        //placar.transform.position = placarPosition;

        //if (networkView.isMine)
        //{
        //    Debug.Log(networkView.viewID + " spawned");

        //    // networkView.RPC("TellAllOurName", info.sender, this.name, this.tag);
        //}


    }



    void Awake()
    {



        if (!GetComponent<NetworkView>().isMine)
        {
            desativaoControles(true);
            desativaCameraScripts();

            Debug.Log("Nao eh meu");

        }
        else
        {


            gerente = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();

            gerente.OnStateChange += HandleOnStateChange;


            TextMesh nametext = this.gameObject.GetComponentsInChildren<TextMesh>()[0];
            nametext.text = this.tag;

            OnlineTagsPlacar();



        }



    }


    public void OnlineTagsPlacar()
    {

        Vector3 placarPosition = new Vector3(0, 0, 0);


        switch(tag)
        {
            case "player1":
                placarPosition = new Vector3(98.28f, 23.49591f, 0);
                break;
            case "player2":
                placarPosition = new Vector3(98.52f, 23.49591f, 0);
                break;
            case "player3":
                placarPosition = new Vector3(98.8f, 23.49591f, 0);
                break;
            case "player4":
                placarPosition = new Vector3(99f, 23.49591f, 0);
                break;

        }
             
            

        placar = (GameObject)Network.Instantiate(placarPlayer, placarPosition, placarPlayer.transform.rotation, 0);

        coinst = placar.GetComponentsInChildren<GUIText>()[1];

        lifet = placar.GetComponentsInChildren<GUIText>()[0];


        coinst.text = coins.ToString();
        lifet.text = lifes.ToString();

       // SetRoupa(gerente.roupa);

     
        GetComponent<NetworkView>().RPC("TellAllOurName", RPCMode.All, "", tag, gerente.roupa);

    }

    void OnDestroy()
    {


        if (GetComponent<NetworkView>().isMine)
        {
            Network.Destroy(placar);

        }

        // Destroy(placar);
    }

    public void Start()
    {

        ff = GetComponentsInChildren<Animation>()[0];
        tt = GetComponentInParent<CharacterController>();

        timer = 0;

        fireflower = true;
        cogumelo = false;
        coins = 0;
        pistolCD = 60;

        timedie = 4;
        timeclear = 3;
        timeinvencivel = 70;

        nochao = true;
        lifes = 3;

        if (gerente.gameModeOnLine == GameModeOnLine.takeflag)
        {
            transform.position = new Vector3(-30.26363F + UnityEngine.Random.Range(1, 10), 48.491678F, 62.81319F + UnityEngine.Random.Range(1, 10));
        }
        else
        {

            if (Tag == "player2")
                transform.position = new Vector3(-30.26363F, 48.491678F, 62.81319F);
            else
                if (Tag == "player3")
                transform.position = new Vector3(-22.97078f, 9.795295f, -99.43913f);
            else
                    if (Tag == "player4")
                transform.position = new Vector3(104.9768f, 9.795295f, -52.62424f);
            else
                transform.position = new Vector3(57.36654f, 9.795295f, 64.04122f);


        }


    }

    public void restart()
    {





        //invencivel = false;
        //pistolCD = 60;
        //GetComponent<CharacterMotor>().jumping.extraHeight = 1;
        //desativaoControles(false);

        Start();

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
        }

        if (gerente.gameState == GameState.clear)
        {
            //stageOut = GameObject.FindGameObjectWithTag("SaidaFase");
            SetPlacar(0, lifes, flag);
        }

        if (gerente.gameState == GameState.dieing)
        {
            restart();

        }
    }




    void AnimationControl()
    {
        if (FireBall)
        {
            ff.Play("Armature|MyFireBall", PlayMode.StopAll);
            FireBall = false;
        }


        var VV = new Vector3(tt.velocity.x, 0, tt.velocity.z);
        speedV = VV.magnitude;

        var VH = new Vector3(0, tt.velocity.y, 0);
        speedH = VH.magnitude;




        if (speedH > 0.5f && !ff.IsPlaying("Armature|MyFireBall"))
        {

            ff.Play("Armature|MyJump", PlayMode.StopAll);
        }
        else

            if (speedV > 0.5f && !ff.IsPlaying("Armature|MyFireBall"))
        {
            ff.Play("Armature|MyWalk", PlayMode.StopAll);
        }



    }

    void Update()
    {

        //Vector3 placarPosition = new Vector3(0, 0, 0);
        //if (this.tag == "player1")
        //    placarPosition = new Vector3(98.28f, 23.49591f, 0);
        //else if (this.tag == "player2")
        //    placarPosition = new Vector3(98.52f, 23.49591f, 0);
        //else if (this.tag == "player3")
        //    placarPosition = new Vector3(98.8f, 23.49591f, 0);
        //else if (this.tag == "player4")
        //    placarPosition = new Vector3(99f, 23.49591f, 0);


        //placar.transform.position = placarPosition;

        //this.coinst.text = coins.ToString();
        //this.lifet.text = lifes.ToString();
        //networkView.RPC("TellAllOurPlacar", RPCMode.All, this.lifes, this.coins);



        if (GetComponent<NetworkView>().isMine)
        {
            // networkView.RPC("TellAllOurName", RPCMode.All, "", this.tag, Roupa);

            AnimationControl();

            if (gerente.gameModeOnLine == GameModeOnLine.takeflag)
            {
                //if (gerente.gameState == GameState.playing)
                {
                    timer += Time.deltaTime;

                    //coinst.text = coins.ToString();
                    //lifet.text = lifes.ToString();

                    if (invencivel)
                    {
                        timerInvencivel += Time.deltaTime;

                        if (timerInvencivel > timeinvencivel)
                        {
                            invencivel = false;
                            star = false;

                            GetComponent<AudioSource>().Stop();


                        }

                    }



                    if (Input.GetMouseButton(0) && fireflower && pistolCD == 0)
                    { //Tiro
                        FireBall = true;

                        // fireball.name = this.tag;
                        FireBallOnLine playeraction = fireball.GetComponent<FireBallOnLine>();
                        playeraction.myPlayer = this.gameObject;

                        Network.Instantiate(fireball, fireballout.transform.position, fireballout.transform.rotation, 0);

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
            if (gerente.gameModeOnLine == GameModeOnLine.timeatack && gerente.gameState==  GameState.playing)
            {

                // if (gerente.gameState == GameState.playing)
                {
                    timer += Time.deltaTime;

                    //coinst.text = coins.ToString();
                    //lifet.text = lifes.ToString();

                    if (invencivel)
                    {
                        timerInvencivel += Time.deltaTime;

                        if (timerInvencivel > timeinvencivel)
                        {
                            invencivel = false;
                        }

                    }



                    if (Input.GetMouseButton(0) && fireflower && pistolCD == 0)
                    { //Tiro

                        // fireball.name = this.tag;
                        FireBallOnLine playeraction = fireball.GetComponent<FireBallOnLine>();
                        playeraction.myPlayer = this.gameObject;

                        Network.Instantiate(fireball, fireballout.transform.position, fireballout.transform.rotation, 0);

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
        }



    }

    void desativaoControles(bool desativa)
    {
        try
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
                    break;
                }

            }
        }
        catch { }

    }

    void desativaCameraScripts()
    {
        foreach (var scriptj in gameObject.GetComponentsInChildren<MonoBehaviour>())
        {


            if (scriptj.name == "Main Camera")
            {
                // scriptj.enabled = false;
                scriptj.GetComponent<Camera>().enabled = false;

                scriptj.GetComponent<AudioListener>().enabled = false;

                scriptj.GetComponentInChildren<MeshRenderer>().enabled = false;

                break;

            }

        }

        //var script1 = (MonoBehaviour)gameObject.GetComponent("PlayerActionOnLine");
        //script1.enabled = false;
    }

    void Invencivel(int TimeInvencivel)
    {

        timerInvencivel = 0;
        timeinvencivel = TimeInvencivel;
        invencivel = true;
    }




    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (GetComponent<NetworkView>().isMine)
        {
            if (gerente.gameModeOnLine == GameModeOnLine.takeflag)
            {
                if (hit.gameObject.tag == "enemy" && star)
                {
                    Debug.Log("matou com estrela");

                    SetPlacar(100, lifes, flag);
                    Network.Destroy(hit.gameObject);

                }
                else
                    if (hit.gameObject.tag == "enemy" && !invencivel)
                {



                    GetComponent<AudioSource>().PlayOneShot(bump);

                    Invencivel(1);



                    gerente.SetGameState(GameState.dieing);



                }



                if (hit.gameObject.name == "Water")
                {
                    gerente.SetGameState(GameState.dieing);


                    // desativaoControles(true);


                }

                if (hit.gameObject.tag == "bandeira")
                {

                    flag = true;
                    gerente.SetGameState(GameState.clear);

                }

                if (hit.gameObject.tag == "chao")
                {

                    nochao = true;
                    // Debug.Log("colidiu com o chao");

                }



                if (hit.gameObject.tag == "Cogumelo")
                {

                    GetComponent<AudioSource>().PlayOneShot(cogumelos);
                    SetPlacar(150, lifes, flag);
                    Network.Destroy(hit.gameObject);

                }


                if (hit.gameObject.tag == "Flor")
                {

                    GetComponent<AudioSource>().PlayOneShot(cogumelos);
                    SetPlacar(200, this.lifes, flag);
                    Network.Destroy(hit.gameObject);
                }


                if (hit.gameObject.name == "Moeda")
                {
                    GetComponent<AudioSource>().PlayOneShot(moeda);
                    SetPlacar(100, this.lifes, flag);
                    Network.Destroy(hit.gameObject);

                }

                if (hit.gameObject.tag == "Fireball")
                {

                    GetComponent<AudioSource>().PlayOneShot(bump);
                    SetPlacar(-100, this.lifes, flag);
                    Network.Destroy(hit.gameObject);

                }

                if (hit.gameObject.tag == "estrela")
                {



                    GetComponent<AudioSource>().PlayOneShot(estrela);

                    Invencivel(11);
                    star = true;
                    Network.Destroy(hit.gameObject);

                }

                //hit.gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.cyan;

                //Destroy(hit.gameObject);

                //}
            }

            if (gerente.gameModeOnLine == GameModeOnLine.timeatack &&  gerente.gameState== GameState.playing)
            {
                if (hit.gameObject.tag == "enemy" && star)
                {
                    Debug.Log("matou com estrela");

                    SetPlacar(100, lifes, flag);
                    Network.Destroy(hit.gameObject);

                }
                else
                    if (hit.gameObject.tag == "enemy" && !invencivel)
                {
                    GetComponent<AudioSource>().PlayOneShot(bump);

                    Invencivel(1);
                    SetPlacar(-100, lifes, flag);

                }



                if (hit.gameObject.name == "Water")
                {
                    gerente.SetGameState(GameState.dieing);


                    // desativaoControles(true);

                }

                if (hit.gameObject.tag == "bandeira")
                {


                }

                if (hit.gameObject.tag == "chao")
                {

                    nochao = true;
                    // Debug.Log("colidiu com o chao");

                }



                if (hit.gameObject.tag == "Cogumelo")
                {
                    //GetComponent<CharacterMotor>().jumping.extraHeight = 10;
                    SetPlacar(150, lifes, flag);

                    GetComponent<AudioSource>().PlayOneShot(cogumelos);

                    Network.Destroy(hit.gameObject);

                }


                if (hit.gameObject.tag == "Flor")
                {
                    // GetComponent<CharacterMotor>().jumping.extraHeight = 10;

                    SetPlacar(200, lifes, flag);

                    GetComponent<AudioSource>().PlayOneShot(cogumelos);

                    Network.Destroy(hit.gameObject);
                }


                if (hit.gameObject.name == "Moeda")
                {
                    GetComponent<AudioSource>().PlayOneShot(moeda);
                    SetPlacar(100, lifes, flag);
                    Network.Destroy(hit.gameObject);

                }

                if (hit.gameObject.tag == "Fireball")
                {

                    GetComponent<AudioSource>().PlayOneShot(bump);
                    SetPlacar(-100, this.lifes, flag);
                    Network.Destroy(hit.gameObject);

                }

                if (hit.gameObject.tag == "estrela")
                {


                    GetComponent<AudioSource>().PlayOneShot(estrela);

                    Invencivel(11);
                    star = true;
                    Network.Destroy(hit.gameObject);

                }

                //hit.gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.cyan;

                //Destroy(hit.gameObject);

                //}


            }
        }

    }



    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {


        Vector3 syncPosition = Vector3.zero;
        Quaternion syncRotation = Quaternion.identity;

        bool syncFireBall = false;
        float syncSpeedV = 0;
        float syncSpeedH = 0;

        bool syncStar = false;

        if (stream.isWriting)
        {
            syncPosition = transform.position;
            syncRotation = transform.rotation;
            syncSpeedH = speedH;
            syncSpeedV = speedV;
            syncFireBall = FireBall;
            syncStar = star;


            stream.Serialize(ref syncPosition);
            stream.Serialize(ref syncRotation);

            stream.Serialize(ref syncSpeedH);
            stream.Serialize(ref syncSpeedV);
            stream.Serialize(ref syncFireBall);

            stream.Serialize(ref syncStar);


        }
        else
        {
            stream.Serialize(ref syncPosition);
            stream.Serialize(ref syncRotation);

            stream.Serialize(ref syncSpeedH);
            stream.Serialize(ref syncSpeedV);
            stream.Serialize(ref syncFireBall);

            stream.Serialize(ref syncStar);


            transform.position = syncPosition;
            transform.rotation = syncRotation;

            speedH = syncSpeedH;
            speedV = syncSpeedV;
            FireBall = syncFireBall;

            star = syncStar;


            ff = GetComponentsInChildren<Animation>()[0];


            if (FireBall)
            {
                ff.Play("Armature|MyFireBall", PlayMode.StopAll);
                FireBall = false;
            }



            if (speedH > 0.5f && !ff.IsPlaying("Armature|MyFireBall"))
            {

                ff.Play("Armature|MyJump", PlayMode.StopAll);
            }
            else

                if (speedV > 0.5f && !ff.IsPlaying("Armature|MyFireBall"))
            {
                ff.Play("Armature|MyWalk", PlayMode.StopAll);
            }

            if (star && !GetComponent<AudioSource>().isPlaying)
            {

                GetComponent<AudioSource>().PlayOneShot(estrela);

            }


        }
    }

}
