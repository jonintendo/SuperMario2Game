﻿
using UnityEngine;
using System.Collections;

public class BlocoOnLine : MonoBehaviour
{


    public GameObject cogumelo;
    public GameObject flor;
    public GameObject estrela;


    public AudioClip destruction;
    public AudioClip item;
    public AudioClip moeda;

   

    public Texture blocoUsado;

    // public GameObject Player1;

    bool usado;

    float delayDie;

    void Start()
    {
        usado = false;
        delayDie = 0.4f;
        //Player1 = GameObject.FindGameObjectWithTag("player1");
    }

  
    void Update()
    {
        //if (!usado && Mathf.Abs(Vector3.Distance(transform.position, Player1.transform.FindChild("Cabeca").transform.position)) < 2)
        //{

        //    usado = true;

        //    if (tag == "cogumelo")
        //        Instantiate(cogumelo,  transform.position + Vector3.up, Quaternion.identity);

        //    if (tag== "flor")
        //        Instantiate(flor, transform.position + Vector3.up, Quaternion.identity);
        //}

    }

    public void CreateItem()
    {
        if (tag == "cogumelo")
           Network.Instantiate(cogumelo, transform.position + Vector3.up, Quaternion.identity,1);

        if (tag == "flor")
            Network.Instantiate(flor, transform.position + Vector3.up, Quaternion.identity,1);

        if (tag == "estrela")
            Network.Instantiate(estrela, transform.position + Vector3.up, Quaternion.identity, 1);
    }


    void OnCollisionEnter(Collision collision)
    {

        Debug.Log("BOx Colidiu" + collision.gameObject.name);
        if (collision.gameObject.name == "prop_powerCube")
        {
            Network.Destroy(collision.gameObject);
        }

        if (collision.transform.name.Equals("Cabeca") && !usado)
        {
            usado = true;
            Debug.Log(collision.gameObject.tag);
           
            this.gameObject.GetComponent<Collider>().enabled = false;
            
           
            if (this.gameObject.name == "CubeRede" || this.gameObject.name == "ItemRede" || this.gameObject.name == "CubeRede2" )
            {

               
                if (this.gameObject.tag == "moeda")
                {
                    GetComponent<AudioSource>().PlayOneShot(moeda);
                    this.gameObject.GetComponent<Renderer>().material.mainTexture = blocoUsado;
                  

                }
                //else if (this.gameObject.tag == "item")
                //{
                //    audio.PlayOneShot(moeda);
                //    this.gameObject.renderer.material.mainTexture = blocoUsado;
                //    this.gameObject.name = "ite";

                //}
                else if (this.gameObject.tag == "bloco")
                {
                    Debug.Log("Player Colidiu");
                    GetComponent<AudioSource>().PlayOneShot(destruction);
                    Destroy(this.gameObject, delayDie);
                }
                else if (this.gameObject.tag == "cogumelo")
                {

                    GetComponent<AudioSource>().PlayOneShot(item);
                    this.gameObject.GetComponent<Renderer>().material.mainTexture = blocoUsado;
                  
                    CreateItem();

                }
                else if (this.gameObject.tag == "flor")
                {

                    GetComponent<AudioSource>().PlayOneShot(item);
                    this.gameObject.GetComponent<Renderer>().material.mainTexture = blocoUsado;
                   
                    CreateItem();
                }
                else if (this.gameObject.tag == "estrela")
                {

                    GetComponent<AudioSource>().PlayOneShot(item);
                    this.gameObject.GetComponent<Renderer>().material.mainTexture = blocoUsado;

                    CreateItem();
                }
                else
                {


                }
              
            }



        }

    }


}
