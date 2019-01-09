using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

    
    private float timer;
	private float durationTime = 3.0f;
	private bool didDamage;
    public AudioClip fireballs;

    public GameObject Player;


    private void Awake()
    {
        GetComponent<AudioSource>().PlayOneShot(fireballs);
    }


    void Start ()
	{
        GetComponent<Rigidbody>().AddForce(transform.forward * 1000);      

    }

    void Update()
    {
       
        timer += Time.deltaTime;

        if (timer > durationTime)
        {
            Destroy(this.gameObject);
        }
    }


    void OnTriggerEnter(Collider collision)
    {
       /* if (!collision.tag.Equals("enemy"))
        {
            Destroy(this.gameObject);
        }*/
    }

    void OnCollisionEnter(Collision collision)
    {

       
       // if (!collision.gameObject.tag.Equals("chao"))
        {


            if (collision.gameObject.tag.Equals("enemy"))
            {               
                
                PlayerAction playeraction = Player.GetComponent<PlayerAction>();
                playeraction.coins += 100;
                Debug.Log("colidiu com inmigo");
            }
            // Network.Destroy(this.gameObject);
        }

    }


}
