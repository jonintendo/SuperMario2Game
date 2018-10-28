using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

    
    private float timer;
	private float durationTime = 3.0f;
	private bool didDamage;


    public GameObject myPlayer;

	void Start ()
	{
        rigidbody.AddForce(transform.forward * 1000);	
	
	}

    void Update()
    {
        //this.transform.position += this.transform.forward/7 ;

        //this.transform.position += this.transform.forward. / 7;
        //this.transform.position = new Vector3(this.transform.position.x+0.07f,this.transform.position.y,this.transform.position.z) ;

        timer += Time.deltaTime;

        if (timer > durationTime)
        {
            Destroy(this.gameObject);
        }
    }


    void OnTriggerEnter(Collider collision)
    {
        if (!collision.tag.Equals("enemy"))
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        //Debug.Log(collision.gameObject.name);
        //if (!collision.transform.tag.Equals("Bullet"))
          //  Destroy(this.gameObject);

        if (!collision.gameObject.tag.Equals("chao"))
        {


            if (collision.gameObject.tag.Equals("enemy"))
            {

                PlayerAction playeraction = myPlayer.GetComponent<PlayerAction>();
                playeraction.coins += 100;
                Debug.Log("colidiu com inmigo");
            }
            // Network.Destroy(this.gameObject);
        }

    }


}
