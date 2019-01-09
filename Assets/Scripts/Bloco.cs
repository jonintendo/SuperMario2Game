using UnityEngine;
using System.Collections;

public class Bloco : MonoBehaviour
{


    public GameObject cogumelo;
    public GameObject flor;


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
     
    }

       


    void OnCollisionEnter(Collision collision)
    {

        Debug.Log("BOx Colidiu" + collision.gameObject.name);
        if (collision.gameObject.name == "prop_powerCube")
        {
            Destroy(collision.gameObject);
        }

        if (collision.transform.name.Equals("Cabeca") && !usado)
        {
            usado = true;
            gameObject.GetComponent<Collider>().enabled = false;


            switch (gameObject.tag)
            {
                case "moeda":
                    GetComponent<AudioSource>().PlayOneShot(moeda);
                    this.gameObject.GetComponent<Renderer>().material.mainTexture = blocoUsado;
                    break;
                case "bloco":                  
                    GetComponent<AudioSource>().PlayOneShot(destruction);
                    Destroy(this.gameObject, delayDie);
                    break;
                case "cogumelo":
                    GetComponent<AudioSource>().PlayOneShot(item);
                    this.gameObject.GetComponent<Renderer>().material.mainTexture = blocoUsado;

                    Instantiate(cogumelo, transform.position + Vector3.up, Quaternion.identity);
                    break;
                case "flor":
                    GetComponent<AudioSource>().PlayOneShot(item);
                    this.gameObject.GetComponent<Renderer>().material.mainTexture = blocoUsado;

                    Instantiate(flor, transform.position + Vector3.up, Quaternion.identity);
                    break;

            }
        }

    }


}
