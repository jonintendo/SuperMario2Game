



using UnityEngine;
using System.Collections;

public class EnemysOnLine : MonoBehaviour
{


    
    public AudioClip smashenemy;


    Quaternion rot;
    Vector3 behind;


    float timer = 0;
	// Use this for initialization
	void Start () {
      
        behind = -transform.forward;
	}
	
	
	void Update () {
        timer += Time.deltaTime;

     //   this.transform.position += this.transform.forward / 70;
       // transform.LookAt(new Vector3(player.transform.position.x,0,player.transform.position.z));


        float damping = 0.5f;
       
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);

        if ( timer>50)
        {
            timer = 0;
            behind = -1*behind;
          
        }
        rot = Quaternion.LookRotation(behind);


        if (transform.name != "Monstro2")
        {
            if (transform.name == "Tartaruga")
                this.transform.position += this.transform.forward / 200;
            else
                this.transform.position += this.transform.forward / 300;
        }

	}

    void OnTriggerEnter(Collider collision)
    {
        if (!collision.tag.Equals("enemy"))
        {
          //  Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        if (GetComponent<NetworkView>().isMine)
        {
            //if (!collision.transform.tag.Equals("chao"))
            //{
            //    Debug.Log(collision.gameObject.name);
            //}

            if (collision.transform.name.Equals("Pes"))
            {
                Debug.Log(collision.gameObject.tag);

                //GameObject Player = collision.gameObject.transform.parent.gameObject;
                //PlayerAction playeraction = Player.GetComponent<PlayerAction>();
                //playeraction.coins += 100;


                GetComponent<AudioSource>().PlayOneShot(smashenemy);
                //this.gameObject.GetComponent<Collider>().enabled = false;
                Network.Destroy(this.gameObject);
            }

            if (collision.transform.tag.Equals("Fireball"))
            {

                //GameObject Player = GameObject.FindGameObjectWithTag(collision.transform.name.Replace("(Clone)", ""));
                //PlayerActionOnLine playeraction = Player.GetComponent<PlayerActionOnLine>();
                //playeraction.coins += 100;


                // Destroy(collision.gameObject);
                GetComponent<AudioSource>().PlayOneShot(smashenemy);
                //Network.Destroy(this.gameObject,0.50f);
                Network.Destroy(this.gameObject);


            }
            else if (collision.transform.tag.Contains("layer"))
            {
                Debug.Log(collision.gameObject.tag);
                PlayerActionOnLine playerstar = collision.transform.GetComponent<PlayerActionOnLine>();
                if (playerstar.star)
                {
                    GetComponent<AudioSource>().PlayOneShot(smashenemy);
                }
            }
            else
            {

                behind = -1 * behind;
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.gameObject.tag);
    }


    //REDE////////////////////////
    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
       

        Vector3 syncPosition = Vector3.zero;

        if (stream.isWriting)
        {
            syncPosition = GetComponent<Rigidbody>().position;
            stream.Serialize(ref syncPosition);
        }
        else
        {
            stream.Serialize(ref syncPosition);
            GetComponent<Rigidbody>().position = syncPosition;
        }
    }

}
