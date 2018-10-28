



using UnityEngine;
using System.Collections;

public class FireBallOnLine : MonoBehaviour
{


    private float timer;
    private float durationTime = 3.0f;
    private bool didDamage;

    public GameObject myPlayer;

    void Start()
    {
        rigidbody.AddForce(transform.forward * 1000);

    }

    void Update()
    {
        if (networkView.isMine)
        {
            //this.transform.position += this.transform.forward/7 ;

            //this.transform.position += this.transform.forward. / 7;
            //this.transform.position = new Vector3(this.transform.position.x+0.07f,this.transform.position.y,this.transform.position.z) ;

            timer += Time.deltaTime;

            if (timer > durationTime)
            {
                Network.Destroy(this.gameObject);
            }
        }
    }


    //void OnTriggerEnter(Collider collision)
    //{
    //    if (!collision.tag.Equals("enemy"))
    //    {
    //        Network.Destroy(this.gameObject);
    //    }
    //}

    void OnCollisionEnter(Collision collision)
    {
        if (networkView.isMine)
        {

            if (!collision.gameObject.tag.Equals("chao"))
            {


                if (collision.gameObject.tag.Equals("enemy"))
                {                   
                  
                    PlayerActionOnLine playeraction = myPlayer.GetComponent<PlayerActionOnLine>();
                    playeraction.SetPlacar(100, 0, null);
                    Debug.Log("colidiu com inmigo");
                }


                if (collision.gameObject.tag.Contains("player") && !collision.gameObject.tag.Equals(myPlayer.tag))
                {

                    PlayerActionOnLine playeraction = myPlayer.GetComponent<PlayerActionOnLine>();
                    playeraction.SetPlacar(100, 0, null);
                    Debug.Log("colidiu com Player" + collision.gameObject.tag);
                }

               // Network.Destroy(this.gameObject);
            }
        }
    }

    //REDE////////////////////////
    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 syncPosition = Vector3.zero;
        if (stream.isWriting)
        {
            syncPosition = rigidbody.position;
            stream.Serialize(ref syncPosition);
        }
        else
        {
            stream.Serialize(ref syncPosition);
            rigidbody.position = syncPosition;
        }
    }
}
